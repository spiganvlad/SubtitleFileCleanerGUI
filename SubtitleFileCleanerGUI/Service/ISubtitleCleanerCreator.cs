using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service
{
    public interface ISubtitleCleanerCreator
    {
        public ISubtitleCleanerAsync Create(SubtitleCleaners cleaner);
    }
}
