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
    public class FactionsModel : ModelObject
    {
        public void Load(IGameMetadata gameMetadata, LuaValueLoader luaValueLoader)
        {
            var table = luaValueLoader.GetModelFromFile(RelativePath, TableName);
            foreach (KeyValuePair<object, ModelObject> kv in table)
            {
                Add(kv);
                Names.Add(kv.Value["name"].Text);
            }
        }

        public List<string> Names { get; } = new List<string>();
        public string RelativePath => "common/systems/factions.lua";
        public string TableName => "FactionsData";

    }
}
