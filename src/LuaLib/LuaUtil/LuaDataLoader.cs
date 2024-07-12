using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;

namespace SanctuarySSLib.LuaUtil
{
    public interface ILuaTableDataLoader
    {
        void Load(LuaTableData tableData);
        IEnumerable<string> GetLuaDirs();
    }

    public class LuaDataLoader
    {
        private readonly IGameMetadata gameMetadata;

        public LuaDataLoader(IGameMetadata gameMetadata)
        {
            this.gameMetadata = gameMetadata;
        }

        public void Load(LuaData data, string? luaFolderName = null)
        {
            var luaFolderNameToLoad = luaFolderName ?? gameMetadata.DefaultLuaFolder;
            var luaRootPath = gameMetadata.GetLuaPath(luaFolderNameToLoad);
            foreach (var luaFilePath in Directory.GetFiles(luaRootPath, "*.lua", SearchOption.AllDirectories))
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