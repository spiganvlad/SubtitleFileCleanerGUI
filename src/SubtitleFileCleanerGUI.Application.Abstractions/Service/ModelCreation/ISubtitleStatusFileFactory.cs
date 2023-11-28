using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation
{
    public interface ISubtitleStatusFileFactory
    {
        public SubtitleStatusFile CreateWithStatusWatcher(DefaultFileTypes type);
    }
}
