using SanctuarySSLib.Models;
using Sprache;
using System.Text.RegularExpressions;

namespace SanctuarySSLib.LuaUtil
{
    public interface ILuaTableDataLoader
    {
        void Load(LuaTableData tableData);
    }


    public class LuaTableRegexDataLoader : ILuaTableDataLoader
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline;
        private readonly Regex tableRegex = new Regex(@"^(?<table>[a-zA-Z0-9-]+)\s*=\s*{\s*(?<data>.+)^}", regexOptions);
        private readonly Regex itemSplitRegex = new Regex(@"}\s*,\s*{", regexOptions);
        private readonly Regex valueRegex = new Regex(@"^\s*(?<key>[a-zA-Z0-9-]+)\s*=\s*(?<quote>\""?)(?<value>[^,]*)\k<quote>\s*,\s*(?:--[^$]*?)?$", regexOptions);

        public void Load(LuaTableData tableData)
        {
            foreach (Match tableMatch in tableRegex.Matches(tableData.FileData.ToString()))
            {
                var tableName = tableMatch.Groups["table"].Value;
                var data = tableMatch.Groups["data"].Value.Trim();
                if (data.StartsWith("{"))
                {
                    var trimmedData = data.Trim('{', '}', ',', '\r', '\n');
                    var items = itemSplitRegex.Split(trimmedData);
                    foreach (var item in items)
                    {
                        var keyValues = valueRegex
                            .Matches(item)
                            .Select(match => new LuaTableKeyValue(tableData.FileData, match.Groups["key"].Value, match.Groups["value"]))
                            .ToDictionary(kv => kv.Key);

                        var name = keyValues.ContainsKey("name") ? keyValues["name"].ToString() : null;
                        if (name == null)
                            throw new KeyNotFoundException("could not find column 'name'");
                        tableData.TablesDepth2[tableName][name] = keyValues;
                    }
                }
                else
                {
                    var keyValues = valueRegex
                        .Matches(data)
                        .Select(match => new LuaTableKeyValue(tableData.FileData, match.Groups["key"].Value, match.Groups["value"]))
                        .ToDictionary(kv => kv.Key);
                    tableData.TablesDepth1[tableName] = keyValues;
                }
            }
        }
    }
}