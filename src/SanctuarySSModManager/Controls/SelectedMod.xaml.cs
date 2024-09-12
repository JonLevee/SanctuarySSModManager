using SanctuarySSLib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SanctuarySSModManager.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SelectedMod : UserControl, INotifyPropertyChanged
    {
        public SSSUserSettings UserSettings { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public FolderModeEnum FolderMode
        {
            get => UserSettings.FolderMode;
            set
            {
                UserSettings.FolderMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FolderMode"));
            }
        }

        private List<FolderModeEnum> folderModes;
        public List<FolderModeEnum> FolderModes => folderModes ??= Enum.GetValues<FolderModeEnum>().ToList();


        public SelectedMod()
        {
            UserSettings = DIContainer.Get<SSSUserSettings>();
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
