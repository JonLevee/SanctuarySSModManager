using LuaParserUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.WorkInProgressNotUsed;

namespace SanctuarySSLib.LuaUtil
{
    public class LuaDataLoader
    {
        private readonly IGameMetadata gameMetadata;
        private readonly ILuaTableDataLoader luaTableDataLoader;
        public int Count { get; private set; }

        public LuaDataLoader(
            IGameMetadata gameMetadata,
            ILuaTableDataLoader luaTableDataLoader)
        {
            this.gameMetadata = gameMetadata;
            this.luaTableDataLoader = luaTableDataLoader;
        }

        public void Load(LuaData data, string? luaFolderName = null)
        {
            var luaFolderNameToLoad = luaFolderName ?? gameMetadata.DefaultLuaFolder;
            var luaRootPath = gameMetadata.GetLuaPath(luaFolderNameToLoad);
            foreach (var luaFilePath in Directory.GetFiles(luaRootPath, "*.lua", SearchOption.AllDirectories))
            {
                var tableData = new LuaTableData();
                tableData.FilePath = luaFilePath.Substring(luaRootPath.Length + 1);
                tableData.FileData.Append(File.ReadAllText(luaFilePath));
                luaTableDataLoader.Load(tableData);

                foreach (var tableName in tableData.TableNames)
                {
                    data.Data.Add(tableName, tableData);
                    ++Count;
                }
            }

        }
    }

}