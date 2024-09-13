using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using SanctuarySSLib.RegistryClasses;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows;

namespace SanctuarySSModManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DIContainer.Initialize(ConfigureServices);
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services
                .AddSingleton<MainWindow>()
                .AddSingleton(s => s.GetService<RegistryPersister>().LoadFromRegistry<SSSUserSettings>(Registry.CurrentUser, "UserSettings"))
                .AddSingleton(s => s.GetService<RegistryPersister>().LoadFromRegistry<SSSModManagerSettings>(Registry.LocalMachine, "ModManagerSettings"));

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = DIContainer.Get<MainWindow>();
            mainWindow?.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            var registryPersister = DIContainer.Get<RegistryPersister>();
            var userSettings = DIContainer.Get<SSSUserSettings>();
            var modManagerSettings = DIContainer.Get<SSSModManagerSettings>();

            registryPersister.SaveToRegistry(Registry.CurrentUser, userSettings, "UserSettings");
            registryPersister.SaveToRegistry(Registry.LocalMachine, modManagerSettings, "ModManagerSettings");
        }
    }

}
