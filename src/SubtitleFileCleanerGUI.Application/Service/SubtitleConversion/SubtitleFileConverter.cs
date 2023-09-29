using System.IO;
using System.Threading.Tasks;
using SubtitleBytesClearFormatting.Cleaners;
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
        private readonly ISubtitleCleanerCreator cleanerCreator;
        private readonly ITagCollectionCreator tagsCreator;
        private readonly IAsyncReaderFactory readerFactory;
        private readonly IAsyncWriterFactory writerFactory;
        private readonly IPathGeneratorFactory pathGeneratorFactory;

        public SubtitleFileConverter(IAutoCleanerDefiner autoDefiner, ISubtitleCleanerCreator cleanerCreator,
            ITagCollectionCreator tagsCreator, IAsyncReaderFactory readerFactory, IAsyncWriterFactory writerFactory,
            IPathGeneratorFactory pathGeneratorFactory)
        {
            this.autoDefiner = autoDefiner;
            this.cleanerCreator = cleanerCreator;
            this.tagsCreator = tagsCreator;
            this.readerFactory = readerFactory;
            this.writerFactory = writerFactory;
            this.pathGeneratorFactory = pathGeneratorFactory;
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
                resultBytes = await TxtCleaner.DeleteTagsAsync(resultBytes, tagsCreator.Create(file.Cleaner));

            if (file.ToOneLine)
                resultBytes = await TxtCleaner.ToOneLineAsync(resultBytes);

            var destinationPath = Path.Combine(file.PathDestination, $"{Path.GetFileNameWithoutExtension(file.PathLocation)}.txt");
            var uniquePath = pathGeneratorFactory.CreatePathGenerator(ReadWriteType.FileSystem)
                .CreateUniquePath(destinationPath);

            await writerFactory.CreateAsyncWriter(ReadWriteType.FileSystem)
                .WriteAsync(uniquePath, resultBytes);
        }
    }
}