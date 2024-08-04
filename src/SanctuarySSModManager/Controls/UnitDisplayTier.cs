using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Controls
{
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
