using System.Diagnostics;
using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Extensions
{
    public static class JsonAccess
    {
        public static JsonNode Get(this JsonNode node, params string[] keys)
        {
            foreach (var key in keys)
            {
                node = node.AsObject()[key];
                Debug.Assert(node != null);
            }
            return node;
        }
    }
}