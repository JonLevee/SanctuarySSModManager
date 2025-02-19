using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSModManager;
using SanctuarySSModManager.MiscUtil;

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
            var parser = new LuaDataParser();
            var steamInfo = DIContainer.Get<ISteamInfo>();
            var folder = steamInfo.GetRoot("Sanctuary Shattered Sun Demo");
            var file = Path.Combine(folder, @"prototype\RuntimeContent\Lua\common\units\unitsTemplates\uca1001\uca1001.santp");
            var file2 = Path.Combine(folder, @"prototype\RuntimeContent\Lua\common\globalTables.lua");
            var data = parser.Parse(file2, "UnitTemplate");
        }
    }
}