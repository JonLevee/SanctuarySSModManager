﻿using LuaParserUtil.LuaObjects;
using System.Text;

namespace LuaParserUtil
{
    public class LuaTableData
    {

        public LuaTableData()
        {
            FilePath = string.Empty;
            FileData = new StringBuilder();
            Tables = new LuaDictionary();
        }
        public StringBuilder FileData { get; }
        public string FilePath { get; set; }
        public LuaDictionary Tables { get; }
    }
}