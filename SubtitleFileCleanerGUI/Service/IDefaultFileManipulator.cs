using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Attributes;

namespace SubtitleFileCleanerGUI.Service
{
    // Supported settings types
    public enum DefaultFileTypes
    {
        [SinglePath("DefaultFiles:Default")]
        Default,
        [SinglePath("DefaultFiles:Custom")]
        Custom
    }

    public interface IDefaultFileManipulator
    {
        public T GetDefaultFile<T>(DefaultFileTypes fileType) where T: SubtitleFile, new();
        public void SetDefaultFile(SubtitleFile file, DefaultFileTypes fileType);
    }
}
