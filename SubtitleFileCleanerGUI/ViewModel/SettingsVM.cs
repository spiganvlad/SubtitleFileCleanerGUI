using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service;
using SubtitleFileCleanerGUI.Service.Input;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class SettingsVM : NotifyPropertyChangedObject
    {
        private readonly IDefaultFileManipulator defaultFileManipulator;

        private readonly ICommand saveSettingsCommand;
        private readonly ICommand restoreSettingsCommand;

        private SubtitleFile defaultFile;

        public IEnumerable<SubtitleCleaners> Cleaners { get; }
        public SubtitleFile DefaultFile
        { 
            get => defaultFile;
            private set
            {
                defaultFile = value;
                OnPropertyChanged("DefaultFile");
            }
        }

        public ICommand SaveSettingsCommand => saveSettingsCommand;
        public ICommand RestoreSettingsCommand => restoreSettingsCommand;

        public SettingsVM(IDefaultFileManipulator defaultFileManipulator, IEnumManipulator enumManipulator,
            ICommandCreator commandCreator)
        {
            this.defaultFileManipulator = defaultFileManipulator;

            DefaultFile = defaultFileManipulator.GetDefaultFile<SubtitleFile>(DefaultFileTypes.Custom);
            Cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();

            saveSettingsCommand = commandCreator.Create(SaveSettings);
            restoreSettingsCommand = commandCreator.Create(RestoreSettings);
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
