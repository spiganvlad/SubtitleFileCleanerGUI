﻿using System.Windows;
using SubtitleFileCleanerGUI.ViewModel;

namespace SubtitleFileCleanerGUI.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsVM();
        }
    }
}
