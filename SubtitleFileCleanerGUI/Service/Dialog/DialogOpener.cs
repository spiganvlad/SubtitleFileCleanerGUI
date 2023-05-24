namespace SubtitleFileCleanerGUI.Service.Dialog
{
    public class DialogOpener : IDialogOpener
    {
        private readonly IOpenFileDialog openFileDialog;
        private readonly IOpenFolderDialog openFolderDialog;

        public DialogOpener(IOpenFileDialog openFileDialog, IOpenFolderDialog openFolderDialog)
        {
            this.openFileDialog = openFileDialog;
            this.openFolderDialog = openFolderDialog;
        }

        public bool? ShowFileDialog(out string filePath)
        {
            return openFileDialog.ShowDialog(out filePath);
        }

        public bool? ShowFolderDialog(out string folderPath)
        {
            return openFolderDialog.ShowDialog(out folderPath);
        }
    }
}
