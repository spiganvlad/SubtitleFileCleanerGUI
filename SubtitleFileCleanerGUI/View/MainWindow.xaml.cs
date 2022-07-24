using System.Windows;
using SubtitleFileCleanerGUI.ViewModel;

namespace SubtitleFileCleanerGUI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowApplicationBase
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    }
}
