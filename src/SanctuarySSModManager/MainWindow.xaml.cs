using DiffMatchPatch;
using Microsoft.Extensions.DependencyInjection;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;
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

        public MainWindow(SSSUserSettings userSettings)
        {
            var label = new Label();
            //var fontFam = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#Oswald_Bold");
            //foreach (FontFamily fontFamily in Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./resources/"))
            //{
            //    // Perform action.
            //}

            this.userSettings = userSettings;
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));

            ApplicationDirectoryRoot.Text = userSettings.ShatteredSunDirectoryRoot;


            //var patch = new diff_match_patch();

            //var unitFilePath = @"D:\SteamLibrary\steamapps\common\Sanctuary Shattered Sun Demo\prototype\RuntimeContent\Lua";
            //LuaDataLoader lua = new LuaDataLoader();
            //var result = lua.GetData(unitFilePath);
            //JsonSerializer

            //var root = "D:\\SteamLibrary\\steamapps\\common\\Sanctuary Shattered Sun Demo\\prototype\\RuntimeContent\\Lua\\common\\units\\unitsTemplates\\uel1001\\uel10"
        }

        private void SnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            var timer = Stopwatch.StartNew();
            var sourcePath = userSettings.FullModRootFolder;
            var targetPath = Path.Combine(userSettings.AppDataFolder, SnapshotName.Text, userSettings.ModRootFolder);
            var files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            foreach (var sourceFile in files)
            {
                var targetFile = sourceFile.Replace(sourcePath, targetPath);
                var targetDir = Path.GetDirectoryName(targetFile);
                if (null == targetDir)
                    throw new NullReferenceException();
                targetDir.EnsureDirectoryExists();
                File.Copy(sourceFile, targetFile);
            }
            timer.Stop();
        }
    }
}