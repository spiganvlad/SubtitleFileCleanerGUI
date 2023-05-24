using System.IO;

namespace SubtitleFileCleanerGUI.Service.IO
{
    public class UniquePathCreator : IUniquePathCreator
    {
        public string Create(string path)
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
