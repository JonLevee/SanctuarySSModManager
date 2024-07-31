using SanctuarySSModManager.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
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
        public UnitDisplay()
        {
            InitializeComponent();
        }

        public void Load(JsonObject data)
        {
            DisplayName.Content = data.Get("general", "displayName").GetValue<string>();
            UnitName.Content = data.Get("general", "name").GetValue<string>();
            TpId.Content = data.Get("general", "tpId").GetValue<string>();

            Health.Content = data.Get("general", "defence", "health", "max").GetValue<string>();
            Mass.Content = data.Get("general", "economy", "cost", "alloys").GetValue<string>();
            Energy.Content = data.Get("general", "economy", "cost", "energy").GetValue<string>();
            BuildTime.Content = data.Get("general", "economy", "buildTime").GetValue<string>();

            var tags = new[]
            {
                "ALLOYS_EXTRACTION",
                "ALLOYS_PRODUCTION",
                "ALLOYS_STORAGE",
                "CONSTRUCTION",
                "ENERGY_PRODUCTION",
                "ENERGY_STORAGE",
                "ENGINEER",
                "ENGINEERING_STATION",
                "FACTORY",
                "HARVEST",
                "SHIELD",
                "STRUCTURE",
            };

        }
    }
}
