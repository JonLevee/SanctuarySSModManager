
using Microsoft.Extensions.DependencyInjection;
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
    public class SSSUserSettings
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        public string FileName { get; }
        public SSSUserSettings()
        {
            ShatteredSunDirectoryRoot = string.Empty;
            ModRootFolder = string.Empty;
            AppDataFolder = string.Empty;
        }

        public static SSSUserSettings CreateInstance(IServiceProvider serviceProvider)
        {
            IGameMetadata gameMetaData = serviceProvider.GetService<IGameMetadata>();
            SSSUserSettings settings = new SSSUserSettings();
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ApplicationException("Could not locate AssemblyProductAttribute");
            }
            var appFolder = Path.Combine(appDataFolder, appName);
            appFolder.EnsureDirectoryExists();
            var settingsFile = Path.Combine(appFolder, $"{typeof(SSSUserSettings).Name}.json");
            if (!File.Exists(settingsFile))
            {
                settings.ShatteredSunDirectoryRoot = gameMetaData.LuaPath;
                settings.ModRootFolder = @"prototype\RuntimeContent\Lua";
                settings.AppDataFolder = appFolder;

                var json = JsonSerializer.Serialize(settings, serializerOptions);
                File.WriteAllText(settingsFile, json);
            }
            else
            {
                string json = File.ReadAllText(settingsFile);
                settings = JsonSerializer.Deserialize<SSSUserSettings>(json) ?? settings;
            }
            return settings;

        }

        public string ShatteredSunDirectoryRoot { get; set; }
        public string ModRootFolder { get; set; }

        public string AppDataFolder { get; set; }

        public string FullModRootFolder => Path.Combine(ShatteredSunDirectoryRoot, ModRootFolder);
    }
}
