using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;
using SubtitleFileCleanerGUI.Service.Input;
using SubtitleFileCleanerGUI.Service.Settings;
using SubtitleFileCleanerGUI.Service.Utility;

namespace SubtitleFileCleanerGUI.UI.ViewModel
{
    public class SettingsVM : ObservableObject
    {
        private readonly ILogger<SettingsVM> logger;
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

        public SettingsVM(ILogger<SettingsVM> logger, IDefaultFileManipulator defaultFileManipulator,
            IEnumManipulator enumManipulator, ICommandCreator commandCreator)
        {
            this.logger = logger;
            this.defaultFileManipulator = defaultFileManipulator;

            DefaultFile = defaultFileManipulator.GetDefaultFile(DefaultFileTypes.Custom);
            Cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();

            saveSettingsCommand = commandCreator.Create(SaveSettings);
            restoreSettingsCommand = commandCreator.Create(RestoreSettings);
        }

        private void SaveSettings()
        {
            defaultFileManipulator.SetDefaultFile(DefaultFile, DefaultFileTypes.Custom);

            logger.LogInformation("Settings saved successfully");
            MessageBox.Show("Settings saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreSettings()
        {
            DefaultFile = defaultFileManipulator.GetDefaultFile(DefaultFileTypes.Default);
        }
    }
}
