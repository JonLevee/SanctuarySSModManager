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
    /// Interaction logic for UnitDisplay.xaml
    /// </summary>
    public partial class UnitDisplay : UserControl
    {
        public Dictionary<string, ContentControl> ContentControls { get; }
        public UnitDisplay()
        {
            InitializeComponent();
            ContentControls = new Dictionary<string, ContentControl>
            {

            }
            UnitImage.
        }
    }
}
