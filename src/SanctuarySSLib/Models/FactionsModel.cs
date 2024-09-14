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

        public FactionModel()
        {
            Name = string.Empty;
            TPLetter = string.Empty;
            Tag = string.Empty;
            InitialUnit = string.Empty;
        }
        public override string ToString()
        {
            return string.Concat(GetType().GetProperties().Select(p => $"{p.Name}={p.GetValue(this) ?? "(null)"} "));
        }
    }
}
