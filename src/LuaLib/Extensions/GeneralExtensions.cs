using KeraLua;
using Microsoft.Extensions.DependencyInjection;
using NLua;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SanctuarySSModManager.Extensions
{
    public static class GeneralExtensions
    {
        public static void EnsureDirectoryExists(this string? directory)
        {
            if (null == directory) throw new ArgumentNullException(nameof(directory));
            var info = new DirectoryInfo(directory);
            if (info.Exists)
            {
                return;
            }
            if (info.FullName.Equals(info.Root.FullName, StringComparison.OrdinalIgnoreCase))
            {
                throw new DirectoryNotFoundException($"Directory root {info.Root.FullName} does not exist");
            }
            EnsureDirectoryExists(info.Parent?.FullName);
            Directory.CreateDirectory(info.FullName);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static string CamelCase(this string text)
        {
            if (!string.IsNullOrEmpty(text) && char.IsUpper(text[0]))
            {
                text = char.ToLower(text[0]) + text[1..];
            }
            return text;
        }

        public static ModelObject ToModelObject(this object value)
        {
            var instance = new ModelObject();
            switch (value)
            {
                case LuaTable table:
                    foreach (KeyValuePair<object, object> kv in table)
                    {
                        instance.Add(kv.Key, kv.Value.ToModelObject());
                    }
                    break;
                case string:
                case long:
                case bool:
                case double:
                    instance.Value = value;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return instance;
        }

        public static ICollection<T> ToCollection<T>(this ICollection values)
        {
            var collection = new List<T>();
            foreach (var item in values)
            {
                collection.Add((T)item);
            }
            return collection;
        }

        public static bool TryGetCustomAttribute<T>(this Type type, out T attr) where T : Attribute
        {
            attr = type.GetCustomAttribute<T>();
            return attr != null;
        }

        private static ShatteredSunModel shatteredSunModel;
        public static UnitEnabledEnum GetEnabled(this UnitModel unitModel)
        {
            if (null == shatteredSunModel)
            {
                shatteredSunModel = DIContainer.Services.GetService<ShatteredSunModel>();
            }
            if (shatteredSunModel.AvailableUnits.TryGetValue(unitModel.General.TpId, out bool enabled))
            {
                return enabled ? UnitEnabledEnum.Enabled : UnitEnabledEnum.Disabled;
            }
            return UnitEnabledEnum.MissingAvail;
        }
    }
}
