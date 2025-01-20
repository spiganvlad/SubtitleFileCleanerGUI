using Ookii.Dialogs.Wpf;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog;

namespace SubtitleFileCleanerGUI.Application.Service.Dialog
{
    public class OokiiOpenFolderDialog : IOpenFolderDialog
    {
        public bool? ShowDialog(out string folderPath)
        {
            VistaFolderBrowserDialog dialog = new();
            var success = dialog.ShowDialog();

            folderPath = dialog.SelectedPath;
            return success;
        }
    }
}
