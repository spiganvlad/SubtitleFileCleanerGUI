using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ookii.Dialogs.Wpf;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service;

namespace SubtitleFileCleanerGUI.ViewModel
{
    public class MainVM
    {
        private RelayCommand addFileCommand;
        private RelayCommand removeFileCommand;
        private RelayCommand convertFileCommand;
        private RelayCommand convertAllFilesCommand;
        private RelayCommand getFileLocationCommand;
        private RelayCommand getFileDestinationCommand;
        private RelayCommand previewDragOverCommand;
        private RelayCommand dropFileCommand;
        private RelayCommand cleanerChangedCommand;

        public ObservableCollection<SubtitleFile> SubtitleFiles { get; }
        public IEnumerable<SubtitleCleaners> Cleaners { get; }
        public SubtitleCleaners SelectedCleaner { get; }

        public RelayCommand AddFileCommand => addFileCommand ??= new RelayCommand(item => AddFile());
        public RelayCommand RemoveFileCommand => removeFileCommand ??= new RelayCommand(item => RemoveFile(item));
        public RelayCommand ConvertFileCommand => convertFileCommand ??= new RelayCommand(item => ConvertFile(item));
        public RelayCommand ConvertAllFilesCommand => convertAllFilesCommand ??= new RelayCommand(item => ConvertAllFiles());
        public RelayCommand GetFileLocationCommand => getFileLocationCommand ??= new RelayCommand(item => GetFileLocation(item));
        public RelayCommand GetFileDestinationCommand => getFileDestinationCommand ??= new RelayCommand(item => GetFileDestination(item));
        public RelayCommand PreviewDragOverCommand => previewDragOverCommand ??= new RelayCommand(item => PreviewDragOver(item));
        public RelayCommand DropFileCommand => dropFileCommand ??= new RelayCommand(item => DropFile(item));
        public RelayCommand CleanerChangedCommand => cleanerChangedCommand ??= new RelayCommand(item => CleanerChanged(item));

        public MainVM()
        {
            SubtitleFiles = new ObservableCollection<SubtitleFile>();
            Cleaners = Enum.GetValues(typeof(SubtitleCleaners)).Cast<SubtitleCleaners>();
            SelectedCleaner = SubtitleCleaners.Auto;
        }

        //try... catch...
        private void AddFile()
        {
            SubtitleFiles.Add(new SubtitleFile());
        }

        private void RemoveFile(object item)
        {
            if (item != null)
                SubtitleFiles.Remove((SubtitleFile)item);
        }

        private async void ConvertFile(object item)
        {
            try
            {
                if (item is not null and SubtitleFile file)
                    await new SubtitleFileConverter().ConvertFileAsync(file);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Can't convert file. Check the completeness of the input fields.", "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show(e.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConvertAllFiles()
        {
            if (SubtitleFiles.Count > 0)
                foreach (SubtitleFile file in SubtitleFiles)
                    Task.Run(async() => await new SubtitleFileConverter().ConvertFileAsync(file));
        }

        private void GetFileLocation(object item)
        {
            if (item is not null and SubtitleFile file)
            {
                VistaOpenFileDialog dialog = new();
                bool? success = dialog.ShowDialog();

                if (success == true)
                    file.PathLocation = dialog.FileName;
            }
        }

        private void GetFileDestination(object item)
        {
            if (item is not null and SubtitleFile file)
            {
                VistaFolderBrowserDialog dialog = new();
                bool? success = dialog.ShowDialog();

                if (success == true)
                    file.PathDestination = dialog.SelectedPath;
            }
        }

        private void PreviewDragOver(object item)
        {
            if (item is not null and DragEventArgs eventArgs)
                eventArgs.Handled = true;
        }

        private void DropFile(object item)
        {
            if (item is not null and DragEventArgs eventArgs)
            {
                string[] files;
                if (eventArgs.Data.GetDataPresent(DataFormats.FileDrop))
                    files = (string[])eventArgs.Data.GetData(DataFormats.FileDrop);
                else
                    return;

                // Define where the file was dropped
                if (eventArgs.Source is Button button)
                    DropFileFromButton(button, files[0]);
                else if (eventArgs.Source is Grid grid)
                    DropFileFromGrid(grid, files[0]);
                else if (eventArgs.Source is TextBox textBox)
                    DropFileSetPath(textBox, files[0]);
            }
        }

        // Search parant grid 
        private void DropFileFromButton(Button button, string filePath)
        {
            if (button.Parent is Grid grid)
                DropFileFromGrid(grid, filePath);
        }

        // Search children textBox
        private void DropFileFromGrid(Grid grid, string filePath)
        {
            foreach (object obj in grid.Children)
                if (obj is TextBox textBox)
                    DropFileSetPath(textBox, filePath);
        }

        // Set file path to text box
        private void DropFileSetPath(TextBox textBox, string filePath)
        {
            textBox.Text = filePath;
        }

        private void CleanerChanged(object item)
        {
            if (item is not null and object[] itemArr && itemArr.Length >= 2)
                if (itemArr[0] is not null and SubtitleFile file)
                    if (itemArr[1] is not null and SubtitleCleaners cleaner)
                        file.TargetCleaner = cleaner;
        }
    }
}
