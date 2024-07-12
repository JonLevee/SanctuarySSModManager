using SanctuarySSLib.MiscUtil;

namespace SanctuarySSLib.LuaUtil
{
    public interface IGameMetadata
    {
        string GameAppName { get; }
        string DefaultLuaFolder { get; set; }
        IEnumerable<string> LuaFolders { get; }
        string GetLuaPath(string luaFolder);
    }

    public class GameMetadata : IGameMetadata
    {
        private readonly ISteamInfo steamInfo;
        private readonly Dictionary<string, string> luaFolders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public GameMetadata(ISteamInfo steamInfo, string defaultLuaFolder = "prototype")
        {
            this.steamInfo = steamInfo;
            DefaultLuaFolder = defaultLuaFolder;
        }

        public string GameAppName => "Sanctuary Shattered Sun Demo";
        public string DefaultLuaFolder { get; set; }

        public IEnumerable<string> LuaFolders
        {
            get
            {
                EnsureLuaFoldersPopulated();
                return luaFolders.Keys;
            }
        }


        public string GetLuaPath(string luaFolder)
        {
            EnsureLuaFoldersPopulated();
            return luaFolders[luaFolder];
        }

        private void EnsureLuaFoldersPopulated()
        {
            if (!luaFolders.Any())
            {
                var gameRoot = steamInfo.GetRoot(GameAppName);
                foreach (var folder in Directory.GetDirectories(gameRoot, "lua", SearchOption.AllDirectories))
                {
                    var name = folder.Substring(gameRoot.Length + 1).Split('\\')[0];
                    luaFolders[name] = folder;
                }

                if (string.IsNullOrWhiteSpace(DefaultLuaFolder) || !luaFolders.ContainsKey(DefaultLuaFolder))
                {
                    DefaultLuaFolder = luaFolders.Keys.FirstOrDefault() ?? string.Empty;
                }
            }
        }
    }

}