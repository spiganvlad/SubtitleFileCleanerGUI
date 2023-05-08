using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service.Input;
using SubtitleFileCleanerGUI.Service.Dialog;
using SubtitleFileCleanerGUI.Service.Utility;
using SubtitleFileCleanerGUI.Service.Settings;
using SubtitleFileCleanerGUI.Service.ModelCreation;
using SubtitleFileCleanerGUI.Service.SubtitleConversion;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class MainVM
    {
        private readonly ISubtitleFileConverter fileConverter;
        private readonly ISubtitleStatusFileCreator fileCreator;
        private readonly ISettingsWindowCreator settingsWindowCreator;
        private readonly IOpenFileDialog fileDialog;
        private readonly IOpenFolderDialog folderDialog;

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

        public MainVM(ISubtitleFileConverter fileConverter, IEnumManipulator enumManipulator, ISubtitleStatusFileCreator fileCreator,
            ISettingsWindowCreator settingsWindowCreator, IParameterlessCommandCreator parameterlessCommandCreator,
            IParameterizedCommandCreator parameterizedCommandCreator, IOpenFileDialog fileDialog, IOpenFolderDialog folderDialog)
        {
            this.fileConverter = fileConverter;
            this.fileCreator = fileCreator;
            this.settingsWindowCreator = settingsWindowCreator;
            this.fileDialog = fileDialog;
            this.folderDialog = folderDialog;

            Files = new ObservableCollection<SubtitleStatusFile>();
            Cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();

            addFileCommand = parameterlessCommandCreator.Create(AddFile);
            removeFileCommand = parameterizedCommandCreator.Create<SubtitleStatusFile>(RemoveFile);
            removeAllFileCommand = parameterlessCommandCreator.Create(RemoveAllFile);
            convertFileCommand = parameterizedCommandCreator.Create<SubtitleStatusFile>(async file => await ConvertFileAsync(file));
            convertAllFilesCommand = parameterlessCommandCreator.Create(async () => await ConvertAllFilesAsync());
            getFileLocationCommand = parameterizedCommandCreator.Create<SubtitleFile>(GetFileLocation);
            getFileDestinationCommand = parameterizedCommandCreator.Create<SubtitleFile>(GetFileDestination);
            previewDragOverCommand = parameterizedCommandCreator.Create<DragEventArgs>(PreviewDragOver);
            dropFileCommand = parameterizedCommandCreator.Create<DragEventArgs>(DropFile);
            openSettingsCommand = parameterlessCommandCreator.Create(OpenSettings);
        }

        private void AddFile()
        {
            Files.Add(fileCreator.Create());
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
                await fileConverter.ConvertAsync(file.File);
                file.Status.StatusType = StatusTypes.CompletedProcess;
            }
            catch (Exception e)
            {
                file.Status.StatusType = StatusTypes.FailedProcess;
                file.Status.TextInfo += "\n" + e.Message;
            }
        }

        private async Task ConvertAllFilesAsync()
        {
            foreach (var file in Files)
                await ConvertFileAsync(file);
        }

        private void GetFileLocation(SubtitleFile file)
        {
            var success = fileDialog.ShowDialog(out string filePath);

            if (success.Value)
                file.PathLocation = filePath;
        }

        private void GetFileDestination(SubtitleFile file)
        {
            var success = folderDialog.ShowDialog(out string folderPath);

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
            catch
            {
                MessageBox.Show("Unexpected error", "Unable to set the dropped file", MessageBoxButton.OK, MessageBoxImage.Error);
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

        // Creates new files if they were dropped into the datagrid or more than one file was dropped in the text box/button
        private void DropFileAddNew(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                var file = fileCreator.Create();
                file.File.PathLocation = filePath;
                Files.Add(file);
            }
        }

        public void OpenSettings()
        {
            settingsWindowCreator.Create().Show();
        }
    }
}
