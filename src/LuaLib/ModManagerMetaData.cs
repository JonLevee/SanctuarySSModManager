using SanctuarySSModManager.Extensions;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SanctuarySSModManager
{

    public class ModManagerMetaData
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        public static ModManagerMetaData CreateInstance(IServiceProvider serviceProvider)
        {
            ModManagerMetaData settings = new ModManagerMetaData();
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ApplicationException("Could not locate AssemblyProductAttribute");
            }
            var appFolder = Path.Combine(appDataFolder, appName);
            appFolder.EnsureDirectoryExists();
            var settingsFile = Path.Combine(appFolder, "ModManagerMetaData.json");
            if (!File.Exists(settingsFile))
            {
                var steamInfo = new SteamInfo();
                settings.ShatteredSunDirectoryRoot = steamInfo.GetRoot("Sanctuary Shattered Sun Demo");
                settings.ModRootFolder = @"prototype\RuntimeContent\Lua";
                settings.ModManagerFolder = appFolder;

                var json = JsonSerializer.Serialize(settings, serializerOptions);
                File.WriteAllText(settingsFile, json);
            }
            else
            {
                string json = File.ReadAllText(settingsFile);
                settings = JsonSerializer.Deserialize<ModManagerMetaData>(json) ?? settings;
            }
            return settings;
        }

        public ModManagerMetaData()
        {
            ShatteredSunDirectoryRoot = string.Empty;
            ModRootFolder = string.Empty;
            ModManagerFolder = string.Empty;
        }

        public string ShatteredSunDirectoryRoot { get; set; }
        public string ModRootFolder { get; set; }

        public string ModManagerFolder { get; set; }

        public string FullModRootFolder => Path.Combine(ShatteredSunDirectoryRoot, ModRootFolder);
    }
}