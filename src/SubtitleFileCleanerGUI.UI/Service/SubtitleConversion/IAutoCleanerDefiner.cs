using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface IAutoCleanerDefiner
    {
        public SubtitleCleaners Define(string fileExtension);
    }
}
