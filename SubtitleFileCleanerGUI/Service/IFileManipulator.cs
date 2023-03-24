using System.Threading.Tasks;

namespace SubtitleFileCleanerGUI.Service
{
    public interface IFileManipulator
    {
        public Task<byte[]> ReadFileAsync(string path);
        public Task WriteFileAsync(string path, byte[] bytes);
    }
}
