using System.Threading.Tasks;
using System.IO;

namespace SubtitleFileCleanerGUI.Service
{
    public static class FileManipulator
    {
        public static async Task<byte[]> ReadFileAsync(string filePath)
        {
            using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
            byte[] resultBytes = new byte[fs.Length];
            await fs.ReadAsync(resultBytes, 0, resultBytes.Length);
            return resultBytes;
        }

        public static async Task WriteFileAsync(string filePath, byte[] textBytes)
        {
            using FileStream fs = new(filePath, FileMode.CreateNew, FileAccess.Write);
            await fs.WriteAsync(textBytes, 0, textBytes.Length);
        }
    }
}
