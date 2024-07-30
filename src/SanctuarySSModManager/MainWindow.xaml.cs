using DiffMatchPatch;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.Models;
using SanctuarySSLib.ViewModel;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

using Path = System.IO.Path;

namespace SanctuarySSModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SSSUserSettings userSettings;
        DispatcherTimer timer;

        public async MainWindow(SSSUserSettings userSettings)
        {
            // https://stackoverflow.com/questions/48545971/how-can-i-pass-data-to-from-a-webbrowser-control
            var fontFam = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#Oswald_Bold");
            var fontFamilies = Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./resources/").ToList();
            var resourcenames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (var name in resourcenames)
            {
                var info = Assembly.GetExecutingAssembly().GetManifestResourceInfo(name);
            }
            var resourceUris = Assembly.GetEntryAssembly()
                   .GetCustomAttributes(typeof(AssemblyAssociatedContentFileAttribute), true)
                   .Cast<AssemblyAssociatedContentFileAttribute>()
                   .Select(attr => new Uri(attr.RelativeContentFilePath));
            this.userSettings = userSettings;
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));


            var model = DIContainer.Get<ShatteredSunModel>();
            model.Load();
            var viewModel = DIContainer.Get<ShatteredSunViewModel>();

            //var patch = new diff_match_patch();

            //var unitFilePath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua";
            //LuaDataLoader lua = new LuaDataLoader();
            //var result = lua.GetData(unitFilePath);
            //JsonSerializer

            //var root = "D:\\SteamLibrary\\steamapps\\common\\Sanctuary Shattered Sun Demo\\prototype\\RuntimeContent\\Lua\\common\\units\\unitsTemplates\\uel1001\\uel10"
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}