using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SanctuarySSLib.MiscUtil
{
    [SingletonService]
    public class LuaObjectLoadObjectHandler
    {
        private delegate bool TryLoadObjectHandler(object instance, Type instanceType, LuaTable table);
        private readonly List<TryLoadObjectHandler> tryLoadObjectHandlers;

        public LuaObjectLoadObjectHandler()
        {
            tryLoadObjectHandlers = new List<TryLoadObjectHandler>
            {
                TryLoadDictionaryObject,
                TryLoadListObject,
                TryLoadClassObject
            };
        }

        public void LoadObject(object targetObject, LuaTable table)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new LuaTableConverter()
                }
            };
            var json = JsonSerializer.Serialize(table, serializeOptions);
            blah
            var targetType = targetObject.GetType();
            var success = tryLoadObjectHandlers.Any(h => h(targetObject, targetType, table));
            if (!success)
            {
                throw new InvalidOperationException($"No TryLoadObjectHandler could handle targetType {targetType}");
            }
        }

        private class LuaTableConverter : JsonConverter<LuaTable>
        {
            public override LuaTable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, LuaTable value, JsonSerializerOptions options)
            {
                var list = new List<object>();
                var dictionary = new Dictionary<string, object>();
                long index = 1;

                foreach (var key in value.Keys)
                {
                    if (key is long && Equals(key,index))
                    {
                        list.Add(value[key]);
                        ++index;
                    }
                    else if (key is string)
                    {
                        dictionary.Add((string)key, value[key]);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                object objectToSerialize = list.Count != 0 && dictionary.Count != 0
                    ? throw new InvalidOperationException()
                    : list.Count != 0 ? list : dictionary;
                var text = JsonSerializer.Serialize(objectToSerialize, options);
                writer.WriteRawValue(text);
            }
        }

        public object GetObject(object luaObject, Type targetType)
        {
            if (targetType == typeof(string) || targetType.IsValueType)
            {
                var value = Convert.ChangeType(luaObject, targetType);
                return value;
            }
            var table = luaObject as LuaTable;
            Debug.Assert(table != null);
            var targetObject = Activator.CreateInstance(targetType);
            Debug.Assert(targetObject != null);
            LoadObject(targetObject, table);
            return targetObject;
        }

        private bool TryLoadListObject(object instance, Type instanceType, LuaTable table)
        {
            if (!instanceType.GetInterfaces().Any(i => i.Name == "IList"))
            {
                return false;
            }
            var itemType = instanceType?.BaseType?.GenericTypeArguments[0];
            Debug.Assert(itemType != null);
            var list = (IList)instance;
            Debug.Assert(list != null);
            foreach (var value in table.Values)
            {
                list.Add(GetObject(value, itemType));
            }
            return true;
        }

        private bool TryLoadDictionaryObject(object instance, Type instanceType, LuaTable table)
        {
            if (!instanceType.GetInterfaces().Any(i => i.Name == "IDictionary"))
            {
                return false;
            }
            var dictionary = instance as IDictionary;
            Debug.Assert(dictionary != null);
            var keyType = instanceType.BaseType?.GenericTypeArguments[0];
            Debug.Assert(keyType != null);
            var valueType = instanceType.BaseType?.GenericTypeArguments[1];
            Debug.Assert(valueType != null);
            foreach (KeyValuePair<object, object> kv in table)
            {
                var key = GetObject(kv.Key, keyType);
                var value = GetObject(kv.Value, valueType);
                dictionary.Add(key, value);
            }
            return true;
        }

        private bool TryLoadClassObject(object instance, Type instanceType, LuaTable table)
        {
            if (!instanceType.IsClass)
            {
                return false;
            }
            var common = instanceType
                .GetProperties()
                .Join(
                    table.Keys.Cast<string>(),
                    p => p.Name,
                    k => k,
                    (property, key) => new { property, key },
                    StringComparer.OrdinalIgnoreCase);
            foreach (var item in common)
            {
                var value = GetObject(table[item.key], item.property.PropertyType);
                item.property.SetValue(instance, value);
            }
            return true;
        }
    }
}
