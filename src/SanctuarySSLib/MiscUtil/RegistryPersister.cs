using Microsoft.Win32;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace SanctuarySSLib.MiscUtil
{
    public class RegistryPersister : IObjectPersister
    {
        private static readonly Type[] supportedTypes = [
            typeof(bool),
            typeof(string),
            typeof(int),
            typeof(long),
            typeof(double),
            typeof(DateTime),
            ];
        private static readonly Assembly thisAssembly = Assembly.GetExecutingAssembly();
        private readonly AppInfo appInfo;
        private readonly RegistryKey registryKey;

        public RegistryPersister(AppInfo appInfo)
        {
            Contract.Assert(appInfo != null);
            this.appInfo = appInfo;
            this.registryKey = Registry.CurrentUser;
        }
        public T Load<T>(string name) where T : class, new()
        {
            var item = new T();
            LoadProperties(GetAppKey(registryKey, name), item);
            return item;
        }

        public void Save<T>(T instance, string name)
        {
            Contract.Assert(instance != null);
            SaveProperties(GetAppKey(registryKey, name), instance);
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
                .Where(p => p.CanRead && p.CanWrite)
                .ToList();
            foreach (var p in properties)
            {
                if (supportedTypes.Contains(p.PropertyType) || p.PropertyType.IsEnum)
                {
                    var value = p.GetValue(instance, null);
                    if (value != null && value.ToString() is string textValue)
                    {
                        key.SetValue(p.Name, textValue, RegistryValueKind.String);
                    }
                    continue;
                }
                if (p.PropertyType.IsClass && p.PropertyType.Assembly == thisAssembly)
                {
                    SaveProperties(key.CreateSubKey(p.Name), p.GetValue(instance, null));
                }
            }
        }
        private RegistryKey GetAppKey(RegistryKey key, string name)
        {
            return key.CreateSubKey("Software").CreateSubKey(appInfo.AppName).CreateSubKey(name);
        }
    }
}
