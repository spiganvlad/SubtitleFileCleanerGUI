using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface ISubtitleCleanerCreator
    {
        public ISubtitleCleanerAsync Create(SubtitleCleaners cleaner);
    }
}
