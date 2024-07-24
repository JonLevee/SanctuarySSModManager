using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using System.Text;

namespace SanctuarySSLib.Models
{
    [SingletonService]
    public class AvailableUnitsModel : Dictionary<string, bool>
    {
        public void Load(
        IGameMetadata gameMetadata,
        LuaValueLoader luaValueLoader
        )
        {
            var luaFile = gameMetadata.GetFullPath(RelativePath);
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(luaFile);
                var table = (LuaTable)lua[TableName];
                foreach (var key in table.Keys)
                {
                    Add((string)key, (bool)table[key]);
                }
            }
        }

        public string RelativePath => "common/units/availableUnits.lua";
        public string TableName => "AvailableUnits";

    }

}
