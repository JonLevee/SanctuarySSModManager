using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.Models;
using System.Collections.Specialized;

namespace SanctuarySSLib.ViewModel
{
    public class FactionsViewModel : List<FactionModel>
    {
        public FactionsViewModel(FactionsModel model)
        {
            AddRange(model);
        }
    }
}
