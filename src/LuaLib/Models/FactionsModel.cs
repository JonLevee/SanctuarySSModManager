using SanctuarySSLib.Attributes;
using System.Diagnostics;

namespace SanctuarySSLib.Models
{
    [LuaObject(File = "factions.lua", Table = "FactionsData")]
    public class FactionsModel : List<FactionModel>
    {

    }

    [DebuggerDisplay("{ToString()}")]
    public class FactionModel
    {
        public string Name { get; set; }
        public string TPLetter { get; set; }
        public string Tag { get; set; }
        public string InitialUnit { get; set; }

        public override string ToString()
        {
            return string.Concat(GetType().GetProperties().Select(p => $"{p.Name}={p.GetValue(this) ?? "(null)"} "));
        }
    }
}
