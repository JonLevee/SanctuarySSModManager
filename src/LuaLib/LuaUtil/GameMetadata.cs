using SanctuarySSLib.Attributes;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager.Extensions;

namespace SanctuarySSLib.LuaUtil
{
    [SingletonService]
    public interface IGameMetadata
    {
        string GameAppName { get; }
        string GameRoot { get; set; }
        string SelectedLuaFolder { get; set; }
        IEnumerable<string> LuaFolders { get; }
        string LuaPath { get; }
        string GetFullPath(string relativePath);
        bool Refresh();
    }

    public class GameMetadata : IGameMetadata
    {
        private readonly ISteamInfo steamInfo;
        private readonly Dictionary<string, string> luaFolders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly string[] expectedLuaSubdirectories = ["client", "common", "host", "maps"];
        //private readonly string defaultLuaFolder = "prototype";

        public GameMetadata(ISteamInfo steamInfo)
        {
            this.steamInfo = steamInfo;
            SelectedLuaFolder = string.Empty;
            Refresh();
        }

        public string GameAppName => "Sanctuary Shattered Sun Demo";
        public string GameRoot { get; set; }
        public string SelectedLuaFolder { get; set; }
        public string LuaPath => luaFolders[SelectedLuaFolder];

        public IEnumerable<string> LuaFolders => luaFolders.Keys.Order();

        public string GetFullPath(string relativePath)
        {
            var path = Path.Combine(LuaPath, relativePath);
            path = path.Replace('/', '\\');
            if (Path.HasExtension(path))
                Constraint.FileExists(path);
            else
                Constraint.DirectoryExists(path);
            return path;
        }

        public bool Refresh()
        {
            Dictionary<string, string> currentFolders = new Dictionary<string, string>();
            GameRoot = steamInfo.GetRoot(GameAppName);
            foreach (var folder in Directory.GetDirectories(GameRoot, "lua", SearchOption.AllDirectories))
            {
                var subDirs = Directory.GetDirectories(folder).Select(d => d.Substring(folder.Length + 1)).ToArray();
                if (expectedLuaSubdirectories.All(subDirs.Contains))
                {
                    var name = folder.Substring(GameRoot.Length + 1).Split('\\')[0];
                    currentFolders[name] = folder;
                }
            }

            var updated = luaFolders.MirrorFrom(currentFolders);

            if (string.IsNullOrWhiteSpace(SelectedLuaFolder) || !luaFolders.ContainsKey(SelectedLuaFolder))
            {
                SelectedLuaFolder = luaFolders.Keys.FirstOrDefault() ?? string.Empty;
            }
            return updated;
        }
    }

}