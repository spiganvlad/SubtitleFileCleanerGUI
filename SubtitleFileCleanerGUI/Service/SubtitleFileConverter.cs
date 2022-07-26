using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Model;
using SubtitleBytesClearFormatting.Cleaner;

namespace SubtitleFileCleanerGUI.Service
{
    public static class SubtitleFileConverter
    { 
        public static async Task ConvertFileAsync(SubtitleFile file)
        {
            if (file.Cleaner == SubtitleCleaners.Auto)
                await Task.Run(() => DefineAutoCleaner(file));

            ISubtitleCleanerAsync cleaner = await GetSubtitleCleaner(file.PathLocation, file.Cleaner);

            byte[] resultBytes = await cleaner.DeleteFormattingAsync();

            if (file.DeleteTags)
                resultBytes = await DeleteFileTagsAsync(resultBytes, file.Cleaner);

            if (file.ToOneLine)
                resultBytes = await FileToOneLineAsync(resultBytes);

            string destination = await Task.Run(() => FileManipulator.CreateUniquePath(file.PathLocation, file.PathDestination));
            await FileManipulator.WriteFileAsync(destination, resultBytes);
        }

        private static void DefineAutoCleaner(SubtitleFile file)
        {
            string fileExtension = Path.GetExtension(file.PathLocation).ToLower();
            var cleaners = EnumManipulator<SubtitleCleaners>.GetAllEnumValues();

            foreach (SubtitleCleaners cleaner in cleaners)
            {
                var attributes = EnumManipulator<SubtitleCleaners>.GetEnumAttributes<SubtitleExtensionAttribute>(cleaner);

                if (!attributes.Any())
                    continue;

                foreach (SubtitleExtensionAttribute attribute in attributes)
                {
                    if (attribute.Extension == fileExtension)
                    {
                        file.Cleaner = cleaner;
                        return;
                    }
                }
            }

            throw new InvalidOperationException($"Unable to define converter for {fileExtension} extension in {file.PathLocation} path");
        }

        private static async Task<ISubtitleCleanerAsync> GetSubtitleCleaner(string fileLocation, SubtitleCleaners subtitleCleaners)
        {
            var attributes = EnumManipulator<SubtitleCleaners>.GetEnumAttributes<SubtitleCleanerAttribute>(subtitleCleaners);
            return attributes.First().GetAsyncCleaner(await FileManipulator.ReadFileAsync(fileLocation));
        }

        private static async Task<byte[]> DeleteFileTagsAsync(byte[] textBytes, SubtitleCleaners subtitleCleaners)
        {
            var attributes = EnumManipulator<SubtitleCleaners>.GetEnumAttributes<SubtitleTagsAttribute>(subtitleCleaners);
            return await TxtCleaner.DeleteTagsAsync(textBytes, attributes.First().GetSubtitleTagsDictionary());
        }

        private static async Task<byte[]> FileToOneLineAsync(byte[] textBytes) => await TxtCleaner.ToOneLineAsync(textBytes);
    }
}