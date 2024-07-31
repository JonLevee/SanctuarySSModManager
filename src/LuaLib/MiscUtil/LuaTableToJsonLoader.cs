using NLua;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using System.Text;
using System;
using System.Diagnostics;
using SanctuarySSLib.LuaUtil;
using System.Collections;

namespace SanctuarySSLib.MiscUtil
{
    public class LuaTableToJsonLoader
    {
        private readonly IGameMetadata gameMetadata;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new LuaTableConverter()
            }
        };

        public JsonObject Root { get; set; }
        public LuaTableToJsonLoader(IGameMetadata gameMetadata)
        {
            Root = new JsonObject();
            this.gameMetadata = gameMetadata;
        }
        public void Load(string relativePath, string tableName, Func<JsonNode, string>? getSubKey = null)
        {
            var filePath = gameMetadata.GetFullPath(relativePath);
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(filePath);
                var table = (LuaTable)lua[tableName];
                var jsonText = JsonSerializer.Serialize(table, options);
                var node = JsonSerializer.Deserialize<JsonNode>(jsonText, options);
                if (node == null)
                    throw new NullReferenceException($"file:{filePath} table:{tableName} deserialized to null");
                if (getSubKey == null)
                {
                    Root.Add(tableName, node);
                    return;
                }
                if (Root[tableName] == null)
                {
                    Root[tableName] = new JsonObject();
                }
                var subKey = getSubKey(node);
                Root[tableName]?.AsObject().Add(subKey, node);
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(Root, options);
        }
        private class LuaTableConverter : JsonConverter<LuaTable>
        {
            public override LuaTable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, LuaTable value, JsonSerializerOptions options)
            {
                var text = string.Empty;
                var isList = Enumerable
                    .Range(1, value.Keys.Count)
                    .Zip(value.Keys.Cast<object>(), (i, k) => k is long && Equals((long)k, (long)i))
                    .All(r => r);
                if (isList)
                {
                    var list = value.Values.Cast<object>().ToArray();
                    text = JsonSerializer.Serialize(list, options);
                }
                else
                {
                    var dictionary = value.Keys.Cast<object>().ToDictionary(k => k, k => value[k]);
                    text = JsonSerializer.Serialize(dictionary, options);
                }
                writer.WriteRawValue(text);
            }
        }

    }
}
