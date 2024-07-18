using LuaParserUtil;
using SanctuarySSLib.MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctuarySSLib.Models
{
    public class FactionCollectionModel : Dictionary<string, FactionModel>
    {
        private readonly SteamInfo steamInfo;
        private readonly CollectionModelMetadata collectionModelMetadata;
        private readonly ILuaTableDataLoader tableDataLoader;

        public string RelativePath => @"common\systems\factions.lua";
        public string FullPath { get; }
        public string TableName => "FactionsData";

        public FactionCollectionModel(
            SteamInfo steamInfo,
            CollectionModelMetadata collectionModelMetadata,
            ILuaTableDataLoader tableDataLoader
            ) 
        {
            this.steamInfo = steamInfo;
            this.collectionModelMetadata = collectionModelMetadata;
            this.tableDataLoader = tableDataLoader;
            FullPath = Path.Combine(steamInfo.GetRoot(), collectionModelMetadata.RelativeLuaFolder, RelativePath);
        }
        public void Load()
        {
            var tableData = new LuaTableData();
            tableData.FilePath = RelativePath;
            tableData.FileData.Append(File.ReadAllText(FullPath));

            tableDataLoader.Load(tableData);
            throw new NotImplementedException();
        }
    }

    public class FactionModel : Dictionary<string, string>
    {

    }
}
