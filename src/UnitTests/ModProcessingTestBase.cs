using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.MiscUtil;
using SanctuarySSModManager;

namespace UnitTests
{
    public class ModProcessingTestBase
    {
        private readonly record struct InstanceData(IObjectPersister persister, object instance, string data);
        private static readonly Dictionary<Type, InstanceData> instanceData = new Dictionary<Type, InstanceData>();
        [SetUp]
        public void Setup()
        {
            DIContainer.Initialize(ConfigureServices);
            var steamInfo = DIContainer.Get<SteamInfo>();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            
            services
                .AddSingleton<SSSUserSettings>()
                .AddSingleton<SSSModManagerSettings>()
                .AddSingleton<UserInteractionTestTool>()
                .AddSingleton<IUserInteraction>(s=>s.GetService<UserInteractionTestTool>())
                .AddSingleton<ISteamInfo>(s=>s.GetService<SteamInfoTest>())
                ;
        }

    }
}