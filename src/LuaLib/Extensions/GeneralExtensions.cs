using KeraLua;
using Microsoft.Extensions.DependencyInjection;
using NLua;
using SanctuarySSLib.Enums;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SanctuarySSModManager.Extensions
{
    public static class GeneralExtensions
    {
        public static void EnsureFileDirectoryExists(this string file)
        {
            if (null == file) throw new ArgumentNullException(nameof(file));
            var directoryName = Path.GetDirectoryName(file);
            Debug.Assert(directoryName != null);
            var stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(directoryName));
            while (stack.Any())
            {
                var directory = stack.Pop();
                Debug.Assert(directory.Parent != null);
                if (!directory.Parent.Exists)
                {
                    stack.Push(directory);
                    stack.Push(directory.Parent);
                    continue;
                }
                if (!directory.Exists)
                {
                    Directory.CreateDirectory(directory.FullName);
                }

            }
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

        public static ICollection<T> ToCollection<T>(this ICollection values)
        {
            var collection = new List<T>();
            foreach (var item in values)
            {
                collection.Add((T)item);
            }
            return collection;
        }

        public static bool MirrorFrom(this IDictionary target, IDictionary source)
        {
            var keysToAdd = source.Keys.Cast<object>().Where(k => !target.Contains(k)).ToList();
            var keysToRemove = target.Keys.Cast<object>().Where(k => !source.Contains(k)).ToList();
            var intersectedValuesDifferent = source
                .Keys
                .Cast<object>()
                .Where(target.Contains)
                .Where(k => target[k] != source[k])
                .ToList();
            foreach (var key in keysToAdd.Union(intersectedValuesDifferent))
            {
                target[key] = source[key];
            }
            foreach (var key in keysToRemove)
            {
                target.Remove(key);
            }
            return keysToAdd.Any() || keysToRemove.Any() || intersectedValuesDifferent.Any();
        }
    }
}
