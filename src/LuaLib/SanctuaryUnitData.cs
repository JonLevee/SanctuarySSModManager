
using SanctuarySSLib.WorkInProgressNotUsed;

namespace SanctuarySSModManager
{
    public class SanctuaryUnitData
    {
        private readonly ModManagerMetaData modManagerMetaData;
        private readonly LuaDataLoader luaDataLoader;

        public SanctuaryUnitData(
            ModManagerMetaData modManagerMetaData,
            LuaDataLoader luaDataLoader)
        {
            this.modManagerMetaData = modManagerMetaData;
            this.luaDataLoader = luaDataLoader;
        }
        public void Load()
        {

            //var data = luaDataLoader.LoadFolder("common\\systems\\factions.lua");
            //var factions = luaDataLoader.Load("common\\systems\\factions.lua");
        }
        /*
         * D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\host\systems\adjacencyBuffs.lua
         * D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\engine\LJ\lua\common\units\availableUnits.lua
         * D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\engine\LJ\lua\common\systems\factions.lua
         * D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\engine\LJ\lua\common\colors.lua
         */

    }
}