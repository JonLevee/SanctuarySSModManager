using SanctuarySSLib.Models;

namespace SanctuarySSLib.LuaUtil
{
    public class LuaDataLoader : ILuaDataLoader
    {
        private readonly IModManagerMetaDataLoader metaDataLoader;

        public LuaDataLoader(IModManagerMetaDataLoader metaDataLoader) 
        {
            this.metaDataLoader = metaDataLoader;
        }
        public void Load(LuaData data)
        {
            var rootPath = metaDataLoader
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