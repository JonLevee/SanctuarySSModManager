using Microsoft.Extensions.DependencyInjection;

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