using SanctuarySSLib.MiscUtil;

namespace SanctuarySSLib.LuaUtil
{
    public interface IGameMetadata
    {
        string GameAppName { get; }
        string RelativeLuaFolderPath { get; }
        string FullLuaFolderPath { get; }
    }

    public class GameMetadata : IGameMetadata
    {
        private readonly ISteamInfo steamInfo;

        public GameMetadata(ISteamInfo steamInfo)
        {
            this.steamInfo = steamInfo;
            FullLuaFolderPath = Path.Combine(steamInfo.GetRoot(GameAppName),RelativeLuaFolderPath);
        }

        public string GameAppName => "Sanctuary Shattered Sun Demo";
        public string RelativeLuaFolderPath => @"prototype\RuntimeContent\Lua";
        public string FullLuaFolderPath { get; }
    }

}