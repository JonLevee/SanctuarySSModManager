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
        public FactionsViewModel Factions { get; private set; }
        public UnitsViewModel Units { get; private set; }
        public ShatteredSunViewModel()
        {
        }

        public async Task Load(ShatteredSunModel ssModel)
        {
            Factions = new FactionsViewModel(ssModel.Factions);
            Units = new UnitsViewModel(ssModel.Units);
            await Task.CompletedTask;
        }
    }
}
