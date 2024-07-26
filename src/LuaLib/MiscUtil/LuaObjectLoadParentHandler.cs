using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace SanctuarySSLib.MiscUtil
{
    [SingletonService]
    public class LuaObjectLoadParentHandler
    {
        private delegate bool LuaObjectLoadHandler(object instance, Type instanceType, LuaObjectAttribute attr);
        private readonly List<LuaObjectLoadHandler> loadHandlers;
        private readonly LuaObjectLoadObjectHandler loadObjectHandler;
        private readonly Dictionary<string, string> luaFiles;

        public LuaObjectLoadParentHandler(IGameMetadata metadata, LuaObjectLoadObjectHandler loadObjectHandler)
        {
            RootPath = metadata.LuaPath;
            SubPath = "common";
            loadHandlers = new List<LuaObjectLoadHandler>
            {
                TryLoadDictionaryWithSubDirectories,
                TryLoadDictionary,
                TryLoadList,
            };
            this.loadObjectHandler = loadObjectHandler;
            luaFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        public string RootPath { get; internal set; }
        public string SubPath { get; internal set; }
        public string FullPath => string.IsNullOrWhiteSpace(SubPath) ? RootPath : Path.Combine(RootPath, SubPath);

        public void Load(object instance)
        {
            var targetType = instance.GetType();
            Debug.Assert(targetType.TryGetCustomAttribute(out LuaObjectAttribute attr));
            Debug.Assert(!string.IsNullOrWhiteSpace(attr.Table));
            Debug.Assert(!string.IsNullOrWhiteSpace(attr.File));
            EnsureLuaFileLookupLoaded();

            var success = loadHandlers.Any(h => h(instance, targetType, attr));
            if (!success)
            {
                throw new InvalidOperationException($"No LuaObjectLoadParentHandler could handle targetType {targetType}");
            }

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

        private bool TryLoadDictionaryWithSubDirectories(object instance, Type instanceType, LuaObjectAttribute attr)
        {
            if (string.IsNullOrWhiteSpace(attr.Key))
            {
                return false;
            }
            if (!instanceType.GetInterfaces().Any(i => i.Name == "IDictionary"))
            {
                return false;
            }
            var keyType = instanceType.BaseType?.GenericTypeArguments[0];
            Debug.Assert(keyType != null);
            var valueType = instanceType.BaseType?.GenericTypeArguments[1];
            Debug.Assert(valueType != null);
            var dictionary = instance as IDictionary;
            Debug.Assert(dictionary != null);

            var fullPath = Path.Combine(FullPath, attr.File.Replace('/', '\\'));
            var fileList = luaFiles
                .Values
                .Where(f => f.StartsWith(fullPath, StringComparison.OrdinalIgnoreCase))
                .Order()
                .ToList();
            var keyParts = attr.Key.Split('/');
            dictionary.Clear();
            var unloadableFiles = new List<string>();
            foreach (var filePath in fileList)
            {
                using (Lua lua = new Lua())
                {
                    lua.State.Encoding = Encoding.UTF8;
                    lua.DoFile(filePath);
                    var table = (LuaTable)lua[attr.Table];
                    var value = loadObjectHandler.GetObject(table, valueType);
                    var key = (string)keyParts.Aggregate((object)table, (t, k) => ((LuaTable)t)[k]);
                    var fileKey = Path.GetFileNameWithoutExtension(filePath);
                    var parentKey = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(filePath));
                    if (fileKey != parentKey || fileKey != key || parentKey != key)
                    {
                        unloadableFiles.Add(filePath);
                        continue;
                    }
                    dictionary.Add(key, value);
                }
            }
            return true;
        }

        private bool TryLoadDictionary(object instance, Type instanceType, LuaObjectAttribute attr)
        {
            if (!instanceType.GetInterfaces().Any(i => i.Name == "IDictionary"))
            {
                return false;
            }
            Debug.Assert(luaFiles.TryGetValue(attr.File, out string? luaPath), $"Could not locate {attr.File} in directory '{FullPath}'");
            Debug.Assert(luaPath != null);
            // should be List or Dictionary
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(luaPath);
                var table = (LuaTable)lua[attr.Table];
                loadObjectHandler.LoadObject(instance, table);
            }
            return true;
        }
        private bool TryLoadList(object instance, Type instanceType, LuaObjectAttribute attr)
        {
            if (!instanceType.GetInterfaces().Any(i => i.Name == "IList"))
            {
                return false;
            }
            Debug.Assert(luaFiles.TryGetValue(attr.File, out string? luaPath), $"Could not locate {attr.File} in directory '{FullPath}'");
            Debug.Assert(luaPath != null);
            // should be List or Dictionary
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(luaPath);
                var table = (LuaTable)lua[attr.Table];
                loadObjectHandler.LoadObject(instance, table);
            }
            return true;
        }
    }
}