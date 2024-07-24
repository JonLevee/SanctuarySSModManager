using LuaParserUtil;
using LuaParserUtil.Loader;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSModManager.Extensions;
using System.Reflection;
using System.Xml.Linq;

namespace SanctuarySSModManager
{
    public class DIContainer
    {
        public static IServiceProvider Services { get; private set; }
        private static ServiceCollection services;

        static DIContainer()
        {
            services = new ServiceCollection();
            Services = services.BuildServiceProvider();
        }

        public static void Initialize(Action<ServiceCollection> configureServices)
        {

            ConfigureDefaultServices(services);
            configureServices(services);
            Services = services.BuildServiceProvider();
            var detailLog = Path.Combine(Assembly.GetExecutingAssembly().Location, "detailLog.txt");
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
            foreach (var type in Assembly.GetEntryAssembly().GetTypes().Where(t => !t.IsAbstract))
            {
                var attr = type.GetCustomAttributes().Where(a => a is ServiceAttribute).SingleOrDefault() as ServiceAttribute;
                if (attr != null)
                {
                    attr.Register(services, type);
                }
            }
        }
    }
}