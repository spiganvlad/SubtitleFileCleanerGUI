using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service
{
    public interface IAutoCleanerDefiner
    {
        public SubtitleCleaners Define(string fileExtension);
    }
}
