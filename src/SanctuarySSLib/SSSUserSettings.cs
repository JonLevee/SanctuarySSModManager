using SanctuarySSLib.MiscUtil;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SanctuarySSModManager
{
    public class SSSUserSettings
    {
        public bool DisplayedSetupMessage { get; set; }


    }

    public class SSSModManagerSettings
    {
        public string LuaFolder { get; set; } = string.Empty;
        public bool ModManagerEnabled { get; set; }
    }

    public class SSSCombinedSettings : INotifyPropertyChanged
    {
        private readonly SSSModManagerSettings modManagerSettings;
        private readonly SSSUserSettings userSettings;
        private readonly AppInfo appInfo;
        private readonly ModifySSSApp modifySSSApp;
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
        public bool ModManagerEnabled
        {
            get => modManagerSettings.ModManagerEnabled;
            set
            {
                modifySSSApp.ModManagerEnablement(value);
                Notify();
            }
        }

        public string ShatteredSunInstallRoot => appInfo.ShatteredSunInstallRoot;
            
        public string FullModRootFolder => Path.Combine(ShatteredSunInstallRoot, luaRoots[modManagerSettings.LuaFolder]);

        public SSSCombinedSettings(
            SSSModManagerSettings modManagerSettings, 
            SSSUserSettings userSettings,
            AppInfo appInfo,
            ModifySSSApp modifySSSApp)
        {
            this.modManagerSettings = modManagerSettings;
            this.userSettings = userSettings;
            this.appInfo = appInfo;
            this.modifySSSApp = modifySSSApp;
            if (string.IsNullOrWhiteSpace(modManagerSettings.LuaFolder))
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
