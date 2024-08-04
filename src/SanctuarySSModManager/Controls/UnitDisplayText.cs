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
            SetGroup(grid, previousGroup);
            SetDisplayName(grid);
            SetValue(grid, node);

            return true;
        }

        protected virtual void SetGroup(Grid grid, UnitDisplayText previousGroup)
        {
            if (!AddToGrid || previousGroup.GroupName == GroupName)
                return;
            var control = new TextBlock { Text = GroupName };
            grid.Children.Add(control);
            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(control, 0);
        }

        protected virtual void SetDisplayName(Grid grid)
        {
            if (!AddToGrid)
                return;
            var control = new TextBlock { Text = DisplayName };
            grid.Children.Add(control);
            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(control, 1);
            Grid.SetColumnSpan(control, 2);
        }

        protected virtual void SetValue(Grid grid, JsonNode node)
        {
            SetValue(node);
            if (!AddToGrid)
                return;
            var control = this;
            grid.Children.Add(control);
            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(control, 3);
        }

        protected virtual void SetValue(JsonNode node)
        {
            Text = node.GetValue<string>();
        }
    }
}
