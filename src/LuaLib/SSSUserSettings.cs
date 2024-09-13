
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.Enums;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SanctuarySSModManager
{
    public class SSSUserSettings
    {
        public bool DisplayedSetupMessage { get; set; }


    }

    public class SSSModManagerSettings
    {
        public string LuaFolder { get; set; }
    }

    public class SSSCombinedSettings : INotifyPropertyChanged
    {
        private readonly SSSModManagerSettings modManagerSettings;
        private readonly SSSUserSettings userSettings;
        private readonly Dictionary<string, string> luaRoots = new Dictionary<string, string>
        {
            { "engine", @"engine\LJ\lua" },
            { "prototype", @"prototype\RuntimeContent\Lua" }
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public string[] LuaFolders => luaRoots.Keys.ToArray();
        public string LuaFolder 
        { 
            get => modManagerSettings.LuaFolder;
            set
            {
                modManagerSettings.LuaFolder = value;
                Notify();
            }
        }
        public string ShatteredSunInstallRoot { get; }
            
        public string FullModRootFolder => Path.Combine(ShatteredSunInstallRoot, luaRoots[modManagerSettings.LuaFolder]);

        public SSSCombinedSettings(
            SSSModManagerSettings modManagerSettings, 
            SSSUserSettings userSettings,
            ISteamInfo steamInfo)
        {
            this.modManagerSettings = modManagerSettings;
            this.userSettings = userSettings;
            ShatteredSunInstallRoot = steamInfo.GetRoot("Sanctuary Shattered Sun Demo");
            if (string.IsNullOrEmpty(modManagerSettings.LuaFolder))
            {
                modManagerSettings.LuaFolder = LuaFolders.First();
            }
        }

        private void Notify([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
