using DiffMatchPatch;
using LuaParserUtil;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaTableParsing;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager;
using System.Text;

namespace UnitTests
{
    public class ParsingTests
    {
        private ILuaTableDataLoader luaTableDataLoader;

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
        public void ParseAllExpression()
        {
            var data = new LuaData();
            var loader = DIContainer.GetService<LuaDataLoader>();
            loader.Load(data);
            Console.WriteLine("table names");
            foreach (var item in data.TableNames)
            {
                Console.WriteLine(item);
            }
        }

        [Test]
        public void TestExpression()
        {
            var loader = DIContainer.GetService<ILuaTableDataLoader>();
            var luaRootPath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua";
            // D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\units\availableUnits.lua
            // D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\systems\factions.lua
            // D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\engineFunctions.lua
            // D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\client\breadUIActions.lua
            // D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\systems\factions.lua
            var luaFilePath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\systems\factions.lua";
            var tableData = new LuaTableData();
            tableData.FilePath = luaFilePath.Substring(luaRootPath.Length + 1);
            tableData.FileData.Append(File.ReadAllText(luaFilePath));

            loader.Load(tableData);
            //var rootPath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua";
            //var parser = new LuaRegexTableParser(rootPath);
            //var factionsData = parser.Parse(@"common\systems\factions.lua").GetTable("FactionsData");
            //var availableUnits = parser.Parse(@"common\units\availableUnits.lua").GetTable("AvailableUnits");
            //var enabledUnits = availableUnits.Where(kv=>(bool)kv.Value == true).ToList();
        }
    }
}