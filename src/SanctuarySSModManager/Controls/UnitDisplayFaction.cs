using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Controls
{
    public class UnitDisplayFaction : UnitDisplayText
    {
        public UnitDisplayFaction() : base()
        {
        }
        public UnitDisplayFaction(string groupName, string displayName, string luaPath) : base(groupName, displayName, luaPath)
        {

        }
        protected override void SetValue(JsonNode node)
        {
            var text = node.GetValue<string>();
            switch(text[1])
            {
                case 'e':
                    Text = "EDA";
                    break;
                case 'g':
                    Text = "Guard";
                    break;
                case 'c':
                    Text = "Chosen";
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
