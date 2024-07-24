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
            foreach (var key in model.Keys)
            {
                var unit = new UnitViewModel(model[key]);
                Add(key, unit);
            }
        }
    }

    [TransientService]
    public class UnitViewModel
    {
        private readonly LuaTable table;

        public UnitDefenseViewModel Defense { get; }

        public UnitDefenseViewModel Defence { get; }

        public UnitViewModel(LuaTable table)
        {
            this.table = table;
            Defence = new UnitDefenseViewModel((LuaTable)table["defence"]);
        }
    }

    [TransientService]
    public class UnitDefenseViewModel
    {
        private readonly ModelObject table;
        //public long Health => (long)table["health"]["max"];

        public UnitDefenseViewModel(LuaTable table)
        {
            this.table = table.ToModelObject();
        }
    }
}
