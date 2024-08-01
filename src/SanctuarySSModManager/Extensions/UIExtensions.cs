using System.Windows.Controls;

namespace SanctuarySSModManager.Extensions
{
    public static class UIExtensions
    {
        public static void Set(this Grid grid, Control control, int iColumn, int? iRow = null, int? colSpan = null)
        {

            grid.Children.Add(control);
            Grid.SetRow(control, iRow ?? grid.RowDefinitions.Count - 1);
            Grid.SetColumn(control, iColumn);
            if (colSpan.HasValue)
            {
                Grid.SetColumnSpan(control, colSpan.Value);
            }
        }
    }

}
