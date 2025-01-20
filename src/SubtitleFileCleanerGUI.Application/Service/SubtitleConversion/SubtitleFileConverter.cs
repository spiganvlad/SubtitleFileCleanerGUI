using System.IO;
using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Service.SubtitleConversion
{
    public class SubtitleFileConverter : ISubtitleFileConverter
    {
        private readonly IAutoCleanerDefiner autoDefiner;
        private readonly ISubtitleAsyncCleanerCreator cleanerCreator;
        private readonly IAsyncReaderFactory readerFactory;
        private readonly ITagCleaner tagCleaner;
        private readonly ITagCollectionCreator tagsCreator;
        private readonly IToOneLineCleaner toOneLineCleaner;
        private readonly IPathGeneratorFactory pathGeneratorFactory;
        private readonly IAsyncWriterFactory writerFactory;


        public SubtitleFileConverter(IAutoCleanerDefiner autoDefiner, ISubtitleAsyncCleanerCreator cleanerCreator,
            IAsyncReaderFactory readerFactory, ITagCleaner tagCleaner, ITagCollectionCreator tagsCreator,
            IToOneLineCleaner toOneLineCleaner, IPathGeneratorFactory pathGeneratorFactory, IAsyncWriterFactory writerFactory)
        {
            this.autoDefiner = autoDefiner;
            this.cleanerCreator = cleanerCreator;
            this.readerFactory = readerFactory;
            this.tagCleaner = tagCleaner;
            this.tagsCreator = tagsCreator;
            this.toOneLineCleaner = toOneLineCleaner;
            this.pathGeneratorFactory = pathGeneratorFactory;
            this.writerFactory = writerFactory;
        }

        public async Task ConvertAsync(SubtitleFile file)
        {
            if (file.Cleaner == SubtitleCleaners.Auto)
                file.Cleaner = autoDefiner.Define(Path.GetExtension(file.PathLocation).ToLower());

            var cleaner = cleanerCreator.Create(file.Cleaner);

            var subtitleBytes = await readerFactory.CreateAsyncReader(ReadWriteType.FileSystem)
                .ReadAsync(file.PathLocation);

            var resultBytes = (await cleaner.DeleteFormattingAsync(subtitleBytes)).ToArray();

            if (file.DeleteTags)
                resultBytes = await tagCleaner.DeleteTagsAsync(resultBytes, tagsCreator.Create(file.Cleaner));

            if (file.ToOneLine)
                resultBytes = await toOneLineCleaner.ToOneLineAsync(resultBytes);

            var destinationPath = Path.Combine(file.PathDestination, $"{Path.GetFileNameWithoutExtension(file.PathLocation)}.txt");
            var uniquePath = pathGeneratorFactory.CreatePathGenerator(ReadWriteType.FileSystem)
                .CreateUniquePath(destinationPath);

            await writerFactory.CreateAsyncWriter(ReadWriteType.FileSystem)
                .WriteAsync(uniquePath, resultBytes);
        }
    }
}