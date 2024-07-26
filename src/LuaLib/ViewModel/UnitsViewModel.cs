using SanctuarySSLib.Models;

namespace SanctuarySSLib.ViewModel
{
    public class UnitsViewModel : List<UnitModel>
    {
        public UnitsViewModel(UnitsModel model)
        {
            AddRange(model.Values);
        }
    }
}
