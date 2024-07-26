using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
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
                .AddSingleton<MainWindow>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = DIContainer.Get<MainWindow>();
            mainWindow?.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            var settings = DIContainer.Get<SSSUserSettings>();
            settings.Save();
        }
    }

}
