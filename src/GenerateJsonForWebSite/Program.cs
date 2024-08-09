using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSModManager;
using SanctuarySSModManager.Extensions;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GenerateJsonForWebSite
{
    internal class Program
    {
        private static readonly Dictionary<Type, Func<JsonNode, object>> typeMap = new Dictionary<Type, Func<JsonNode, object>>
        {
            {typeof(long), n=>n.GetValue<long>() },
            {typeof(double), n=>n.GetValue<double>() },
            {typeof(string), GetString },
            {typeof(string[]), GetArray },
        };

        private static object GetString(JsonNode jsonNode)
        {
            if (jsonNode is JsonArray array)
            {
                var list = array.Select(item => item.GetValue<string>()).ToList();
                if (list.Any(item => item.StartsWith("unitsFinalized")))
                {
                    list.RemoveAll(item => item.StartsWith("unitsFinalized"));
                    Debug.Assert(list.Count == 1);
                    return list[0];
                }
            }
            return jsonNode.GetValue<string>();
        }
        private static object GetArray(JsonNode jsonNode)
        {
            var list = new List<string>();
            if (jsonNode is JsonObject dictionary)
            {
                foreach (var item in dictionary)
                {
                    if ((bool)item.Value)
                        list.Add(item.Key);
                }
            }
            else if (jsonNode is JsonArray array)
            {
                foreach (var item in array)
                {
                    list.Add(item.GetValue<string>());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            list.Sort();
            return list.ToArray();
        }

        static void Main(string[] args)
        {
            DIContainer.Initialize(InitializeContainer);
            var model = DIContainer.Get<ShatteredSunModel>();
            model.Load().Wait();
            var factions = model.Factions.ToDictionary(f => f["tpLetter"].GetValue<string>(), f => f["name"].GetValue<string>());
            var units = new Units();
            foreach (var kv in model.Units)
            {
                if (!model.AvailableUnits.TryGet(kv.Key, out bool enabled))
                    enabled = false;
                var unit = new Unit();
                units.Add(unit);
                unit.Add("general/enabled", model.AvailableUnits.ContainsKey(kv.Key) ? model.AvailableUnits[kv.Key].GetValue<bool>() : false);
                unit.Add("general/faction", factions[kv.Key.Substring(0, 2)]);

                unit.TryAdd(model, kv, "adjacency", out UnitItem _);
                unit.TryAdd(model, kv, "construction/buildPower", out UnitItem _);
                unit.TryAdd(model, kv, "construction/canBuild", out UnitItem _);
                unit.TryAdd(model, kv, "construction/range", out UnitItem _);
                unit.TryAdd(model, kv, "construction/upgradesTo", out UnitItem _);
                unit.TryAdd(model, kv, "defence/health/max", out UnitItem _);
                unit.TryAdd(model, kv, "defence/health/regen", out UnitItem _);
                unit.TryAdd(model, kv, "defence/shields", out UnitItem _);
                unit.TryAdd(model, kv, "economy/buildTime", out UnitItem _);
                unit.TryAdd(model, kv, "economy/cost/alloys", out UnitItem _);
                unit.TryAdd(model, kv, "economy/cost/energy", out UnitItem _);
                unit.TryAdd(model, kv, "economy/maintenanceConsumption/energy", out UnitItem _);
                unit.TryAdd(model, kv, "economy/production/alloys", out UnitItem _);
                unit.TryAdd(model, kv, "economy/production/energy", out UnitItem _);
                unit.TryAdd(model, kv, "economy/storage/alloys", out UnitItem _);
                unit.TryAdd(model, kv, "economy/storage/energy", out UnitItem _);
                if (unit.TryAdd(model, kv, "general/class", out UnitItem unitItem))
                {
                    unitItem.Value = ((List<object>)unitItem.Value)
                        .Cast<string>()
                        .Where(item => !item.StartsWith("unitsFinalized"))
                        .Single();
                }
                unit.TryAdd(model, kv, "general/displayName", out UnitItem _);
                unit.TryAdd(model, kv, "general/iconUI", out UnitItem _, isImage: true);
                unit.TryAdd(model, kv, "general/name", out UnitItem _);
                if (unit.TryAdd(model, kv, "general/orders", out unitItem))
                {
                    unitItem.Value = ((Dictionary<string, object>)unitItem.Value)
                        .Where(kv => (bool)kv.Value)
                        .Select(kv => kv.Key)
                        .Order()
                        .ToList();
                }
                unit.TryAdd(model, kv, "general/tpId", out UnitItem _);
                unit.TryAdd(model, kv, "intel/radarRadius", out UnitItem _);
                unit.TryAdd(model, kv, "intel/visionRadius", out UnitItem _);
                unit.TryAdd(model, kv, "movement/acceleration", out UnitItem _);
                unit.TryAdd(model, kv, "movement/air", out UnitItem _);
                unit.TryAdd(model, kv, "movement/minSpeed", out UnitItem _);
                unit.TryAdd(model, kv, "movement/rotationSpeed", out UnitItem _);
                unit.TryAdd(model, kv, "movement/speed", out UnitItem _);
                unit.TryAdd(model, kv, "movement/type", out UnitItem _);
                unit.TryAdd(model, kv, "tags", out UnitItem _);


                var allPaths = GetKeys(kv.Value, new Stack<string>()).ToList();
                var paths = allPaths.Except(unit.Keys).Order().ToList();
                var pathsToRemove = new string[]
                {
                    "collisionInfo",
                    "construction/rollOffPoints",
                    "construction/buildableOnResources",
                    "defence/health/value",
                    "footprint",
                    "general/icon",
                    "general/orders",
                    "general/iconUIBuildSortPriority",
                    "isFactory",
                    "movement/airHover",
                    "movement/animClips",
                    "movement/mass",
                    "movement/sortOrder",
                    "skirtSize",
                    "transport/storage",
                    "turrets",
                    "visuals",


                };
                paths.RemoveAll(path =>
                    pathsToRemove.Any(path.StartsWith)
                );
                if (paths.Any())
                {

                }
            }
        }

        private static IEnumerable<string> GetKeys(JsonNode node, Stack<string> stack)
        {
            if (node is JsonObject dictionary)
            {
                foreach (var kv in dictionary)
                {
                    stack.Push(kv.Key);
                    foreach (var key in GetKeys(kv.Value, stack))
                    {
                        yield return key;
                    }
                    stack.Pop();
                }
                yield break;
            }
            if (node is JsonValue value)
            {
                yield return string.Join("/", stack.Reverse());
                yield break;
            }
            if (node is JsonArray array)
            {
                yield return string.Join("/", stack.Reverse());
                yield break;
            }
            throw new NotImplementedException();
        }

        private static void InitializeContainer(ServiceCollection services)
        {

        }
    }

    public class Units : List<Unit>
    {
    }
    public class UnitItem
    {
        public string Path { get; set; }
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public bool IsImage { get; set; }
    }

    public class Unit : Dictionary<string, UnitItem>
    {
        public UnitItem Add(string path, object value)
        {
            var item = GetUnitItem(path);
            item.Value = value;
            base.Add(item.Path, item);
            return item;
        }

        public bool TryAdd(ShatteredSunModel model, KeyValuePair<string, JsonNode?> kv, string path, out UnitItem item, string? displayPath = null, bool isImage = false)
        {
            item = GetUnitItem(path, displayPath);
            item.IsImage = isImage;
            var unitData = model.Units[kv.Key];

            foreach (var key in path.Split('/'))
            {
                if (unitData == null)
                {
                    return false;
                }
                unitData = unitData[key];
            }
            if (unitData == null)
            {
                return false;
            }
            var converter = new CustomConvert();
            item.Value = converter.Convert(unitData);

            base.Add(item.Path, item);
            return true;
        }

        private UnitItem GetUnitItem(string path, string? displayPath = null)
        {
            var pathParts = (displayPath ?? path).Split('/');
            var item = new UnitItem
            {
                Path = path,
                Group = pathParts[0],
                SubGroup = pathParts.Length > 2 ? pathParts[1] : string.Empty,
                Name = pathParts.Length > 1 ? pathParts[pathParts.Length - 1] : string.Empty,
                IsImage = false,
            };
            return item;
        }
    }

    public class CustomConvert
    {
        internal object Convert(JsonNode node)
        {
            switch (node.GetValueKind())
            {
                case JsonValueKind.Undefined:
                    throw new NotImplementedException();
                    break;
                case JsonValueKind.Object:
                    return ((JsonObject)node).ToDictionary(p => p.Key, p => Convert(p.Value));
                    break;
                case JsonValueKind.Array:
                    return node.AsArray().Select(Convert).ToList();
                case JsonValueKind.String:
                    return node.ToString();
                case JsonValueKind.Number:
                    if (node.ToString().Contains('.'))
                        return node.GetValue<double>();
                    return node.GetValue<long>();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                    throw new NotImplementedException();
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
