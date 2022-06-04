using System.Windows;
using SubtitleFileCleanerGUI.ViewModel;

namespace SubtitleFileCleanerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    }
}
