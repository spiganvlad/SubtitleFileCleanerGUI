using Ookii.Dialogs.Wpf;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog;

namespace SubtitleFileCleanerGUI.Application.Service.Dialog
{
    public class OokiiOpenFileDialog : IOpenFileDialog
    {
        public bool? ShowDialog(out string filePath)
        {
            VistaOpenFileDialog dialog = new();
            var success = dialog.ShowDialog();

            if (success.Value)
                filePath = dialog.FileName;
            else
                filePath = string.Empty;

            return success;
        }
    }
}
