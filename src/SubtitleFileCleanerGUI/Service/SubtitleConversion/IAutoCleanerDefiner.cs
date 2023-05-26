using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface IAutoCleanerDefiner
    {
        public SubtitleCleaners Define(string fileExtension);
    }
}
