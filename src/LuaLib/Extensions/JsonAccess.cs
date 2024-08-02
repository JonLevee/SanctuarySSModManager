using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SanctuarySSModManager.Extensions
{
    public static class JsonAccess
    {
        public static T Get<T>(this JsonNode node, string key) where T : class
        {
            if (TryGet(node, key, out T value))
                return value;
            return default(T);
        }

        public static object Get(this JsonNode node, string key)
        {
            return Get<object>(node, key);
        }

        public static bool TryGet<T>(this JsonNode node, string key, out T? value) where T : class
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
                case JsonValueKind.Object:
                    var dictionary = node.AsObject().ToDictionary(kv=>kv.Key, kv => kv.Value);
                    value = dictionary as T;
                    break;
                case JsonValueKind.String:
                    value = (T)Convert.ChangeType(node.GetValue<string>(), typeof(T));
                    break;
                case JsonValueKind.Number:
                    var element = node.GetValue<JsonElement>();
                    if (element.TryGetDouble(out double d))
                        value = (T)Convert.ChangeType(d, typeof(T));
                    else if (element.TryGetInt64(out long l))
                        value = (T)Convert.ChangeType(l, typeof(T));
                    else
                        throw new InvalidOperationException($"Don't know how to handle {node.GetValueKind()}");
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    value = (T)Convert.ChangeType(node.GetValue<bool>(), typeof(T));
                    break;
                case JsonValueKind.Null:
                    value = default;
                    break;
                default:
                    throw new InvalidOperationException($"Don't know how to handle {node.GetValueKind()}");
            }
            return true;
        }
    }
}