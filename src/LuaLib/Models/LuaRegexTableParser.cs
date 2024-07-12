
using NLua;
using Sprache;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SanctuarySSLib.Models
{

    public class LuaRegexTableParser
    {
        private string rootPath;

        public LuaRegexTableParser(string rootPath)
        {
            this.rootPath = rootPath;
        }


        private Dictionary<string, LuaTableData> luaTableData = new Dictionary<string, LuaTableData>();
        public void LoadTables(params string[] tableNames)
        {
            var luaFilePaths = Directory
                .GetFiles(rootPath, "*.lua", SearchOption.AllDirectories)
                .Where(p => !tableNames.Any() || tableNames.Contains(Path.GetFileName(p)));
            foreach (var luaFilePath in luaFilePaths)
            {
                var tableData = new LuaTableData(luaFilePath);
                tableData.Load();

                foreach (var tableName in tableData.TableNames)
                {
                    luaTableData.Add(tableName, tableData);
                }
            }
        }
    }
}