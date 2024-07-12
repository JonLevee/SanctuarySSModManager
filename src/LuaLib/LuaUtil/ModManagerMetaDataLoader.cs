﻿using SanctuarySSModManager;
using SanctuarySSModManager.Extensions;
using System.Reflection;
using System.Text.Json;

namespace SanctuarySSLib.LuaUtil
{
    public class ModManagerMetaDataLoader : IModManagerMetaDataLoader
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        public void Load(ModManagerMetaData data)
        {
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
                data.ShatteredSunDirectoryRoot = steamInfo.GetRoot("Sanctuary Shattered Sun Demo");
                data.ModRootFolder = @"prototype\RuntimeContent\Lua";
                data.ModManagerFolder = appFolder;

                var json = JsonSerializer.Serialize(data, serializerOptions);
                File.WriteAllText(settingsFile, json);
            }
            else
            {
                string json = File.ReadAllText(settingsFile);
                var settings = JsonSerializer.Deserialize<ModManagerMetaData>(json);
                data.ShatteredSunDirectoryRoot = settings.ShatteredSunDirectoryRoot;
                data.ModRootFolder = settings.ModRootFolder;
                data.ModManagerFolder = settings.ModManagerFolder;
            }

        }
    }

}