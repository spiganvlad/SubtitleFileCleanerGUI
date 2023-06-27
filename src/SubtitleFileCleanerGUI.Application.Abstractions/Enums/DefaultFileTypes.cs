using SubtitleFileCleanerGUI.Domain.Attributes;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Enums
{
    // Supported settings types
    public enum DefaultFileTypes
    {
        [SinglePath("DefaultFiles:Default")]
        Default,
        [SinglePath("DefaultFiles:Custom")]
        Custom
    }
}
