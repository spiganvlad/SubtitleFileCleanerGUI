using SubtitleFileCleanerGUI.Attributes;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service.Settings
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
        public SubtitleFile GetDefaultFile(DefaultFileTypes fileType);
        public void SetDefaultFile(SubtitleFile file, DefaultFileTypes fileType);
    }
}
