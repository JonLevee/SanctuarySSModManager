using SanctuarySSLib.MiscUtil;

namespace SanctuarySSLib
{
    public class CollectionModelMetadata
    {
        private readonly SteamInfo steamInfo;

        public string RelativeLuaFolder { get; }

        public CollectionModelMetadata(SteamInfo steamInfo)
        {
            this.steamInfo = steamInfo;
            RelativeLuaFolder = @"prototype\RuntimeContent\Lua";
        }
    }
}
