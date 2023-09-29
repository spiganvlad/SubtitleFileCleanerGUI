using System.Threading.Tasks;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite
{
    public interface IAsyncReader
    {
        public Task<byte[]> ReadAsync(string path);
    }
}
