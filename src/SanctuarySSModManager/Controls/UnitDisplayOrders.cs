using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Controls
{
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
}
