
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SanctuarySSModManager
{
    [DebuggerDisplay("Count:{Count}")]
    public class LuaTable
    {
        private Dictionary<string, object> table = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            table.Add(key, value);
        }

        public object this[string key]
        {
            get { return table[key]; }
            set { table[key] = value; }
        }
        public object this[string key, string subkey]
        {
            get => ((LuaTable)table[key])[subkey];
            set
            {
                if (!table.ContainsKey(key))
                    table.Add(key, new LuaTable());
                ((LuaTable)table[key])[subkey] = value;
            }
        }

        public bool ContainsKey(string key)
        {
            return table.ContainsKey(key);
        }

        public int Count => table.Count;
    }
    public class LuaRegexTableParser
    {
        private string rootPath;
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*{\s*(?<data>.+)^}", regexOptions);
        private readonly Regex itemSplitRegex = new Regex(@"}\s*,\s*{", regexOptions);
        private readonly Regex valueRegex = new Regex(@"^\s*(?<key>[a-zA-Z0-9-]+)\s*=\s*(?<quote>\""?)(?<value>[^,]*)\k<quote>\s*,\s*(?:--[^$]*?)?$", regexOptions);

        public LuaRegexTableParser(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public LuaTable Parse(string relativePath)
        {
            var table = new LuaTable();
            var path = Path.Combine(rootPath, relativePath);
            var sb = new StringBuilder(File.ReadAllText(path));
            foreach (Match tableMatch in tableRegex.Matches(sb.ToString()))
            {
                var tableName = tableMatch.Groups["table"].Value;
                table[tableName] = new LuaTable();
                var data = tableMatch.Groups["data"].Value.Trim();
                if (data.StartsWith("{"))
                {
                    var trimmedData = data.Trim('{', '}', ',', '\r', '\n');
                    var items = itemSplitRegex.Split(trimmedData);
                    foreach (var item in items)
                    {
                        LuaTable keyValues = GetKeyValues(item);
                        var name = keyValues.ContainsKey("name") ? (string)keyValues["name"] : null;
                        if (name == null)
                            throw new KeyNotFoundException("could not find column 'name'");
                        table[tableName, name] = keyValues;
                    }
                }
                else
                {
                    LuaTable keyValues = GetKeyValues(data);
                    table[tableName] = keyValues;
                }
            }
            return table;
        }

        private LuaTable GetKeyValues(string item)
        {
            var keyValues = new LuaTable();
            foreach (Match keyValue in valueRegex.Matches(item))
            {
                keyValues[keyValue.Groups["key"].Value] = keyValue.Groups["value"].Value;
            }

            return keyValues;
        }
    }
}