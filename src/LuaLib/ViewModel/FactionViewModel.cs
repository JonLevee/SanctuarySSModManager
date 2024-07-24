using NLua;
using SanctuarySSLib.Attributes;
using System.Collections.Specialized;

namespace SanctuarySSLib.ViewModel
{
    [TransientService]
    public class FactionViewModel : OrderedDictionary
    {
        public FactionViewModel(LuaTable model)
        {
            foreach (var key in model.Keys)
            {
                Add(key, model[key]);
            }
        }
    }
}
