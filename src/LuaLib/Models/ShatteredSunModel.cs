using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager;
using SanctuarySSModManager.Extensions;

namespace SanctuarySSLib.Models
{
    [SingletonService]
    public class ShatteredSunModel
    {
        public FactionsModel Factions { get; private set; }
        public AvailableUnitsModel AvailableUnits { get; private set; }
        public UnitsModel Units { get; private set; }

        public ShatteredSunModel()
        {
            Factions = new FactionsModel();
            AvailableUnits = new AvailableUnitsModel();
        }

        public void Load()
        {
            var loader = DIContainer.Services.GetService<LuaObjectLoader>();
            Factions = loader.Load<FactionsModel>();
            AvailableUnits = loader.Load<AvailableUnitsModel>();
            Units = loader.Load<UnitsModel>();
            //var missingAvailKeys = Units.Keys.Where(k => !AvailableUnits.ContainsKey(k)).ToList();
            //var missingUnitKeys = AvailableUnits.Keys.Where(k => !Units.ContainsKey(k)).ToList();
            //foreach (var kv in Units)
            //{
            //    kv.Value.Enabled = AvailableUnits.ContainsKey(kv.Key)
            //        ? AvailableUnits[kv.Key] ? UnitEnabledEnum.Enabled : UnitEnabledEnum.Disabled
            //        : UnitEnabledEnum.MissingAvail;
            //}
            //var results = Units.GroupBy(u => u.Value.Enabled).ToDictionary(g => g.Key, g => g.OrderBy(u => u.Key).ToList());
        }

    }
}
