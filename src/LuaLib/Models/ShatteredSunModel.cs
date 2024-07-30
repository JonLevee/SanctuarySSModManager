using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.Enums;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager;
using SanctuarySSModManager.Extensions;

namespace SanctuarySSLib.Models
{
    [SingletonService]
    public class ShatteredSunModel
    {
        private readonly LuaObjectLoader loader;

        public FactionsModel Factions { get; private set; }
        public AvailableUnitsModel AvailableUnits { get; private set; }
        public UnitsModel Units { get; private set; }

        public ShatteredSunModel(LuaObjectLoader loader)
        {
            Factions = new FactionsModel();
            AvailableUnits = new AvailableUnitsModel();
            this.loader = loader;
        }

        public void Reload()
        {
            loader.Reload(Factions);
            loader.Reload(AvailableUnits);
            loader.Reload(Units);
            Units.ForEach(kv => kv.Value.Enabled = AvailableUnits.ContainsKey(kv.Key)
                    ? AvailableUnits[kv.Key] ? UnitEnabledEnum.Enabled : UnitEnabledEnum.Disabled
                    : UnitEnabledEnum.MissingAvail);
        }

        public async void Load()
        {
            Factions = loader.Load<FactionsModel>();
            AvailableUnits = loader.Load<AvailableUnitsModel>();
            Units = loader.Load<UnitsModel>();
            Units.ForEach(kv => kv.Value.Enabled = AvailableUnits.ContainsKey(kv.Key)
                    ? AvailableUnits[kv.Key] ? UnitEnabledEnum.Enabled : UnitEnabledEnum.Disabled
                    : UnitEnabledEnum.MissingAvail);
            await Task.Run(() => Thread.Sleep(2000));
        }
    }
}
