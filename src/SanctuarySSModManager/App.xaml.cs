using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.WorkInProgressNotUsed;
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
            services.AddSingleton(ModManagerMetaData.CreateInstance );
            services.AddSingleton<MainWindow>();
            services.AddSingleton<SanctuaryUnitData>();
            services.AddTransient<LuaDataLoader>();

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = DIContainer.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }

}
