using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.Models;
using System.Collections.Specialized;

namespace SanctuarySSLib.ViewModel
{
    [TransientService]
    public class FactionsViewModel : OrderedDictionary
    {
        public FactionsViewModel(FactionsModel model)
        {
            foreach (var key in model.Keys)
            {
                var item = model[key];
                var faction = new FactionViewModel((LuaTable)item);
                Add(key, faction);
            }
        }
    }
}
