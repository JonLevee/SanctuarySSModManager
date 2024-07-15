namespace LuaParserUtil
{

    public class LuaData
    {
        public Dictionary<string, LuaTableData> Data { get; }

        public LuaData()
        {
            Data = new Dictionary<string, LuaTableData>();
        }

        public IEnumerable<string> TableNames => Data.SelectMany(d => d.Value.TableNames).Distinct().Order(StringComparer.OrdinalIgnoreCase);
    }
}