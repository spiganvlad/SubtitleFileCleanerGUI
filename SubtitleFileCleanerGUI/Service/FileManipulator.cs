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
            await fs.ReadAsync(resultBytes);
            return resultBytes;
        }

        public static async Task WriteFileAsync(string filePath, byte[] textBytes)
        {
            using FileStream fs = new(filePath, FileMode.CreateNew, FileAccess.Write);
            await fs.WriteAsync(textBytes);
        }

        public static string CreateUniquePath(string pathLocation, string pathDestination)
        {
            string path = pathDestination + "\\" + Path.ChangeExtension(Path.GetFileName(pathLocation), ".txt");

            if (File.Exists(path))
            {
                string fileDir = Path.GetDirectoryName(path);
                string fileName = Path.GetFileNameWithoutExtension(path);
                string fileExt = Path.GetExtension(path);

                int i = 1;
                do
                {
                    path = fileDir + "\\" + fileName + $" ({i++})" + fileExt;
                } while (File.Exists(path));
            }

            return path;
        }
    }
}
