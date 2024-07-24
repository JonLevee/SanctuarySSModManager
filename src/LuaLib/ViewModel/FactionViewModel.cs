using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using System.Collections.Specialized;

namespace SanctuarySSLib.ViewModel
{
    [TransientService]
    public class FactionViewModel : Dictionary<string,string>
    {
        public FactionViewModel(ModelObject model)
        {
            foreach (var kv in model)
            {
                Add((string)kv.Key, kv.Value.Text);
            }
        }
    }
}
