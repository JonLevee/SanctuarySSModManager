using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using SanctuarySSLib.MiscUtil;
using System.Configuration;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SanctuarySSModManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly record struct InstanceData(IObjectPersister persister, object instance, string data);
        private static readonly Dictionary<Type, InstanceData> instanceData = new Dictionary<Type, InstanceData>();

        public App()
        {
            DIContainer.Initialize(ConfigureServices);
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services
                .AddSingleton<MainWindow>()
                .AddSingleton(s => GetSettings<SSSUserSettings>(s.GetService<AppInfo>(), s.GetService<RegistryPersister>(), "UserSettings"))
                .AddSingleton(s => GetSettings<SSSModManagerSettings>(s.GetService<AppInfo>(), s.GetService<FilePersister>(), "ModManagerSettings"))
                ;
        }

        private T GetSettings<T>(AppInfo? appInfo, IObjectPersister? persister, string name)
            where T : class, new()
        {
            Contract.Assert(appInfo != null);
            Contract.Assert(persister != null);
            var instance = persister.Load<T>(name);
            instanceData.Add(typeof(T), new InstanceData(persister, instance, name));
            return instance;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = DIContainer.Get<MainWindow>();
            mainWindow?.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            foreach (var data in instanceData.Values)
            {
                data.persister.Save(data.instance, data.data);
            }
        }
    }

}
