namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog
{
    public interface IOpenFolderDialog
    {
        public bool? ShowDialog(out string folderPath);
    }
}
