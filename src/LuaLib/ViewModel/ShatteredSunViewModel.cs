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
        public ShatteredSunViewModel(IServiceProvider serviceProvider, ShatteredSunModel ssModel)
        {
            Factions = serviceProvider.GetService<FactionsViewModel>();
            Units = serviceProvider.GetService<UnitsViewModel>();
        }
    }

    [TransientService]
    public class UnitsViewModel : Dictionary<string, UnitViewModel>
    {
        public UnitsViewModel(UnitsModel model)
        {
            foreach (var kv in model)
            {
                var unit = new UnitViewModel(kv.Value);
                Add(kv.Key, unit);
            }
        }
    }

    [TransientService]
    public class UnitViewModel
    {
        private readonly ModelObject table;

        public UnitDefenseViewModel Defense { get; }

        public UnitDefenseViewModel Defence { get; }

        public UnitViewModel(ModelObject model)
        {
            this.table = model;
            Defence = new UnitDefenseViewModel(model["defence"]);
        }
    }

    [TransientService]
    public class UnitDefenseViewModel
    {
        private readonly ModelObject model;
        public long Health => model["health"]["max"].Long;

        public UnitDefenseViewModel(ModelObject model)
        {
            this.model = model;
        }
    }
}
