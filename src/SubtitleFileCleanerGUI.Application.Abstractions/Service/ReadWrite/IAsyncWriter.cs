using System.Threading.Tasks;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite
{
    public interface IAsyncWriter
    {
        public Task WriteAsync(string path, byte[] content);
    }
}
