using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Extensions
{
    public static class JsonAccess
    {
        public static T Get<T>(this JsonNode node, string key)
        {
            if (TryGet(node, key, out T value))
                return value;
            return default(T);
        }

        public static object Get(this JsonNode node, string key)
        {
            return Get<object>(node, key);
        }

        public static bool TryGet<T>(this JsonNode node, string key, out T? value)
        {
            value = default;
            var keys = key.Split('/');
            foreach (var k in keys)
            {
                var jObject = node as JsonObject;
                if (jObject == null)
                {
                    return false;
                }
                node = jObject[k];
                if (node == null)
                {
                    return false;
                }
            }
            switch (node.GetValueKind())
            {
                case System.Text.Json.JsonValueKind.Object:
                    value = (T)Convert.ChangeType(node.GetValue<object>(), typeof(T));
                    break;
                case System.Text.Json.JsonValueKind.String:
                    value = (T)Convert.ChangeType(node.GetValue<string>(), typeof(T));
                    break;
                case System.Text.Json.JsonValueKind.Number:
                    value = (T)Convert.ChangeType(node.GetValue<long>(), typeof(T));
                    break;
                case System.Text.Json.JsonValueKind.True:
                case System.Text.Json.JsonValueKind.False:
                    value = (T)Convert.ChangeType(node.GetValue<bool>(), typeof(T));
                    break;
                case System.Text.Json.JsonValueKind.Null:
                    value = default;
                    break;
                default:
                    throw new InvalidOperationException($"Don't know how to handle {node.GetValueKind()}");
            }
            return true;
        }
    }
}