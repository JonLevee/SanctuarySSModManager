using SanctuarySSLib.Models;
using SanctuarySSModManager.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UnitViewControl : UserControl
    {
        private readonly List<UnitDisplay> allUnits;
        public UnitViewControl()
        {
            InitializeComponent();
            allUnits = new List<UnitDisplay>();
        }

        public void Load(ShatteredSunModel model)
        {
            allUnits.Clear();
            //var tags = model
            //    .Units
            //    .SelectMany(kv => (JsonArray)kv.Value["tags"])
            //    .Select(v => v.GetValue<string>())
            //    .Distinct()
            //    .Order()
            //    .ToList();
            //File.WriteAllLines("tags.txt", tags);
            var timer = Stopwatch.StartNew();
            foreach (var unit in model.Units)
            {
                Debug.Assert(unit.Value != null);
                var unitView = DIContainer.Get<UnitDisplay>(unit.Value.AsObject());
                allUnits.Add(unitView);
            }
            timer.Stop();
        }

        public void UpdateUnits()
        {
            var timer = Stopwatch.StartNew();
            switch (Group1.SelectedValue.GetValueOrDefault("Faction"))
            {
                case "Faction":
                    var groupByFaction = allUnits.GroupBy(unit => unit.Faction.Text);
                    break;
                case "Tech":
                    var groupByTech = allUnits.GroupBy(unit => unit.Tier.Text);
                    break;

            }
            Grid.RowDefinitions.Clear();
            Grid.ColumnDefinitions.Clear();
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
            bool newColumn = true;
            foreach (var unitView in allUnits)
            {
                if (newColumn)
                {
                    Grid.RowDefinitions.Add(new RowDefinition { });
                    Grid.Set(unitView, 0);
                }
                else
                {
                    Grid.Set(unitView, 1);
                }
                newColumn = !newColumn;
            }
            timer.Stop();
        }

        private void Group1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUnits();
        }

        private void Group2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUnits();
        }
    }
}
