using System.Windows;
using System.Collections.Generic;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class SettingsVM : NotifyPropertyChangedObject
    {
        private SubtitleFile defaultFile;
        private RelayCommand saveSettingsCommand;
        private RelayCommand restoreSettingsCommand;

        public IEnumerable<SubtitleCleaners> Cleaners { get; private set; }
        public SubtitleFile DefaultFile
        { 
            get => defaultFile;
            private set
            {
                defaultFile = value;
                OnPropertyChanged("DefaultFile");
            }
        }

        public RelayCommand SaveSettingsCommand => saveSettingsCommand ??= new RelayCommand(_ => SaveSettings());
        public RelayCommand RestoreSettingsCommand => restoreSettingsCommand ??= new RelayCommand(_ => RestoreSettings());

        public SettingsVM(SubtitleFile defaultFile)
        {
            DefaultFile = defaultFile;
            Cleaners = EnumManipulator<SubtitleCleaners>.GetAllEnumValues();
        }

        private void SaveSettings()
        {
            DefaultFilesManipulator.SaveSettings(DefaultFile, SettingsTypes.Custom);
            MessageBox.Show("Settings saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreSettings() =>
            DefaultFile = DefaultFilesManipulator.LoadSettings(SettingsTypes.Default);
    }
}
