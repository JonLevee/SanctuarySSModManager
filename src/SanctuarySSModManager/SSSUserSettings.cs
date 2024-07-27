
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SanctuarySSModManager
{
    [SingletonService]
    public class SSSUserSettings
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        private string settingsFilePath;

        public SSSUserSettings()
        {
            var gameMetaData = DIContainer.Get<IGameMetadata>();
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ApplicationException("Could not locate AssemblyProductAttribute");
            }
            var appFolder = Path.Combine(appDataFolder, appName);
            settingsFilePath = Path.Combine(appFolder, $"{typeof(SSSUserSettings).Name}.json");

            ShatteredSunDirectoryRoot = string.Empty;
            ModRootFolder = string.Empty;
            AppDataFolder = string.Empty;
        }

        public void Load()
        {
            if (!File.Exists(settingsFilePath))
            {
                return;
            }
            string json = File.ReadAllText(settingsFilePath);
            var settings = JsonSerializer.Deserialize<SSSUserSettings>(json);
            if (settings != null)
            {
                foreach (var property in GetType().GetProperties())
                {
                    if (property.CanWrite)
                    {
                        property.SetValue(this, property.GetValue(settings));
                    }
                }
            }
        }

        public void Save()
        {
            settingsFilePath.EnsureFileDirectoryExists();
            var json = JsonSerializer.Serialize(this, serializerOptions);
            File.WriteAllText(settingsFilePath, json);
        }

        public string ShatteredSunDirectoryRoot { get; set; }
        public string ModRootFolder { get; set; }

        public string AppDataFolder { get; set; }

        public string FullModRootFolder => Path.Combine(ShatteredSunDirectoryRoot, ModRootFolder);
    }
}
