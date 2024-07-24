using LuaParserUtil;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSModManager.Extensions;
using System.Reflection;

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
                builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: Path.GetFileName(detailLog));
            });

        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            var typeAttrs = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && null != t.GetCustomAttributes().SingleOrDefault(a => a is ServiceAttribute))
                .Select(type => new { type, attr = type.GetCustomAttributes().SingleOrDefault(a => a is ServiceAttribute) as ServiceAttribute })
                .ToList();
            var defaultServiceAttributeName = typeof(DefaultServiceAttribute<>).Name;
            foreach (var typeAttr in typeAttrs)
            {
                if (null == typeAttr || null == typeAttr.type || null == typeAttr.attr)
                    continue;

                typeAttr.attr.Register(services, typeAttr.type);
            }
        }
    }
}