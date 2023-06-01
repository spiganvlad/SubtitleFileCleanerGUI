using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface ISubtitleCleanerCreator
    {
        public ISubtitleCleanerAsync Create(SubtitleCleaners cleaner);
    }
}
