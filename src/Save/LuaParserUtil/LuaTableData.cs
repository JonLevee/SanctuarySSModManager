using System.Text;
using LuaParserUtil.ToDelete;

namespace LuaParserUtil
{
    public class LuaTableDataObject : Dictionary<string, LuaTableDataObject>
    {
        public LuaTableDataObject()
        {
            Value = string.Empty;
        }
        public string Value { get; set; }
        public bool HasChildren { get; set; }

    }
    public class LuaTableData
    {

        public LuaTableData()
        {
            FilePath = string.Empty;
            FileData = new StringBuilder();
            TableData = new List<LuaExpressionList>();
        }
        public StringBuilder FileData { get; }
        public string FilePath { get; set; }
        public IEnumerable<string> TableNames => TableData.Select(x => x.Name.ToString());
        public List<LuaExpressionList> TableData { get; }

    }
}