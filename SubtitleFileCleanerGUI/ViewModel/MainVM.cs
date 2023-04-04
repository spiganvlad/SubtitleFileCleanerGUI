using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ookii.Dialogs.Wpf;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service;
using SubtitleFileCleanerGUI.Service.Input;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class MainVM
    {
        private readonly ISubtitleFileConverter fileConverter;
        private readonly IDefaultFileManipulator defaultFileManipulator;
        private readonly ISettingsWindowCreator settingsWindowCreator;

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

        public MainVM(ISubtitleFileConverter fileConverter, IEnumManipulator enumManipulator, IDefaultFileManipulator defaultFileManipulator,
            ISettingsWindowCreator settingsWindowCreator, ICommandCreator commandCreator, IGenericCommandCreator genericCommandCreator)
        {
            this.fileConverter = fileConverter;
            this.defaultFileManipulator = defaultFileManipulator;
            this.settingsWindowCreator = settingsWindowCreator;

            Files = new ObservableCollection<SubtitleStatusFile>();
            Cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();

            addFileCommand = commandCreator.Create(AddFile);
            removeFileCommand = genericCommandCreator.Create<SubtitleStatusFile>(RemoveFile);
            removeAllFileCommand = commandCreator.Create(RemoveAllFile);
            convertFileCommand = genericCommandCreator.Create<SubtitleStatusFile>(async file => await ConvertFileAsync(file));
            convertAllFilesCommand = commandCreator.Create(async () => await ConvertAllFilesAsync());
            getFileLocationCommand = genericCommandCreator.Create<SubtitleFile>(GetFileLocation);
            getFileDestinationCommand = genericCommandCreator.Create<SubtitleFile>(GetFileDestination);
            previewDragOverCommand = genericCommandCreator.Create<DragEventArgs>(PreviewDragOver);
            dropFileCommand = genericCommandCreator.Create<DragEventArgs>(DropFile);
            openSettingsCommand = commandCreator.Create(OpenSettings);
        }

        private void AddFile() =>
            Files.Add(defaultFileManipulator.GetDefaultFile<SubtitleStatusFile>(DefaultFileTypes.Custom));
        
        private void RemoveFile(SubtitleStatusFile file) =>
            Files.Remove(file);
        
        private void RemoveAllFile() =>
            Files.Clear();

        private async Task ConvertFileAsync(SubtitleStatusFile file)
        {
            try
            {
                file.StatusType = StatusTypes.ConvertingProcess;
                await fileConverter.ConvertAsync(file);
                file.StatusType = StatusTypes.CompletedProcess;
            }
            catch (Exception e)
            {
                file.StatusType = StatusTypes.FailedProcess;
                file.TextInfo += "\n" + e.Message;
            }
        }

        private async Task ConvertAllFilesAsync()
        {
            foreach (var file in Files)
                await ConvertFileAsync(file);
        }

        private void GetFileLocation(SubtitleFile file)
        {
            VistaOpenFileDialog dialog = new();
            bool? success = dialog.ShowDialog();

            if (success == true)
                file.PathLocation = dialog.FileName;
        }

        private void GetFileDestination(SubtitleFile file)
        {
            VistaFolderBrowserDialog dialog = new();
            bool? success = dialog.ShowDialog();

            if (success == true)
                file.PathDestination = dialog.SelectedPath;
        }

        private void PreviewDragOver(DragEventArgs eventArgs) =>
            eventArgs.Handled = true;
        
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

        // Search parant grid 
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
                var file = defaultFileManipulator.GetDefaultFile<SubtitleStatusFile>(DefaultFileTypes.Custom);
                file.PathLocation = filePath;
                Files.Add(file);
            }
        }

        public void OpenSettings() =>
            settingsWindowCreator.Create().Show();
    }
}
