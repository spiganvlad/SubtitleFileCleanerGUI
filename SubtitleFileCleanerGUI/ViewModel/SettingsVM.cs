using System.Windows;
using System.Collections.Generic;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class SettingsVM : NotifyPropertyChangedObject
    {
        private readonly IDefaultFileManipulator defaultFileManipulator;

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

        public SettingsVM(IDefaultFileManipulator defaultFileManipulator, IEnumManipulator enumManipulator)
        {
            this.defaultFileManipulator = defaultFileManipulator;

            DefaultFile = defaultFileManipulator.GetDefaultFile<SubtitleFile>(DefaultFileTypes.Custom);
            Cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();
        }

        private void SaveSettings()
        {
            defaultFileManipulator.SetDefaultFile(DefaultFile, DefaultFileTypes.Custom);
            MessageBox.Show("Settings saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreSettings() =>
            DefaultFile = defaultFileManipulator.GetDefaultFile<SubtitleFile>(DefaultFileTypes.Default);
    }
}
