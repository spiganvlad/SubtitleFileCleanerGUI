﻿using SubtitleFileCleanerGUI.UI.ViewModel;

namespace SubtitleFileCleanerGUI.UI.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : WindowApplicationBase
    {
        public SettingsWindow(SettingsVM dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
