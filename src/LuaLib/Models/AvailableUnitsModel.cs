using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using System.Text;

namespace SanctuarySSLib.Models
{
    [LuaObject(File = "availableUnits.lua", Table = "AvailableUnits")]
    public class AvailableUnitsModel : Dictionary<string, bool>
    {
        public AvailableUnitsModel() : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }

}
