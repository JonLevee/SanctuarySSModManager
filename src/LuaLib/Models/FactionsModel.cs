using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager.Extensions;
using System.Collections.Specialized;
using System.Text;

namespace SanctuarySSLib.Models
{
    [SingletonService]
    public class FactionsModel : OrderedDictionary
    {
        public void Load(IGameMetadata gameMetadata, LuaValueLoader luaValueLoader)
        {
            var table1 = luaValueLoader.GetTableFromFile(RelativePath, TableName);
            var model = table1.ToModelObject();



            var luaFile = Path.Combine(gameMetadata.LuaPath, RelativePath);

            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(luaFile);
                var table = (LuaTable)lua[TableName];
                foreach (LuaTable value in table.Values)
                {
                    var name = (string)value["name"];
                    Names.Add(name);
                    Add(name, value);
                }
            }
        }

        public List<string> Names { get; } = new List<string>();
        public string RelativePath => "common/systems/factions.lua";
        public string TableName => "FactionsData";

    }
}
