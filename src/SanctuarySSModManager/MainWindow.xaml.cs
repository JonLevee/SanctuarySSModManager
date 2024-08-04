using DiffMatchPatch;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSLib.ViewModel;
using SanctuarySSModManager.Controls;
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

        public MainWindow(SSSUserSettings userSettings)
        {
            // https://stackoverflow.com/questions/48545971/how-can-i-pass-data-to-from-a-webbrowser-control
            this.userSettings = userSettings;
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));


            OuterDock.SizeChanged += MainWindow_SizeChanged;
            //var patch = new diff_match_patch();

            //var unitFilePath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua";
            //LuaDataLoader lua = new LuaDataLoader();
            //var result = lua.GetData(unitFilePath);
            //JsonSerializer

            //var root = "D:\\SteamLibrary\\steamapps\\common\\Sanctuary Shattered Sun Demo\\prototype\\RuntimeContent\\Lua\\common\\units\\unitsTemplates\\uel1001\\uel10"
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var topLeft = Tab.TranslatePoint(new Point(0, 0), OuterDock);
            var width = e.NewSize.Width - topLeft.X;
            var height = e.NewSize.Height - topLeft.Y;
            Tab.Width = width;
            Tab.Height = height;
            //UnitViewControl.Grid.Width= width;
            //UnitViewControl.Grid.Height = height;
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();
            await LoadModels();
        }

        private async Task LoadModels()
        {
            LoadingPanel.Visibility = Visibility.Visible;
            SelectedModPanel.Visibility = Visibility.Collapsed;
            UpdateLayout();
            var model = DIContainer.Get<ShatteredSunModel>();
            await model.Load();
            File.WriteAllText("root.json", model.GetJson());
            UnitViewControl.Load(model);
            UnitViewControl.UpdateUnits();
            //var model = DIContainer.Get<ShatteredSunModel>();
            //await model.Load();
            //var viewModel = DIContainer.Get<ShatteredSunViewModel>();
            //await viewModel.Load(model);
            //await Task.Run(() => Thread.Sleep(2000));
            LoadingPanel.Visibility = Visibility.Collapsed;
            SelectedModPanel.Visibility = Visibility.Visible;
            await Task.CompletedTask;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}