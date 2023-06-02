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

            if (success.Value)
                folderPath = dialog.SelectedPath;
            else
                folderPath = string.Empty;

            return success;
        }
    }
}
