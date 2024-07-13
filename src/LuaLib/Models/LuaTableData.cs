using Sprache;
using System.Text;

namespace SanctuarySSLib.Models
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
            TablesDepth1 = new Dictionary<string, Dictionary<string, LuaTableKeyValue>>();
            TablesDepth2 = new Dictionary<string, Dictionary<string, Dictionary<string, LuaTableKeyValue>>>();
            TableData = new LuaTableDataObject();
        }
        public StringBuilder FileData { get; }
        public string FilePath { get; set; }
        public Dictionary<string, Dictionary<string, LuaTableKeyValue>> TablesDepth1 { get; }
        public Dictionary<string, Dictionary<string, Dictionary<string, LuaTableKeyValue>>> TablesDepth2 { get; }
        public IEnumerable<string> TableNames => TablesDepth1.Keys.Concat(TablesDepth2.Keys).Concat(TableData.Keys);
        public LuaTableDataObject TableData { get; }

    }
}