using KeraLua;
using NLua;
using SanctuarySSLib.LuaUtil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static ModelObject ToModelObject(this LuaTable luaTable)
        {
            throw new NotImplementedException();
            var instance = new ModelObject();
            foreach (var kv in luaTable)
            {

            }

            return instance;
        }
    }
}
