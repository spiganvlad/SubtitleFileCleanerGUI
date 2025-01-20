using System.IO;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;

namespace SubtitleFileCleanerGUI.Application.Service.ReadWrite.FileSystem
{
    public class FileSystemPathGenerator : IPathGenerator
    {
        public string CreateUniquePath(string path)
        {
            if (File.Exists(path))
            {
                string fileDir = Path.GetDirectoryName(path);
                string fileName = Path.GetFileNameWithoutExtension(path);
                string fileExt = Path.GetExtension(path);

                int i = 1;
                do
                {
                    path = $"{fileDir}\\{fileName} ({i++}){fileExt}";
                } while (File.Exists(path));
            }

            return path;
        }
    }
}
