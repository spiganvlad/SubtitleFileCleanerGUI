using System.IO;
using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;

namespace SubtitleFileCleanerGUI.Application.Service.ReadWrite.FileSystem
{
    public class FileSystemAsyncWriter : IAsyncWriter
    {
        public async Task WriteAsync(string path, byte[] content)
        {
            using var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            await fs.WriteAsync(content);
        }
    }
}
