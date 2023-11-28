using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Input;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.UI.ViewModel
{
    public class MainVM
    {
        private readonly ILogger<MainVM> logger;
        private readonly ISubtitleFileConverter fileConverter;
        private readonly ISubtitleStatusFileFactory fileCreator;
        private readonly ISettingsWindowCreator settingsWindowCreator;
        private readonly IDialogOpener dialogOpener;

        private readonly ICommand addFileCommand;
        private readonly ICommand removeFileCommand;
        private readonly ICommand removeAllFileCommand;
        private readonly ICommand convertFileCommand;
        private readonly ICommand convertAllFilesCommand;
        private readonly ICommand getFileLocationCommand;
        private readonly ICommand getFileDestinationCommand;
        private readonly ICommand previewDragOverCommand;
        private readonly ICommand dropFileCommand;
        private readonly ICommand openSettingsCommand;

        public ObservableCollection<SubtitleStatusFile> Files { get; }
        public IEnumerable<SubtitleCleaners> Cleaners { get; }

        public ICommand AddFileCommand => addFileCommand;
        public ICommand RemoveFileCommand => removeFileCommand;
        public ICommand RemoveAllFileCommand => removeAllFileCommand;
        public ICommand ConvertFileCommand => convertFileCommand;
        public ICommand ConvertAllFilesCommand => convertAllFilesCommand;
        public ICommand GetFileLocationCommand => getFileLocationCommand;
        public ICommand GetFileDestinationCommand => getFileDestinationCommand;
        public ICommand PreviewDragOverCommand => previewDragOverCommand;
        public ICommand DropFileCommand => dropFileCommand;
        public ICommand OpenSettingsCommand => openSettingsCommand;

        public MainVM(ILogger<MainVM> logger, ISubtitleFileConverter fileConverter, IEnumManipulator enumManipulator,
            ISubtitleStatusFileFactory fileCreator, ISettingsWindowCreator settingsWindowCreator, ICommandCreator commandCreator,
            IDialogOpener dialogOpener)
        {
            this.logger = logger;
            this.fileConverter = fileConverter;
            this.fileCreator = fileCreator;
            this.settingsWindowCreator = settingsWindowCreator;
            this.dialogOpener = dialogOpener;

            Files = new ObservableCollection<SubtitleStatusFile>();
            Cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();

            addFileCommand = commandCreator.Create(AddFile);
            removeFileCommand = commandCreator.Create<SubtitleStatusFile>(RemoveFile);
            removeAllFileCommand = commandCreator.Create(RemoveAllFile);
            convertFileCommand = commandCreator.Create<SubtitleStatusFile>(async file => await ConvertFileAsync(file));
            convertAllFilesCommand = commandCreator.Create(async () => await ConvertAllFilesAsync());
            getFileLocationCommand = commandCreator.Create<SubtitleFile>(GetFileLocation);
            getFileDestinationCommand = commandCreator.Create<SubtitleFile>(GetFileDestination);
            previewDragOverCommand = commandCreator.Create<DragEventArgs>(PreviewDragOver);
            dropFileCommand = commandCreator.Create<DragEventArgs>(DropFile);
            openSettingsCommand = commandCreator.Create(OpenSettings);
        }

        private void AddFile()
        {
            var subtitleFile = fileCreator.CreateWithStatusWatcher(DefaultFileTypes.Custom);
            subtitleFile.Status.StatusType = StatusTypes.WaitingProcess;

            Files.Add(subtitleFile);
        }
        
        private void RemoveFile(SubtitleStatusFile file)
        {
            Files.Remove(file);
        }
        
        private void RemoveAllFile()
        {
            Files.Clear();
        }

        private async Task ConvertFileAsync(SubtitleStatusFile file)
        {
            try
            {
                file.Status.StatusType = StatusTypes.ConvertingProcess;
                logger.LogInformation("Start converting \"{fileName}\" file", Path.GetFileName(file.File.PathLocation));

                await fileConverter.ConvertAsync(file.File);
                
                file.Status.StatusType = StatusTypes.CompletedProcess;
                logger.LogInformation("\"{fileName}\" file conversion completed successfully", Path.GetFileName(file.File.PathLocation));
            }
            catch (Exception ex)
            {
                file.Status.StatusType = StatusTypes.FailedProcess;
                file.Status.TextInfo += "\n" + ex.Message;
                logger.LogError(ex, "An error occurred while converting the \"{fileName}\" file", Path.GetFileName(file.File.PathLocation));
            }
        }

        private async Task ConvertAllFilesAsync()
        {
            foreach (var file in Files)
                await ConvertFileAsync(file);
        }

        private void GetFileLocation(SubtitleFile file)
        {
            var success = dialogOpener.ShowFileDialog(out string filePath);

            if (success.Value)
                file.PathLocation = filePath;
        }

        private void GetFileDestination(SubtitleFile file)
        {
            var success = dialogOpener.ShowFolderDialog(out string folderPath);

            if (success.Value)
                file.PathDestination = folderPath;
        }

        private void PreviewDragOver(DragEventArgs eventArgs)
        {
            eventArgs.Handled = true;
        }
        
        private void DropFile(DragEventArgs eventArgs)
        {
            try
            {
                string[] files;
                if (eventArgs.Data.GetDataPresent(DataFormats.FileDrop))
                    files = (string[])eventArgs.Data.GetData(DataFormats.FileDrop);
                else
                    return;

                // Define where the file was dropped
                if (eventArgs.OriginalSource is Border border && border.TemplatedParent is Button button)
                {
                    DropFileFromButton(button, files);
                    return;
                }

                Type type = eventArgs.OriginalSource.GetType();
                if ("TextBoxView" == type.Name)
                {
                    if (type.GetProperty("Parent").GetValue(eventArgs.OriginalSource, null) is ScrollViewer scroll &&
                        scroll.TemplatedParent is TextBox textBox)
                        DropFileSetPath(textBox, files);
                }
                else
                    DropFileAddNew(files);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while trying to drop the file(s). Source: {source}. " +
                    "Original Source: {originalSource}", eventArgs.Source, eventArgs.OriginalSource);
                MessageBox.Show("Unable to set the dropped file", "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Search parent grid 
        private void DropFileFromButton(Button button, string[] filePaths)
        {
            if (button.Parent is Grid grid)
                DropFileFromGrid(grid, filePaths);
        }

        // Search children textBox
        private void DropFileFromGrid(Grid grid, string[] filePaths)
        {
            foreach (object obj in grid.Children)
                if (obj is TextBox textBox)
                    DropFileSetPath(textBox, filePaths);
        }

        // Set file path to text box
        private void DropFileSetPath(TextBox textBox, string[] filePaths)
        {
            textBox.Text = filePaths[0];
            if (filePaths.Length > 1)
                DropFileAddNew(filePaths.Skip(1).ToArray());
        }

        // Creates new files if they were dropped into the DataGrid or more than one file was dropped in the text box/button
        private void DropFileAddNew(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                var file = fileCreator.CreateWithStatusWatcher(DefaultFileTypes.Custom);
                file.Status.StatusType = StatusTypes.WaitingProcess;
                file.File.PathLocation = filePath;
                Files.Add(file);
            }
        }

        public void OpenSettings()
        {
            try
            {
                settingsWindowCreator.Create().Show();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                MessageBox.Show("Unable to open settings window", "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
