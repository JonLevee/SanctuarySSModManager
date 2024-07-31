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
            Factions = new FactionsViewModel();
            Units = new UnitsViewModel();
        }

        public async Task Load(ShatteredSunModel ssModel)
        {
            await Task.CompletedTask;
        }
    }
}
