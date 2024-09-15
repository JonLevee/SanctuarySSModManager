using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SanctuarySSModManager.Controls
{
    /// <summary>
    /// Interaction logic for TooltipPanel.xaml
    /// </summary>
    public partial class TooltipPanel : StackPanel
    {
        //                 https://www.wpfsharp.com/2011/09/04/wpf-binding-to-a-property-of-a-static-class/

        private Dictionary<string, string> ToolTips { get; }

        private string? _tip;
        public string? Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                ToolTip = string.IsNullOrWhiteSpace(_tip) ? null : ToolTips[_tip];
            }
        }
        public TooltipPanel()
        {
            ToolTips = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "EnableMods", "This is the tooltip for enable mods" }
            };
        }
    }
}
