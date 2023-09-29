using SubtitleFileCleanerGUI.Application.Abstractions.Enums;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite
{
    public interface IAsyncReaderFactory
    {
        public IAsyncReader CreateAsyncReader(ReadWriteType type);
    }
}
