namespace SubtitleFileCleanerGUI.Service.Dialog
{
    public interface IOpenFileDialog
    {
        public bool? ShowDialog(out string filePath);
    }
}
