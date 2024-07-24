using DiffMatchPatch;
using LuaParserUtil;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSLib.ViewModel;
using SanctuarySSModManager;
using System.Text;

namespace UnitTests
{
    public class ParsingTests
    {

        [SetUp]
        public void Setup()
        {
            DIContainer.Initialize(ConfigureServices);

        }
        private void ConfigureServices(ServiceCollection services)
        {
        }

        [Test]
        public void ModManager()
        {
            //var manager = DIContainer.Services.GetService<modm>();
            var m = DIContainer.Services.GetService<ShatteredSunModel>();
            m.Load();
            var vm = DIContainer.Services.GetService<ShatteredSunViewModel>();


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

    }
}