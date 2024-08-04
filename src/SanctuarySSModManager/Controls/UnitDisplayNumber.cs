using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Controls
{
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
}
