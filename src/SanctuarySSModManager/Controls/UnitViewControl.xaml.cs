using SanctuarySSLib.Models;
using SanctuarySSModManager.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UnitViewControl : UserControl
    {
        public UnitViewControl()
        {
            InitializeComponent();
        }

        public void Load(ShatteredSunModel model)
        {
            Grid.RowDefinitions.Clear();
            Grid.ColumnDefinitions.Clear();
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
            var units = model.Units;
            //var tags = units
            //    .SelectMany(kv => (JsonArray)kv.Value["tags"])
            //    .Select(v => v.GetValue<string>())
            //    .Distinct()
            //    .Order()
            //    .ToList();
            //File.WriteAllLines("tags.txt", tags);
            var timer = Stopwatch.StartNew();
            bool newColumn = true;
            foreach (var unit in units
                //.Take(2)
                )
            {
                Debug.Assert(unit.Value != null);
                var unitView = DIContainer.Get<UnitDisplay>(unit.Value.AsObject());
                unitView.Load();
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
    }
}
