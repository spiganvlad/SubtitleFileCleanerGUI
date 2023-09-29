using System.IO;
using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;

namespace SubtitleFileCleanerGUI.Application.Service.ReadWrite.FileSystem
{
    public class FileSystemAsyncReader : IAsyncReader
    {
        public async Task<byte[]> ReadAsync(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var resultBytes = new byte[fs.Length];
            await fs.ReadAsync(resultBytes);
            return resultBytes;
        }
    }
}
