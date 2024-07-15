
namespace LuaParserUtil
{
    public interface ILuaTableDataLoader
    {
        void Load(LuaTableData tableData);
        IEnumerable<string> GetUnsupportedTableNames();
    }
}