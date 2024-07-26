using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace SanctuarySSLib.Attributes
{

    public abstract class ServiceAttribute : Attribute
    {
        public static SingletonServiceAttribute Singleton = new SingletonServiceAttribute();
        public abstract ServiceLifetime Lifetime { get; }
        public virtual Type ServiceType(Type implementationType) => implementationType;
        public void Register(IServiceCollection services, Type implementationType)
        {
            // interface types will be handled by default impl
            if (implementationType.IsAbstract)
            {
                return;
            }

            var lifeTime = Lifetime;
            var serviceType = ServiceType(implementationType);
            var descriptor = new ServiceDescriptor(serviceType, implementationType, lifeTime);
            services.Add(descriptor);
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class SingletonServiceAttribute : ServiceAttribute
    {
        public override ServiceLifetime Lifetime => ServiceLifetime.Singleton;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TransientServiceAttribute : ServiceAttribute
    {
        public override ServiceLifetime Lifetime => ServiceLifetime.Transient;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultServiceAttribute<T> : ServiceAttribute
    {
        public override Type ServiceType(Type implementationType) => GetType().GetGenericArguments()[0];

        public override ServiceLifetime Lifetime =>
            GetType()
            .GetGenericArguments()[0]
            .GetCustomAttributes()
            .Where(a => a is ServiceAttribute)
            .Cast<ServiceAttribute>()
            .Single().Lifetime;
    }

}
