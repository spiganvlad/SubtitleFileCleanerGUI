using System.IO;
using System.Threading.Tasks;

namespace SubtitleFileCleanerGUI.Service.IO
{
    public class FileManipulator : IFileManipulator
    {
        public async Task<byte[]> ReadFileAsync(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var resultBytes = new byte[fs.Length];
            await fs.ReadAsync(resultBytes);
            return resultBytes;
        }

        public async Task WriteFileAsync(string filePath, byte[] textBytes)
        {
            using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
            await fs.WriteAsync(textBytes);
        }
    }
}
