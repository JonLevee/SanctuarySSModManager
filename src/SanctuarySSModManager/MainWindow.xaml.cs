using DiffMatchPatch;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSLib.LuaUtil;
using SanctuarySSLib.MiscUtil;
using SanctuarySSLib.Models;
using SanctuarySSLib.ViewModel;
using SanctuarySSModManager.Controls;
using SanctuarySSModManager.Extensions;
using SanctuarySSModManager.MiscUtil;
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
using System;
using System.Runtime.InteropServices;

using Path = System.IO.Path;

namespace SanctuarySSModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("SanctuaryCPPLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AddNumbers(int a, int b);

        public Tooltips Tooltips { get; }

        public MainWindow()
        {
            // https://stackoverflow.com/questions/48545971/how-can-i-pass-data-to-from-a-webbrowser-control
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
            Tooltips = DIContainer.Get<Tooltips>();
            DataContext = DIContainer.Get<SSSCombinedSettings>();
            OuterDock.SizeChanged += MainWindow_SizeChanged;

            int result = AddNumbers(5, 7);
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
    }
}