using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ookii.Dialogs.Wpf;
using SubtitleFileCleanerGUI.View;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class MainVM
    {
        private RelayCommand addFileCommand;
        private RelayCommand removeFileCommand;
        private RelayCommand removeAllFileCommand;
        private RelayCommand convertFileCommand;
        private RelayCommand convertAllFilesCommand;
        private RelayCommand getFileLocationCommand;
        private RelayCommand getFileDestinationCommand;
        private RelayCommand previewDragOverCommand;
        private RelayCommand dropFileCommand;
        private RelayCommand openSettingsCommand;

        public ObservableCollection<SubtitleStatusFile> Files { get; }
        public IEnumerable<SubtitleCleaners> Cleaners { get; }
        private SubtitleStatusFile DefaultFile { get; }
        private Window SettingsWindow { get; set; }

        public RelayCommand AddFileCommand => addFileCommand ??= new RelayCommand(_ => AddFile());
        public RelayCommand RemoveFileCommand => removeFileCommand ??= new RelayCommand(item => RemoveFile(item));
        public RelayCommand RemoveAllFileCommand => removeAllFileCommand ??= new RelayCommand(_ => RemoveAllFile());
        public RelayCommand ConvertFileCommand => convertFileCommand ??= new RelayCommand(item => ConvertFile(item));
        public RelayCommand ConvertAllFilesCommand => convertAllFilesCommand ??= new RelayCommand(_ => ConvertAllFiles());
        public RelayCommand GetFileLocationCommand => getFileLocationCommand ??= new RelayCommand(item => GetFileLocation(item));
        public RelayCommand GetFileDestinationCommand => getFileDestinationCommand ??= new RelayCommand(item => GetFileDestination(item));
        public RelayCommand PreviewDragOverCommand => previewDragOverCommand ??= new RelayCommand(item => PreviewDragOver(item));
        public RelayCommand DropFileCommand => dropFileCommand ??= new RelayCommand(item => DropFile(item));
        public RelayCommand OpenSettingsCommand => openSettingsCommand ??= new RelayCommand(_ => OpenSettings());

        public MainVM()
        {
            Files = new ObservableCollection<SubtitleStatusFile>();
            Cleaners = EnumManipulator<SubtitleCleaners>.GetAllEnumValues();
            DefaultFile = DefaultFilesManipulator.LoadSettings(SettingsTypes.Custom).CastTo<SubtitleStatusFile>();
            SettingsWindow = new SettingsWindow(DefaultFile);
        }

        private void AddFile() => Files.Add(DefaultFile.Clone());

        private void RemoveFile(object item)
        {
            if (item != null)
                Files.Remove((SubtitleStatusFile)item);
        }

        private void RemoveAllFile() => Files.Clear();

        private void ConvertFile(object item)
        {
            if (item is SubtitleStatusFile file)
                Task.Run(async () => await ConvertFileAsync(file));
        }

        private void ConvertAllFiles()
        {
            foreach (SubtitleStatusFile file in Files)
                Task.Run(async () => await ConvertFileAsync(file));
        }

        private async Task ConvertFileAsync(SubtitleStatusFile file)
        {
            try
            {
                file.StatusType = StatusTypes.ConvertingProcess;
                await SubtitleFileConverter.ConvertFileAsync(file);
                file.StatusType = StatusTypes.CompletedProcess;
            }
            catch (Exception e)
            {
                file.StatusType = StatusTypes.FailedProcess;
                file.TextInfo += "\n" + e.Message;
            }
        }

        private void GetFileLocation(object item)
        {
            if (item is SubtitleFile file)
            {
                VistaOpenFileDialog dialog = new();
                bool? success = dialog.ShowDialog();

                if (success == true)
                    file.PathLocation = dialog.FileName;
            }
        }

        private void GetFileDestination(object item)
        {
            if (item is SubtitleFile file)
            {
                VistaFolderBrowserDialog dialog = new();
                bool? success = dialog.ShowDialog();

                if (success == true)
                    file.PathDestination = dialog.SelectedPath;
            }
        }

        private void PreviewDragOver(object item)
        {
            if (item is DragEventArgs eventArgs)
                eventArgs.Handled = true;
        }

        private void DropFile(object item)
        {
            try
            {
                if (item is DragEventArgs eventArgs)
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
                SubtitleStatusFile file = DefaultFile.Clone();
                file.PathLocation = filePath;
                Files.Add(file);
            }
        }

        public void OpenSettings()
        {
            if (SettingsWindow.IsLoaded)
                SettingsWindow.Focus();
            else
            {
                SettingsWindow = new SettingsWindow(DefaultFile);
                SettingsWindow.Show();
            }
        }
    }
}
