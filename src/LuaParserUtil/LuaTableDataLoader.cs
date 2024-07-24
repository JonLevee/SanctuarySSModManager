using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaParserUtil
{
    public class LuaTableDataLoader
    {
        public void Load(LuaTableData tableData)
        {
            var luaFile = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\units\unitsTemplates\uea3011\uea3011.santp";
            var luaString = File.ReadAllText(luaFile);
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoString(luaString);
                var table = (LuaTable)lua["UnitTemplate"];
                var turrets = table["turrets"];

                string res = (string)lua["UnitTemplate"];

            }
            throw new NotImplementedException();
        }
    }

    public class LuaData
    {

    }

    public class LuaTableData
    {
        public string FilePath { get; set; }
        public StringBuilder FileData { get; set; }
        public List<LuaTableData> TableNames { get; }
    }
}
