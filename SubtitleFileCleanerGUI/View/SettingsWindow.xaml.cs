using System.Windows;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.ViewModel;

namespace SubtitleFileCleanerGUI.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : WindowApplicationBase
    {
        public SettingsWindow(SubtitleFile defaultFile)
        {
            InitializeComponent();
            DataContext = new SettingsVM(defaultFile);
        }
    }
}
