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

        }

        public void Load()
        {
            var gameMetadata = DIContainer.Services.GetService<IGameMetadata>();
            var luaValueLoader = DIContainer.Services.GetService<LuaValueLoader>();

            Factions = DIContainer.Services.GetService<FactionsModel>();
            Factions.Load(gameMetadata, luaValueLoader);

            AvailableUnits = DIContainer.Services.GetService<AvailableUnitsModel>();
            AvailableUnits.Load(gameMetadata, luaValueLoader);

            Units = DIContainer.Services.GetService<UnitsModel>();
            Units.Load(gameMetadata, luaValueLoader);

        }
    }
}
