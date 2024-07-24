using LuaParserUtil;
using LuaParserUtil.LuaObjects;
using LuaParserUtil.Tokens;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctuarySSLib.Models
{
    public class FactionCollectionModel : Dictionary<string, FactionModel>
    {
        public string RelativePath => @"common\systems\factions.lua";
        public string FullPath { get; }
        public string TableName => "FactionsData";

        public FactionCollectionModel() 
        {
            var gameMetadata = DIContainer.GetService<IGameMetadata>();
            FullPath = Path.Combine(gameMetadata.FullLuaFolderPath, RelativePath);
        }
        public void Load()
        {
            var tableData = new LuaTableData();
            tableData.FilePath = RelativePath;
            tableData.FileData.Append(File.ReadAllText(FullPath));

            var tableDataLoader = DIContainer.GetService<ILuaTableDataLoader>();
            tableDataLoader.Load(tableData);
            var factionData = (LuaDictionary)tableData.Tables[TableName];
            foreach (LuaDictionary table in factionData.ArrayItems)
            {
                var faction = new FactionModel(table);
                Add(faction.Key, faction);
            }
        }
    }

    public class FactionModel : Dictionary<string, Token>
    {
        private readonly LuaDictionary dictionary;
        public string KeyField => "name";
        public string Key => this[KeyField].Text;

        public FactionModel(LuaDictionary dictionary)
        {
            this.dictionary = dictionary;
            foreach(var key in dictionary.Keys)
            {
                var value = (LuaValue)dictionary[key];
                Add((string)key, value.Token);
            }
        }
    }
}
