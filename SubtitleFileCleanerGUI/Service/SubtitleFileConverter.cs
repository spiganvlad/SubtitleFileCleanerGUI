using System.IO;
using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Model;
using SubtitleBytesClearFormatting.Cleaners;

namespace SubtitleFileCleanerGUI.Service
{
    public class SubtitleFileConverter : ISubtitleFileConverter
    {
        private readonly IAutoCleanerDefiner autoDefiner;
        private readonly ISubtitleCleanerCreator cleanerCreator;
        private readonly ITagCollectionCreator tagsCreator;
        private readonly IUniquePathCreator uniquePathCreator;
        private readonly IFileManipulator fileManipulator;

        public SubtitleFileConverter(IAutoCleanerDefiner autoDefiner, ISubtitleCleanerCreator cleanerCreator,
            ITagCollectionCreator tagsCreator, IUniquePathCreator uniquePathCreator, IFileManipulator fileManipulator)
        {
            this.autoDefiner = autoDefiner;
            this.cleanerCreator = cleanerCreator;
            this.tagsCreator = tagsCreator;
            this.uniquePathCreator = uniquePathCreator;
            this.fileManipulator = fileManipulator;
        }

        public async Task ConvertAsync(SubtitleFile file)
        {
            if (file.Cleaner == SubtitleCleaners.Auto)
                file.Cleaner = autoDefiner.Define(Path.GetExtension(file.PathLocation).ToLower());

            var cleaner = cleanerCreator.Create(file.Cleaner);

            var subtitleBytes = await fileManipulator.ReadFileAsync(file.PathLocation);
            var resultBytes = (await cleaner.DeleteFormattingAsync(subtitleBytes)).ToArray();

            if (file.DeleteTags)
                resultBytes = await TxtCleaner.DeleteTagsAsync(resultBytes, tagsCreator.Create(file.Cleaner));

            if (file.ToOneLine)
                resultBytes = await TxtCleaner.ToOneLineAsync(resultBytes);

            var destinationPath = Path.Combine(file.PathDestination, $"{Path.GetFileNameWithoutExtension(file.PathLocation)}.txt");
            await fileManipulator.WriteFileAsync(uniquePathCreator.Create(destinationPath), resultBytes);
        }
    }
}