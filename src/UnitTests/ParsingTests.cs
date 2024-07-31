using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Models;
using SanctuarySSModManager;

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
            var m = DIContainer.Get<ShatteredSunModel>();
            m.Load();
            var Units = m.Units.GroupBy(kv => m.IsUnitEnabled(kv.Key)).ToDictionary(g => g.Key, g => g.ToList());
            //var vm = DIContainer.Services.GetService<ShatteredSunViewModel>();


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