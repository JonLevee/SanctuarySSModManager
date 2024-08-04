using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Controls
{
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
}
