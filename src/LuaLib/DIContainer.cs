using LuaParserUtil;
using LuaParserUtil.Loader;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.WorkInProgressNotUsed;
using System.Reflection;
using System.Xml.Linq;

namespace SanctuarySSModManager
{
    public class DIContainer
    {
        private static ServiceProvider serviceProvider;
        private static ServiceCollection services;

        static DIContainer()
        {
            services = new ServiceCollection();
            serviceProvider = services.BuildServiceProvider();
        }

        public static void Initialize(Action<ServiceCollection> configureServices)
        {

            ConfigureDefaultServices(services);
            configureServices(services);
            serviceProvider = services.BuildServiceProvider();
            var detailLog = Path.Combine(
                Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, 
                "detailLog.txt");
            if (File.Exists(detailLog))
            {
                File.Delete(detailLog);
            }
            LogManager.Setup().LoadConfiguration(builder =>
            {
                builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToConsole();
                builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: detailLog);
            });

        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services
                .AddSingleton(ModManagerMetaData.CreateInstance)
                .AddSingleton(typeof(ISteamInfo), typeof(SteamInfo))
                .AddSingleton(typeof(IGameMetadata), typeof(GameMetadata))
                .AddSingleton(typeof(ILuaTableDataLoader), typeof(LuaTableDataLoader));

        }

        public static ServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }
        public static T GetService<T>() where T : class
        {
            var instance = serviceProvider.GetService<T>();
            if (instance == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }
            return instance;
        }
    }
}