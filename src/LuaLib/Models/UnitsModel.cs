using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.Enums;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager.Extensions;
using System;
using System.Diagnostics;
using System.Text;

namespace SanctuarySSLib.Models
{
    [LuaObject(File = "units/unitsTemplates", Table = "UnitTemplate", Key = "general/tpId")]
    public class UnitsModel : Dictionary<string, UnitModel>
    {
        public UnitsModel() : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }

    [DebuggerDisplay("{DebugString}")]
    public class UnitModel
    {
        public UnitEnabledEnum Enabled { get; set; }
        public bool IsEnabled => Enabled == UnitEnabledEnum.Enabled;
        public UnitDefenceModel Defence { get; set; }
        public UnitGeneralModel General { get; set; }
        public string DebugString => $"[{General.TpId}] {General.Name} ({General.DisplayName})";
    }
    public class UnitDefenceModel
    {
        public UnitHealthModel Health { get; set; }
    }
    public class UnitHealthModel
    {
        public int Max { get; set; }
        public int Value { get; set; }
    }
    public class UnitGeneralModel
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string TpId { get; set; }
    }
}
