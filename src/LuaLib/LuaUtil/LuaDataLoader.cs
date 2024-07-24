using LuaParserUtil;
using SanctuarySSLib.MiscUtil;

namespace SanctuarySSLib.LuaUtil
{
    public class LuaDataLoader
    {
        private readonly IGameMetadata gameMetadata;
        private readonly LuaTableDataLoader luaTableDataLoader;
        public int Count { get; private set; }

        public LuaDataLoader(
            IGameMetadata gameMetadata,
            LuaTableDataLoader luaTableDataLoader)
        {
            this.gameMetadata = gameMetadata;
            this.luaTableDataLoader = luaTableDataLoader;
        }

        public void Load(LuaData data, string? luaFolderName = null)
        {
            throw new NotImplementedException();
            //var luaFolderNameToLoad = luaFolderName ?? gameMetadata.LuaFolder;
            //var luaRootPath = gameMetadata.GetLuaPath(luaFolderNameToLoad);
            //foreach (var luaFilePath in Directory.GetFiles(luaRootPath, "*.lua", SearchOption.AllDirectories))
            //{
            //    var tableData = new LuaTableData();
            //    tableData.FilePath = luaFilePath.Substring(luaRootPath.Length + 1);
            //    tableData.FileData.Append(File.ReadAllText(luaFilePath));
            //    luaTableDataLoader.Load(tableData);

            //    foreach (var tableName in tableData.TableNames)
            //    {
            //        //data.Data.Add(tableName, tableData);
            //        ++Count;
            //    }
            //}

        }
    }

}