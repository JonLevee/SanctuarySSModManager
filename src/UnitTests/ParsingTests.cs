using DiffMatchPatch;
using SanctuarySSModManager;
using System.Text;

namespace UnitTests
{
    public class ParsingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParseAll()
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
            var rootPath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype";
            var luaFilePaths = Directory.GetFiles(rootPath, "*.lua", SearchOption.AllDirectories);
            foreach (var luaFilePath in luaFilePaths)
            {
                var luaFile = new LuaFile(luaFilePath);
                var parser = new LuaDescentParser(luaFile.StringData);
                var result = parser.Parse().ToList();
            }
        }

        [Test]
        public void TestExpression()
        {
            // D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\units\availableUnits.lua
            // D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\systems\factions.lua
            var rootPath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua";
            var parser = new LuaRegexTableParser(rootPath);
            var factionsData = parser.Parse(@"common\systems\factions.lua")["FactionsData"];
            var availableUnits = parser.Parse(@"common\units\availableUnits.lua")["AvailableUnits"];
        }
    }
}