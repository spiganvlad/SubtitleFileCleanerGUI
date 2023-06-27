using SubtitleFileCleanerGUI.UI.ViewModel;

namespace SubtitleFileCleanerGUI.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowApplicationBase
    {
        public MainWindow(MainVM dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
