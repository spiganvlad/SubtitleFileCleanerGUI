using SubtitleFileCleanerGUI.Application.Abstractions.Enums;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite
{
    public interface IAsyncWriterFactory
    {
        public IAsyncWriter CreateAsyncWriter(ReadWriteType type);
    }
}
