using SanctuarySSLib.Attributes;
using SanctuarySSLib.MiscUtil;

namespace SanctuarySSLib.LuaUtil
{
    [SingletonService]
    public interface IGameMetadata
    {
        string GameAppName { get; }
        string LuaFolder { get; set; }
        IEnumerable<string> LuaFolders { get; }
        string LuaPath { get; }
        string GetFullPath(string relativePath);
    }

    [DefaultService<IGameMetadata>]
    public class GameMetadata : IGameMetadata
    {
        private readonly ISteamInfo steamInfo;
        private readonly Dictionary<string, string> luaFolders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly string[] expectedLuaSubdirectories = ["client", "common", "host", "maps"];
        //private readonly string defaultLuaFolder = "prototype";
        private readonly string defaultLuaFolder = "engine";

        public GameMetadata(ISteamInfo steamInfo)
        {
            this.steamInfo = steamInfo;
            LuaFolder = defaultLuaFolder;
            var gameRoot = steamInfo.GetRoot(GameAppName);
            foreach (var folder in Directory.GetDirectories(gameRoot, "lua", SearchOption.AllDirectories))
            {
                var subDirs = Directory.GetDirectories(folder).Select(d => d.Substring(folder.Length + 1)).ToArray();
                if (expectedLuaSubdirectories.All(subDirs.Contains))
                {
                    var name = folder.Substring(gameRoot.Length + 1).Split('\\')[0];
                    luaFolders[name] = folder;
                }
            }

            if (string.IsNullOrWhiteSpace(LuaFolder) || !luaFolders.ContainsKey(LuaFolder))
            {
                LuaFolder = luaFolders.Keys.FirstOrDefault() ?? string.Empty;
            }
        }

        public string GameAppName => "Sanctuary Shattered Sun Demo";
        public string LuaFolder { get; set; }
        public string LuaPath => luaFolders[LuaFolder];

        public IEnumerable<string> LuaFolders => luaFolders.Keys;

        public string GetFullPath(string relativePath)
        {
            var path = Path.Combine(LuaPath, relativePath);
            path = path.Replace('/', '\\');
            return path;
        }
    }

}