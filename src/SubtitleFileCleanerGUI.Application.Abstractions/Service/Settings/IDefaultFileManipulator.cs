using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings
{
    public interface IDefaultFileManipulator
    {
        public SubtitleFile GetDefaultFile(DefaultFileTypes fileType);
        public void SetDefaultFile(SubtitleFile file, DefaultFileTypes fileType);
    }
}
