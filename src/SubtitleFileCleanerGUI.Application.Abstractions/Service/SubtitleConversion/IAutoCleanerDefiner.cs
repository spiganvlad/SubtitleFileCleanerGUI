using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion
{
    public interface IAutoCleanerDefiner
    {
        public SubtitleCleaners Define(string fileExtension);
    }
}
