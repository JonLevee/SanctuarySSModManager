using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SanctuarySSLib.MiscUtil
{
    [SingletonService]
    public class LuaObjectLoader
    {
        private readonly Dictionary<string, string> luaFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public LuaObjectLoader(IGameMetadata metadata)
        {
            RootPath = metadata.LuaPath;
            SubPath = "common";
        }

        public string RootPath { get; internal set; }
        public string SubPath { get; internal set; }

        public string FullPath => string.IsNullOrWhiteSpace(SubPath) ? RootPath : Path.Combine(RootPath, SubPath);

        public T Load<T>() where T : class, new()
        {
            Debug.Assert(typeof(T).TryGetCustomAttribute(out LuaObjectAttribute attr));
            Debug.Assert(!string.IsNullOrWhiteSpace(attr.Table));
            Debug.Assert(!string.IsNullOrWhiteSpace(attr.File));

            EnsureLuaFileLookupLoaded();
            var file = attr.File.Replace('/', '\\');
            if (!string.IsNullOrWhiteSpace(attr.Key))
            {
                Debug.Assert(typeof(T).GetInterfaces().Any(i => i.Name == "IDictionary"));
                var keyType = typeof(T).BaseType?.GenericTypeArguments[0];
                Debug.Assert(keyType != null);
                var valueType = typeof(T).BaseType?.GenericTypeArguments[1];
                Debug.Assert(valueType != null);
                var rootDir = Path.Combine(FullPath, file);
                var fileList = luaFiles
                    .Values
                    .Where(f => f.StartsWith(rootDir, StringComparison.OrdinalIgnoreCase))
                    .Order()
                    .ToList();
                var keyParts = attr.Key.Split('/');
                var instance = Activator.CreateInstance<T>();
                var unloadableFiles = new List<string>();
                foreach (var filePath in fileList)
                {
                    using (Lua lua = new Lua())
                    {
                        lua.State.Encoding = Encoding.UTF8;
                        lua.DoFile(filePath);
                        var table = (LuaTable)lua[attr.Table];
                        var value = GetObject(table, valueType);
                        var key = (string)keyParts.Aggregate((object)table, (t, k) => ((LuaTable)t)[k]);
                        var fileKey = Path.GetFileNameWithoutExtension(filePath);
                        var parentKey = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(filePath));
                        if (fileKey != parentKey || fileKey != key || parentKey != key)
                        {
                            unloadableFiles.Add(filePath);
                            continue;
                        }
                        ((IDictionary)instance).Add(key, value);
                    }
                }
                return instance;
            }


            Debug.Assert(luaFiles.TryGetValue(attr.File, out string? luaPath), $"Could not locate {attr.File} in directory '{FullPath}'");
            Debug.Assert(luaPath != null);
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(luaPath);
                var table = (LuaTable)lua[attr.Table];
                var instance = (T)GetObject(table, typeof(T));
                return instance;
            }
        }

        private object GetObject(object luaObject, Type targetType)
        {
            if (targetType == typeof(string) || targetType.IsValueType)
            {
                var value = Convert.ChangeType(luaObject, targetType);
                return value;
            }
            var table = luaObject as LuaTable;
            Debug.Assert(table != null);
            if (targetType.GetInterfaces().Any(i => i.Name == "IList"))
            {
                var itemType = targetType?.BaseType?.GenericTypeArguments[0];
                Debug.Assert(itemType != null);
                var list = Activator.CreateInstance(targetType) as IList;
                Debug.Assert(list != null);
                foreach (var value in table.Values)
                {
                    list.Add(GetObject(value, itemType));
                }
                return list;
            }
            if (targetType.GetInterfaces().Any(i => i.Name == "IDictionary"))
            {
                var dictionary = Activator.CreateInstance(targetType) as IDictionary;
                Debug.Assert(dictionary != null);
                var keyType = targetType.BaseType?.GenericTypeArguments[0];
                Debug.Assert(keyType != null);
                var valueType = targetType.BaseType?.GenericTypeArguments[1];
                Debug.Assert(valueType != null);
                foreach (KeyValuePair<object, object> kv in table)
                {
                    var key = GetObject(kv.Key, keyType);
                    var value = GetObject(kv.Value, valueType);
                    dictionary.Add(key, value);
                }
                return dictionary;
            }
            var instance = Activator.CreateInstance(targetType);
            Debug.Assert(instance != null);
            var common = targetType
                .GetProperties()
                .Join(
                    table.Keys.Cast<string>(),
                    p => p.Name,
                    k => k,
                    (property, key) => new { property, key },
                    StringComparer.OrdinalIgnoreCase);
            foreach (var item in common)
            {
                var value = GetObject(table[item.key], item.property.PropertyType);
                item.property.SetValue(instance, value);
            }
            return instance;
        }

        private void EnsureLuaFileLookupLoaded()
        {
            if (luaFiles.Any())
            {
                return;
            }
            var fileFilters = new string[] { "*.lua", "*.santp" };
            foreach (var file in fileFilters.SelectMany(ff => Directory.GetFiles(FullPath, ff, SearchOption.AllDirectories)))
            {
                luaFiles.Add(Path.GetFileName(file), file);
            }
        }
    }

}
