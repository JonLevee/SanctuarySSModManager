using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.Models;
using System.Collections.Specialized;

namespace SanctuarySSLib.ViewModel
{
    [TransientService]
    public class FactionsViewModel : List<FactionViewModel>
    {
        public FactionsViewModel(FactionsModel model)
        {
            foreach (var kv in model)
            {
                Add(new FactionViewModel(kv.Value));
            }
        }
    }
}
