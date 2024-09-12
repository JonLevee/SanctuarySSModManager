using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SanctuarySSLib.RegistryClasses
{
    public class RegistryPersister
    {
        private static readonly Type[] supportedTypes = [
            typeof(string),
            typeof(int),
            typeof(long),
            typeof(double),
            typeof(DateTime),
            ];
        private static readonly Assembly thisAssembly = Assembly.GetExecutingAssembly();
        private readonly RegistryKey registryKey;

        public RegistryPersister() 
        {
            var appName = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            Contract.Assert(appName != null);
            registryKey = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey(appName);
        }
        public T LoadFromRegistry<T>(string name) where T : class, new()
        {
            var key = registryKey.CreateSubKey(name);
            var item = new T();
            LoadProperties(key, item);
            return item;
        }

        public void SaveToRegistry<T>(T instance, string name)
        {
            Contract.Assert(instance != null);
            var key = registryKey.CreateSubKey(name);
            SaveProperties(key, instance);
        }

        private static void LoadProperties(RegistryKey key, object? instance)
        {
            if (instance == null || key == null)
            {
                return;
            }
            Contract.Assert(instance != null);
            Contract.Assert(key != null);
            var properties = instance
                .GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .ToList();
            foreach (var p in properties)
            {
                if (supportedTypes.Contains(p.PropertyType) || p.PropertyType.IsEnum)
                {
                    var value = key.GetValue(p.Name);
                    if (value is string text)
                    {
                        if (p.PropertyType.IsEnum)
                            p.SetValue(instance, Enum.Parse(p.PropertyType, text));
                        else
                            p.SetValue(instance, Convert.ChangeType(value, p.PropertyType));
                    }
                    continue;
                }
                if (p.PropertyType.IsClass && p.PropertyType.Assembly == thisAssembly)
                {
                    LoadProperties(key.CreateSubKey(p.Name), p.GetValue(instance, null));
                }
            }
        }

        private static void SaveProperties(RegistryKey key, object? instance)
        {
            if (instance == null)
            {
                return;
            }
            Contract.Assert(instance != null);
            Contract.Assert(key != null);
            var keyNames = key.GetValueNames();
            var properties = instance
                .GetType()
                .GetProperties()
                .Where(p=>p.CanRead && p.CanWrite)
                .ToList();
            foreach (var p in properties)
            {
                if (supportedTypes.Contains(p.PropertyType) || p.PropertyType.IsEnum)
                {
                    var value = p.GetValue(instance, null);
                    if (value != null)
                    {
                        Contract.Assert(value != null);
                        key.SetValue(p.Name, value.ToString(), RegistryValueKind.String);
                    }
                    continue;
                }
                if (p.PropertyType.IsClass && p.PropertyType.Assembly == thisAssembly)
                {
                    SaveProperties(key.CreateSubKey(p.Name), p.GetValue(instance, null));
                }
            }
        }
    }
}
