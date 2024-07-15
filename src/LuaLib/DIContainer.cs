using LuaParserUtil;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.WorkInProgressNotUsed;

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
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services
                .AddSingleton(ModManagerMetaData.CreateInstance)
                .AddSingleton(typeof(ISteamInfo), typeof(SteamInfo))
                .AddTransient<LuaDataLoader>()
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