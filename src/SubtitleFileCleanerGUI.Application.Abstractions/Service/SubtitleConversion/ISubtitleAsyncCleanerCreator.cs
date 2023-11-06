using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion
{
    public interface ISubtitleAsyncCleanerCreator
    {
        public ISubtitleCleanerAsync Create(SubtitleCleaners cleaner);
    }
}
