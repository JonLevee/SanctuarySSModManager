using SanctuarySSLib.Models;
using SanctuarySSModManager.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public void UpdateUnits(IGrouping<object?, UnitDisplay> units, int level, int colOffset, params string[] groupNames)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var member = typeof(UnitDisplay)
                .GetMember(
                groupNames[level],
                MemberTypes.Field | MemberTypes.Property,
                flags
                )
                .Single();

            Func<UnitDisplay, object> getValue = member.MemberType == MemberTypes.Field
                ? typeof(UnitDisplay).GetField(groupNames[level],flags).GetValue
                : typeof(UnitDisplay).GetProperty(groupNames[level],flags).GetValue;
            var grouping = units.GroupBy(getValue);
            var count = grouping.Count();
            int iCol = 0;
            foreach (var group in grouping)
            {
                UpdateUnits(group, level + 1, colOffset, groupNames);
                ++iCol;
            }
        }
        public void UpdateUnits()
        {
            var timer = Stopwatch.StartNew();
            Grid.RowDefinitions.Clear();
            Grid.ColumnDefinitions.Clear();
            UpdateUnits(allUnits.GroupBy(_=>(object)1).First(), 0, 0, "Faction", "Tech");
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
