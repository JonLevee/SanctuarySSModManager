using Microsoft.Extensions.DependencyInjection;

namespace SanctuarySSModManager
{
    public class DIContainer
    {
        private static ServiceProvider serviceProvider;

        DIContainer()
        {
            serviceProvider = null;
        }

        public static void Initialize(Action<ServiceCollection> configureServices)
        {
            ServiceCollection services = new ServiceCollection();
            configureServices(services);
            serviceProvider = services.BuildServiceProvider();
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
                throw new ArgumentNullException(nameof(instance));
            }
            return instance;
        }
    }
}