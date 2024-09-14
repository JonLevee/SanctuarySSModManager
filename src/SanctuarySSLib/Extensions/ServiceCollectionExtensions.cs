using Microsoft.Extensions.DependencyInjection;

namespace SanctuarySSModManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonByInterface<T>(this IServiceCollection services)
            where T : class
        {
            var interfaceType = typeof(T).GetInterfaces().Single();
            services.AddSingleton(interfaceType, typeof(T));
            return services;
        }
    }
}
