namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog
{
    public interface IOpenFileDialog
    {
        public bool? ShowDialog(out string filePath);
    }
}
