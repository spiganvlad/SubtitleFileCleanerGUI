using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class SettingsVM : INotifyPropertyChanged
    {
        private CustomSettings settings;
        private RelayCommand saveSettingsCommand;
        private RelayCommand restoreSettingCommand;

        public CustomSettings Settings 
        { 
            get { return settings; }
            private set {
                settings = value;
                OnPropertyChanged("Settings");
            }
        }
        public IEnumerable<SubtitleCleaners> Cleaners { get; private set; }

        public RelayCommand SaveSettingsCommand => saveSettingsCommand ??= new RelayCommand(_ => SaveSettings());
        public RelayCommand RestoreSettingCommand => restoreSettingCommand ??= new RelayCommand(_ => RestoreSettings());
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsVM()
        {
            Settings = SettingsManipulator.LoadSettings(SettingsTypes.Custom);
            Cleaners = Enum.GetValues(typeof(SubtitleCleaners)).Cast<SubtitleCleaners>();
        }

        private void SaveSettings()
        {
            SettingsManipulator.SaveSettings(Settings, SettingsTypes.Custom);
            MessageBox.Show("Settings saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreSettings()
        {
            Settings = SettingsManipulator.LoadSettings(SettingsTypes.Default);
        }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
