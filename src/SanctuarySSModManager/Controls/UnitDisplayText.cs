using SanctuarySSModManager.Extensions;
using System.Data.Common;
using System.Diagnostics;
using System.Text.Json.Nodes;
using System.Windows.Controls;

namespace SanctuarySSModManager.Controls
{
    [DebuggerDisplay("[{DisplayName}] {LuaPath}")]
    public class UnitDisplayText : TextBlock
    {
        public static readonly UnitDisplayText Empty = new();
        public string LuaPath { get; set; }
        public string DisplayName { get; set; }
        public virtual object Value => Text;

        public string GroupName { get; set; }
        public bool AddToGrid { get; }

        public UnitDisplayText()
        {
            LuaPath = string.Empty;
            GroupName = string.Empty;
            AddToGrid = false;
            DisplayName = string.Empty;
        }

        public UnitDisplayText(string groupName, string displayName, string luaPath)
        {
            GroupName = groupName;
            DisplayName = displayName;
            LuaPath = luaPath;
            AddToGrid = true;
        }
        public bool Initialize(Grid grid, JsonNode data, UnitDisplayText previousGroup)
        {
            var node = data;
            foreach (var key in LuaPath.Split('/'))
            {
                if (node == null)
                    break;
                node = node[key];
            }
            if (node == null)
                return false;
            SetValue(node);
            if (!AddToGrid)
                return true;
            var control = new TextBlock { Text = GroupName };
            grid.Children.Add(control);
            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(control, 0);

            control = new TextBlock { Text = DisplayName };
            grid.Children.Add(control);
            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(control, 1);
            Grid.SetColumnSpan(control, 2);

            control = this;
            grid.Children.Add(control);
            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(control, 3);

            return true;
        }

        protected virtual void SetValue(JsonNode node)
        {
            Text = node.GetValue<string>();
        }
    }

    public class UnitDisplayNumber : UnitDisplayText
    {
        public double Double { get; private set; }
        public override object Value => Double;
        public UnitDisplayNumber() : base()
        {
        }
        public UnitDisplayNumber(string groupName, string displayName, string luaPath) : base(groupName, displayName, luaPath)
        {

        }
        protected override void SetValue(JsonNode node)
        {
            Double = node.GetValue<double>();
            Text = Double.ToString();
        }
    }
    public class UnitDisplayBoolean : UnitDisplayText
    {
        public bool Bool { get; private set; }
        public override object Value => Bool;
        public UnitDisplayBoolean() : base()
        {
        }
        public UnitDisplayBoolean(string groupName, string displayName, string luaPath) : base(groupName, displayName, luaPath)
        {

        }
        protected override void SetValue(JsonNode node)
        {
            Bool = node.GetValue<bool>();
            Text = Bool.ToString();
        }
    }
    public class UnitDisplayOrders : UnitDisplayText
    {
        public UnitDisplayOrders() : base()
        {
        }
        public UnitDisplayOrders(string groupName, string displayName, string luaPath) : base(groupName, displayName, luaPath)
        {

        }
        protected override void SetValue(JsonNode node)
        {
            var orders = string.Join(", ", node.AsObject().Where(kv => kv.Value != null && kv.Value.GetValue<bool>()).Select(kv => kv.Key).Order());
            Text = orders;
        }
    }
    public class UnitDisplayTier : UnitDisplayText
    {
        public UnitDisplayTier() : base()
        {
        }
        public UnitDisplayTier(string groupName, string displayName, string luaPath) : base(groupName, displayName, luaPath)
        {

        }
        protected override void SetValue(JsonNode node)
        {
            var tags = node.AsArray().Select(item => item.GetValue<string>()).ToList();
            var tiers = new string[] { "TECH1", "TECH2", "TECH3", "TECH4" };
            var techTier = tiers.Intersect(tags).Single();
            Text = techTier;
        }
    }
}
