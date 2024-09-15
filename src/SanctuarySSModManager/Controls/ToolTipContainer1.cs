using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SanctuarySSModManager.Controls
{
    public class ToolTipContainer1 : StackPanel
    {
        private Dictionary<string,string> ToolTips { get; }

        private string? _tip;
        public string Tip 
        {
            get => _tip;
            set
            {
                _tip = value;
                ToolTip= ToolTips[_tip];
            } 
        }
        public ToolTipContainer1()
        {
            ToolTips = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "EnableMods", "This is the tooltip for enable mods" }
            };
        }
    }
}
