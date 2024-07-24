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
                .AddSingleton<MainWindow>()
                .AddSingleton(SSSUserSettings.CreateInstance);
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = DIContainer.Services.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }

}
