using Microsoft.Extensions.DependencyInjection;
using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.Models;
using SanctuarySSModManager.Extensions;

namespace SanctuarySSLib.ViewModel
{
    [SingletonService]
    public class ShatteredSunViewModel
    {
        public FactionsViewModel Factions { get; }
        public UnitsViewModel Units { get; }
        public ShatteredSunViewModel(ShatteredSunModel ssModel)
        {
            Factions = new FactionsViewModel(ssModel.Factions);
            Units = new UnitsViewModel(ssModel.Units);
        }
    }
}
