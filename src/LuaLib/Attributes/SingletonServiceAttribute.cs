using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace SanctuarySSLib.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
    public class ServiceScopeAttribute : Attribute
    {
        public static readonly ServiceScopeAttribute Default = new TransientServiceAttribute();

        public ServiceLifetime Scope { get; }
        public ServiceScopeAttribute(ServiceLifetime scope = ServiceLifetime.Transient)
        {
            Scope = scope;
        }
    }

    public class SingletonServiceAttribute : ServiceScopeAttribute
    {
        public SingletonServiceAttribute() : base(ServiceLifetime.Singleton)
        {
        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class TransientServiceAttribute : ServiceScopeAttribute
    {
        public TransientServiceAttribute() : base(ServiceLifetime.Transient)
        {
        }
    }
}
