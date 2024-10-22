
using System.Text.RegularExpressions;

namespace SanctuarySSLib.LuaUtil
{
    public class LuaDataParser
    {
        private static readonly RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline;
        private static readonly Regex nameRegex = new Regex(@"^\s*(?<name>[\w_\.]+)\s*=(?<data>\s*{.+?^})", regexOptions);
        public LuaDataParserObject Parse(string file, string name)
        {
            var result = new LuaDataParserObject(LuaDataParserObjectType.Array);
            var text = File.ReadAllText(file);
            var match = nameRegex.Match(text);
            while (match.Success)
            {
                var matchName = match.Groups["name"].Value;
                var data = match.Groups["data"].Value;
                match = match.NextMatch();
            }
            return result;
        }
    }
    public enum LuaDataParserObjectType
    {
        Null,
        String,
        Int,
        Double,
        Array,
        Dictionary
    }
    public class LuaDataParserObject
    {
        private readonly Dictionary<string, object> data;
        public LuaDataParserObjectType ObjectType { get; }

        public LuaDataParserObject(LuaDataParserObjectType objectType)
        {
            ObjectType = objectType;
        }
    }
}