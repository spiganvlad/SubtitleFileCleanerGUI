using SubtitleFileCleanerGUI.Application.Abstractions.Enums;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite
{
    public interface IPathGeneratorFactory
    {
        public IPathGenerator CreatePathGenerator(ReadWriteType type);
    }
}
