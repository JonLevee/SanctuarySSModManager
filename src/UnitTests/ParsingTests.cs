using DiffMatchPatch;
using LuaParserUtil;
using Microsoft.Extensions.DependencyInjection;
using NLua;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSLib.ViewModel;
using SanctuarySSModManager;
using SanctuarySSModManager.Extensions;
using System.Reflection;
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
            var loader = new LuaObjectLoader
            {
                RootPath = @"C:\Program Files (x86)\Steam\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua"
            };
            var file = @"C:\Program Files (x86)\Steam\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua\common\systems\factions.lua";
            var factions = loader.Load<Factions>("factions.lua");
            //var manager = DIContainer.Services.GetService<modm>();
            //var m = DIContainer.Services.GetService<ShatteredSunModel>();
            //m.Load();
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

    [LuaObject(Table = "FactionsData")]
    public class Factions : List<Faction>
    {

    }
    public class Faction
    {
        public string Name { get; set; }
        public string TPLetter { get; set; }
        public string Tag { get; set; }
        public string InitialUnit { get; set; }
    }
    public class LuaObjectLoader
    {
        private readonly Dictionary<string,string> luaFiles = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);


        public string RootPath { get; internal set; }

        public T Load<T>(string luaFile) where T : class, new()
        {
            Should.
            var attr = typeof(T).GetCustomAttributeOrThrow<LuaObjectAttribute>();
            Assure.IsNotNullOrWhiteSpace(attr.Table);
            if (string.IsNullOrWhiteSpace(attr.Table))
            EnsureLuaFileLookupLoaded();
            if (!luaFiles.TryGetValue(luaFile, out string luaPath))
                throw new FileNotFoundException(luaFile);
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(luaPath);
                var table = (LuaTable)lua[tableName];
                var mo = table.ToModelObject();
                return mo;
            }

        }

        private void EnsureLuaFileLookupLoaded()
        {
            if (luaFiles.Any())
            {
                return;
            }
            foreach(var file in Directory.GetFiles(RootPath, "*.lua", SearchOption.AllDirectories))
            {
                luaFiles.Add(Path.GetFileName(file), file);
            }
            foreach (var file in Directory.GetFiles(RootPath, "*.santp", SearchOption.AllDirectories))
            {
                luaFiles.Add(Path.GetFileName(file), file);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LuaObjectAttribute : Attribute
    {
        public string Table { get; set; }
        public LuaObjectAttribute() 
        {
            Table = string.Empty;
        }
    }
}