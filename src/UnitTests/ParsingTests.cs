using DiffMatchPatch;
using LuaParserUtil;
using LuaParserUtil.Loader;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSModManager;
using System.Text;

namespace UnitTests
{
    public class ParsingTests
    {
        private ILuaTableDataLoader luaTableDataLoader;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [SetUp]
        public void Setup()
        {
            DIContainer.Initialize(ConfigureServices);
            ILuaTableDataLoader luaTableDataLoader = DIContainer.GetService<ILuaTableDataLoader>();

        }
        private void ConfigureServices(ServiceCollection services)
        {
            services
                .AddSingleton(typeof(ILuaTableDataLoader), typeof(LuaTableDataLoader));


        }

        [Test]
        public void DescentParserParseAll()
        {
            //        var modManagerMetaData = new ModManagerMetaData
            //        {
            //            ShatteredSunDirectoryRoot = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype",
            //            ModRootFolder = "prototype",
            //            ModManagerFolder = string.Empty
            //        };
            //        var luaDataLoader = new LuaDataLoader(modManagerMetaData);


            //        var rootPath = luaRelativePath == null
            //? Path.Combine(modManagerMetaData.FullModRootFolder, luaRelativePath)
            //: modManagerMetaData.FullModRootFolder;
            //var rootPath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype";
            //var luaFilePaths = Directory.GetFiles(rootPath, "*.lua", SearchOption.AllDirectories);
            //foreach (var luaFilePath in luaFilePaths)
            //{
            //    var luaFile = new LuaFile(luaFilePath);
            //    var parser = new LuaDescentParser(luaFile.StringData);
            //    var result = parser.Parse().ToList();
            //}
        }

        [Test]
        public void TestExpression()
        {
            var factions = new FactionCollectionModel();
            factions.Load();
            Dump(factions);
            //var loader = DIContainer.GetService<ILuaTableDataLoader>();
            //var steamInfo = new SteamInfo();
            //var appRootPath = steamInfo.GetRoot();
            //var luaRootPath = Path.Combine(appRootPath,  @"prototype\RuntimeContent\Lua");
            //var files = new List<string>
            //{
            //    @"common\units\availableUnits.lua",
            //    @"common\systems\factions.lua",
            //};
            //foreach (var luaFilePath in files)
            //{
            //    var tableData = new LuaTableData();
            //    tableData.FilePath = luaFilePath;
            //    tableData.FileData.Append(File.ReadAllText(Path.Combine(luaRootPath, luaFilePath)));

            //    loader.Load(tableData);
            //    logger.Debug("Finished loading {tableData}", tableData);
            //}
        }

        private void Dump(FactionCollectionModel model)
        {
            foreach (var faction in model)
            {
                logger.Debug($"faction '{faction.Key}'");
                foreach(var field in faction.Value)
                {
                    logger.Debug($"  '{field.Key}' = {field.Value.Text}");
                }
            }
        }
    }

}