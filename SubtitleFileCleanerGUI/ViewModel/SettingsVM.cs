using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service.Input;
using SubtitleFileCleanerGUI.Service.Utility;
using SubtitleFileCleanerGUI.Service.Settings;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class SettingsVM : ObservableObject
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
                OnPropertyChanged(nameof(DefaultFile));
            }
        }

        public ICommand SaveSettingsCommand => saveSettingsCommand;
        public ICommand RestoreSettingsCommand => restoreSettingsCommand;

        public SettingsVM(IDefaultFileManipulator defaultFileManipulator, IEnumManipulator enumManipulator,
            IParameterlessCommandCreator parameterlessCommandCreator)
        {
            this.defaultFileManipulator = defaultFileManipulator;

            DefaultFile = defaultFileManipulator.GetDefaultFile(DefaultFileTypes.Custom);
            Cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();

            saveSettingsCommand = parameterlessCommandCreator.Create(SaveSettings);
            restoreSettingsCommand = parameterlessCommandCreator.Create(RestoreSettings);
        }

        private void SaveSettings()
        {
            defaultFileManipulator.SetDefaultFile(DefaultFile, DefaultFileTypes.Custom);
            MessageBox.Show("Settings saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreSettings() =>
            DefaultFile = defaultFileManipulator.GetDefaultFile(DefaultFileTypes.Default);
    }
}
