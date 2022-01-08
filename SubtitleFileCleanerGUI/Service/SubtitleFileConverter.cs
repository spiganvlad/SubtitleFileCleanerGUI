using System;
using System.IO;
using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Model;
using SubtitleBytesClearFormatting.Cleaner;
using SubtitleBytesClearFormatting.TagsGenerate;

namespace SubtitleFileCleanerGUI.Service
{
    public class SubtitleFileConverter
    {
        public SubtitleFileConverter() { }

        public async Task ConvertFileAsync(SubtitleFile file)
        {
            if (file.TargetCleaner == SubtitleCleaners.Auto)
                await Task.Run(() => DefineAutoCleaner(file));

            ISubtitleCleanerAsync cleaner = await GetSubtitleCleaner(file.PathLocation, file.TargetCleaner);

            byte[] resultBytes = await cleaner.DeleteFormattingAsync();

            if (file.DeleteTags)
                resultBytes = await DeleteFileTagsAsync(resultBytes, file.TargetCleaner);

            if (file.ToOneLine)
                resultBytes = await FileToOneLineAsync(resultBytes);

            string destination = await Task.Run(() => CreatePathDestination(file));
            await FileManipulator.WriteFileAsync(destination, resultBytes);
        }

        private async Task<ISubtitleCleanerAsync> GetSubtitleCleaner(string fileLocation, SubtitleCleaners subtitleCleaners)
        {
            return subtitleCleaners switch
            {
                SubtitleCleaners.Srt => new SrtCleaner(await FileManipulator.ReadFileAsync(fileLocation)),
                SubtitleCleaners.Ass => new AssCleaner(await FileManipulator.ReadFileAsync(fileLocation)),
                SubtitleCleaners.Vtt => new VttCleaner(await FileManipulator.ReadFileAsync(fileLocation)),
                SubtitleCleaners.Sbv => new SbvCleaner(await FileManipulator.ReadFileAsync(fileLocation)),
                SubtitleCleaners.Sub => new SubCleaner(await FileManipulator.ReadFileAsync(fileLocation)),
                SubtitleCleaners.Auto => throw new Exception(),
                _ => throw new Exception(),
            };
        }

        private void DefineAutoCleaner(SubtitleFile file)
        {
            file.TargetCleaner = Path.GetExtension(file.PathLocation).ToLower() switch
            {
                ".srt" => SubtitleCleaners.Srt,
                ".ass" => SubtitleCleaners.Ass,
                ".vtt" => SubtitleCleaners.Vtt,
                ".sbv" => SubtitleCleaners.Sbv,
                ".sub" => SubtitleCleaners.Sub,
                _ => throw new Exception($"Unable to determine file convertor type."),
            };
        }

        private async Task<byte[]> DeleteFileTagsAsync(byte[] textBytes, SubtitleCleaners cleaner)
        {
            switch (cleaner)
            {
                case SubtitleCleaners.Srt:
                case SubtitleCleaners.Vtt:
                case SubtitleCleaners.Sub:
                    return await TxtCleaner.DeleteTagsAsync(textBytes, TagsCollectionGeneretor.GetBasicTags());
                case SubtitleCleaners.Ass:
                    return await TxtCleaner.DeleteTagsAsync(textBytes, TagsCollectionGeneretor.GetAssSpecificTags());
                case SubtitleCleaners.Sbv:
                    return await TxtCleaner.DeleteTagsAsync(textBytes, TagsCollectionGeneretor.GetSubSpecificTags());
                default:
                    throw new Exception("Unable to determine file tags type.");
            }
        }

        private async Task<byte[]> FileToOneLineAsync(byte[] textBytes)
        {
            return await TxtCleaner.ToOneLineAsync(textBytes);
        }

        private string CreatePathDestination(SubtitleFile file)
        {
            string path = file.PathDestination + "\\" + Path.ChangeExtension(Path.GetFileName(file.PathLocation), ".txt");

            if (File.Exists(path))
                CreateUniquePath(ref path);

            return path;
        }

        private string CreateUniquePath(ref string path)
        {
            string fileDir = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string fileExt = Path.GetExtension(path);

            int i = 1;
            while(File.Exists(path))
            {
                path = fileDir + "\\" + fileName + $" ({i++})" + fileExt;
            }

            return path;
        }
    }
}
