using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.SubtitleConversion
{
    public class TestSubtitleFileConverter
    {
        private readonly Mock<IAutoCleanerDefiner> autoCleanerDefinerMock;
        private readonly Mock<ISubtitleAsyncCleanerCreator> subtitleCleanerCreatorMock;
        private readonly Mock<IAsyncReaderFactory> asyncReaderFactoryMock;
        private readonly Mock<ITagCleaner> tagCleanerMock;
        private readonly Mock<ITagCollectionCreator> tagCollectionCreatorMock;
        private readonly Mock<IToOneLineCleaner> toOneLineCleanerMock;
        private readonly Mock<IPathGeneratorFactory> pathGeneratorFactoryMock;
        private readonly Mock<IAsyncWriterFactory> asyncWriterFactoryMock;

        private readonly Mock<ISubtitleCleanerAsync> subtitleCleanerAsyncMock;
        private readonly Mock<IPathGenerator> pathGeneratorMock;
        private readonly Mock<IAsyncWriter> asyncWriterMock;
        private readonly Mock<IAsyncReader> asyncReaderMock;

        public TestSubtitleFileConverter()
        {
            autoCleanerDefinerMock = new Mock<IAutoCleanerDefiner>();
            subtitleCleanerCreatorMock = new Mock<ISubtitleAsyncCleanerCreator>();
            asyncReaderFactoryMock = new Mock<IAsyncReaderFactory>();
            tagCleanerMock = new Mock<ITagCleaner>();
            tagCollectionCreatorMock = new Mock<ITagCollectionCreator>();
            toOneLineCleanerMock = new Mock<IToOneLineCleaner>();
            pathGeneratorFactoryMock = new Mock<IPathGeneratorFactory>();
            asyncWriterFactoryMock = new Mock<IAsyncWriterFactory>();

            subtitleCleanerAsyncMock = new Mock<ISubtitleCleanerAsync>();
            pathGeneratorMock = new Mock<IPathGenerator>();
            asyncWriterMock = new Mock<IAsyncWriter>();
            asyncReaderMock = new Mock<IAsyncReader>();
        }

        [Fact]
        public async Task ConvertAsync_WithValidSubtitleFile_WorkValid()
        {
            // Arrange
            var subtitleFile = new SubtitleFile
            {
                PathLocation = @"C:\Test\Location\Path\foo.test",
                PathDestination = @"C:\Test\Destination\Path",
                Cleaner = (SubtitleCleaners)(-1),
                DeleteTags = false,
                ToOneLine = false
            };

            subtitleCleanerCreatorMock.Setup(scc => scc.Create(subtitleFile.Cleaner))
                .Returns(subtitleCleanerAsyncMock.Object);

            asyncReaderFactoryMock.Setup(arf => arf.CreateAsyncReader(ReadWriteType.FileSystem))
                .Returns(asyncReaderMock.Object);

            var subtitleBytes = new byte[] { 0, 1, 2, 3, 4 };
            asyncReaderMock.Setup(ar => ar.ReadAsync(subtitleFile.PathLocation))
                .ReturnsAsync(subtitleBytes);

            var cleanedBytes = new byte[] { 0, 3, 4 };
            subtitleCleanerAsyncMock.Setup(sca => sca.DeleteFormattingAsync(subtitleBytes))
                .ReturnsAsync(cleanedBytes.ToList());

            pathGeneratorFactoryMock.Setup(pgf => pgf.CreatePathGenerator(ReadWriteType.FileSystem))
                .Returns(pathGeneratorMock.Object);

            var uniquePath = @"Unique\Test\Path";
            pathGeneratorMock.Setup(pg => pg.CreateUniquePath(subtitleFile.PathDestination + "\\foo.txt"))
                .Returns(uniquePath);

            asyncWriterFactoryMock.Setup(awf => awf.CreateAsyncWriter(ReadWriteType.FileSystem))
                .Returns(asyncWriterMock.Object);

            byte[] resultBytes = null;
            asyncWriterMock.Setup(aw => aw.WriteAsync(uniquePath, cleanedBytes))
                .Callback((string _, byte[] bytes) => resultBytes = bytes);

            var subtitleFileConverter = new SubtitleFileConverter(autoCleanerDefinerMock.Object, subtitleCleanerCreatorMock.Object, asyncReaderFactoryMock.Object,
                tagCleanerMock.Object, tagCollectionCreatorMock.Object, toOneLineCleanerMock.Object , pathGeneratorFactoryMock.Object, asyncWriterFactoryMock.Object);

            // Act
            await subtitleFileConverter.ConvertAsync(subtitleFile);

            // Assert
            autoCleanerDefinerMock.Verify(acd => acd.Define(It.IsAny<string>()), Times.Never());
            subtitleCleanerCreatorMock.Verify(scc => scc.Create(It.IsAny<SubtitleCleaners>()), Times.Once());
            asyncReaderFactoryMock.Verify(arf => arf.CreateAsyncReader(It.IsAny<ReadWriteType>()), Times.Once());
            asyncReaderMock.Verify(ar => ar.ReadAsync(It.IsAny<string>()), Times.Once());
            subtitleCleanerAsyncMock.Verify(sca => sca.DeleteFormattingAsync(It.IsAny<byte[]>()), Times.Once());
            tagCollectionCreatorMock.Verify(tcc => tcc.Create(It.IsAny<SubtitleCleaners>()), Times.Never());
            tagCleanerMock.Verify(tc => tc.DeleteTagsAsync(It.IsAny<byte[]>(), It.IsAny<Dictionary<byte, List<TxtTag>>>()), Times.Never());
            toOneLineCleanerMock.Verify(tolc => tolc.ToOneLineAsync(It.IsAny<byte[]>()), Times.Never());
            pathGeneratorFactoryMock.Verify(pgf => pgf.CreatePathGenerator(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterFactoryMock.Verify(awf => awf.CreateAsyncWriter(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterMock.Verify(aw => aw.WriteAsync(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once());

            resultBytes.Should().NotBeNull()
                .And.ContainInConsecutiveOrder(cleanedBytes);
        }

        [Fact]
        public async Task ConvertAsync_WithAutoCleanerType_WorkValid()
        {
            // Arrange
            var subtitleFile = new SubtitleFile
            {
                PathLocation = @"C:\Test\Location\Path\foo.test",
                PathDestination = @"C:\Test\Destination\Path",
                Cleaner = SubtitleCleaners.Auto,
                DeleteTags = false,
                ToOneLine = false
            };

            var cleanerEnum = (SubtitleCleaners)(-1);
            autoCleanerDefinerMock.Setup(acd => acd.Define(".test"))
                .Returns(cleanerEnum);

            subtitleCleanerCreatorMock.Setup(scc => scc.Create(cleanerEnum))
                .Returns(subtitleCleanerAsyncMock.Object);

            asyncReaderFactoryMock.Setup(arf => arf.CreateAsyncReader(ReadWriteType.FileSystem))
                .Returns(asyncReaderMock.Object);

            var subtitleBytes = new byte[] { 0, 1, 2, 3, 4 };
            asyncReaderMock.Setup(ar => ar.ReadAsync(subtitleFile.PathLocation))
                .ReturnsAsync(subtitleBytes);

            var cleanedBytes = new byte[] { 0, 3, 4 };
            subtitleCleanerAsyncMock.Setup(sca => sca.DeleteFormattingAsync(subtitleBytes))
                .ReturnsAsync(cleanedBytes.ToList());

            pathGeneratorFactoryMock.Setup(pgf => pgf.CreatePathGenerator(ReadWriteType.FileSystem))
                .Returns(pathGeneratorMock.Object);

            var uniquePath = @"Unique\Test\Path";
            pathGeneratorMock.Setup(pg => pg.CreateUniquePath(subtitleFile.PathDestination + "\\foo.txt"))
                .Returns(uniquePath);

            asyncWriterFactoryMock.Setup(awf => awf.CreateAsyncWriter(ReadWriteType.FileSystem))
                .Returns(asyncWriterMock.Object);
            
            byte[] resultBytes = null;
            asyncWriterMock.Setup(aw => aw.WriteAsync(uniquePath, cleanedBytes))
                .Callback((string _, byte[] bytes) => resultBytes = bytes);

            var subtitleFileConverter = new SubtitleFileConverter(autoCleanerDefinerMock.Object, subtitleCleanerCreatorMock.Object, asyncReaderFactoryMock.Object,
                tagCleanerMock.Object, tagCollectionCreatorMock.Object, toOneLineCleanerMock.Object, pathGeneratorFactoryMock.Object, asyncWriterFactoryMock.Object);

            // Act
            await subtitleFileConverter.ConvertAsync(subtitleFile);

            // Assert
            autoCleanerDefinerMock.Verify(acd => acd.Define(It.IsAny<string>()), Times.Once());
            subtitleCleanerCreatorMock.Verify(scc => scc.Create(It.IsAny<SubtitleCleaners>()), Times.Once());
            asyncReaderFactoryMock.Verify(arf => arf.CreateAsyncReader(It.IsAny<ReadWriteType>()), Times.Once());
            asyncReaderMock.Verify(ar => ar.ReadAsync(It.IsAny<string>()), Times.Once());
            subtitleCleanerAsyncMock.Verify(sca => sca.DeleteFormattingAsync(It.IsAny<byte[]>()), Times.Once());
            tagCollectionCreatorMock.Verify(tcc => tcc.Create(It.IsAny<SubtitleCleaners>()), Times.Never());
            tagCleanerMock.Verify(tc => tc.DeleteTagsAsync(It.IsAny<byte[]>(), It.IsAny<Dictionary<byte, List<TxtTag>>>()), Times.Never());
            toOneLineCleanerMock.Verify(tolc => tolc.ToOneLineAsync(It.IsAny<byte[]>()), Times.Never());
            pathGeneratorFactoryMock.Verify(pgf => pgf.CreatePathGenerator(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterFactoryMock.Verify(awf => awf.CreateAsyncWriter(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterMock.Verify(aw => aw.WriteAsync(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once());

            resultBytes.Should().NotBeNull()
                .And.ContainInConsecutiveOrder(cleanedBytes);

        }

        [Fact]
        public async Task Convert_WithDeleteTagsOption_WorkValid()
        {
            // Arrange
            var subtitleFile = new SubtitleFile
            {
                PathLocation = @"C:\Test\Location\Path\foo.test",
                PathDestination = @"C:\Test\Destination\Path",
                Cleaner = (SubtitleCleaners)(-1),
                DeleteTags = true,
                ToOneLine = false
            };

            subtitleCleanerCreatorMock.Setup(scc => scc.Create(subtitleFile.Cleaner))
                .Returns(subtitleCleanerAsyncMock.Object);

            asyncReaderFactoryMock.Setup(arf => arf.CreateAsyncReader(ReadWriteType.FileSystem))
                .Returns(asyncReaderMock.Object);

            var subtitleBytes = new byte[] { 0, 1, 2, 3, 4 };
            asyncReaderMock.Setup(ar => ar.ReadAsync(subtitleFile.PathLocation))
                .ReturnsAsync(subtitleBytes);

            var cleanedBytes = new byte[] { 0, 3, 4 };
            subtitleCleanerAsyncMock.Setup(sca => sca.DeleteFormattingAsync(subtitleBytes))
                .ReturnsAsync(cleanedBytes.ToList());

            var tagsDictionary = new Dictionary<byte, List<TxtTag>>();
            tagCollectionCreatorMock.Setup(tcc => tcc.Create(subtitleFile.Cleaner))
                .Returns(tagsDictionary);

            var optionCleanedBytes = new byte[] { 0, 3 };
            tagCleanerMock.Setup(tc => tc.DeleteTagsAsync(cleanedBytes, tagsDictionary))
                .ReturnsAsync(optionCleanedBytes);

            pathGeneratorFactoryMock.Setup(pgf => pgf.CreatePathGenerator(ReadWriteType.FileSystem))
                .Returns(pathGeneratorMock.Object);

            var uniquePath = @"Unique\Test\Path";
            pathGeneratorMock.Setup(pg => pg.CreateUniquePath(subtitleFile.PathDestination + "\\foo.txt"))
                .Returns(uniquePath);

            asyncWriterFactoryMock.Setup(awf => awf.CreateAsyncWriter(ReadWriteType.FileSystem))
                .Returns(asyncWriterMock.Object);

            byte[] resultBytes = null;
            asyncWriterMock.Setup(aw => aw.WriteAsync(uniquePath, optionCleanedBytes))
                .Callback((string _, byte[] bytes) => resultBytes = bytes);

            var subtitleFileConverter = new SubtitleFileConverter(autoCleanerDefinerMock.Object, subtitleCleanerCreatorMock.Object, asyncReaderFactoryMock.Object,
                tagCleanerMock.Object, tagCollectionCreatorMock.Object, toOneLineCleanerMock.Object, pathGeneratorFactoryMock.Object, asyncWriterFactoryMock.Object);

            // Act
            await subtitleFileConverter.ConvertAsync(subtitleFile);

            // Assert
            autoCleanerDefinerMock.Verify(acd => acd.Define(It.IsAny<string>()), Times.Never());
            subtitleCleanerCreatorMock.Verify(scc => scc.Create(It.IsAny<SubtitleCleaners>()), Times.Once());
            asyncReaderFactoryMock.Verify(arf => arf.CreateAsyncReader(It.IsAny<ReadWriteType>()), Times.Once());
            asyncReaderMock.Verify(ar => ar.ReadAsync(It.IsAny<string>()), Times.Once());
            subtitleCleanerAsyncMock.Verify(sca => sca.DeleteFormattingAsync(It.IsAny<byte[]>()), Times.Once());
            tagCollectionCreatorMock.Verify(tcc => tcc.Create(It.IsAny<SubtitleCleaners>()), Times.Once());
            tagCleanerMock.Verify(tc => tc.DeleteTagsAsync(It.IsAny<byte[]>(), It.IsAny<Dictionary<byte, List<TxtTag>>>()), Times.Once());
            toOneLineCleanerMock.Verify(tolc => tolc.ToOneLineAsync(It.IsAny<byte[]>()), Times.Never());
            pathGeneratorFactoryMock.Verify(pgf => pgf.CreatePathGenerator(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterFactoryMock.Verify(awf => awf.CreateAsyncWriter(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterMock.Verify(aw => aw.WriteAsync(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once());

            resultBytes.Should().NotBeNull()
                .And.BeSameAs(optionCleanedBytes);

        }

        [Fact]
        public async Task Convert_WithToOneLineOption_WorkValid()
        {
            // Arrange
            var subtitleFile = new SubtitleFile
            {
                PathLocation = @"C:\Test\Location\Path\foo.test",
                PathDestination = @"C:\Test\Destination\Path",
                Cleaner = (SubtitleCleaners)(-1),
                DeleteTags = false,
                ToOneLine = true
            };

            subtitleCleanerCreatorMock.Setup(scc => scc.Create(subtitleFile.Cleaner))
                .Returns(subtitleCleanerAsyncMock.Object);

            asyncReaderFactoryMock.Setup(arf => arf.CreateAsyncReader(ReadWriteType.FileSystem))
                .Returns(asyncReaderMock.Object);

            var subtitleBytes = new byte[] { 0, 1, 2, 3, 4 };
            asyncReaderMock.Setup(ar => ar.ReadAsync(subtitleFile.PathLocation))
                .ReturnsAsync(subtitleBytes);

            var cleanedBytes = new byte[] { 0, 3, 4 };
            subtitleCleanerAsyncMock.Setup(sca => sca.DeleteFormattingAsync(subtitleBytes))
                .ReturnsAsync(cleanedBytes.ToList());

            var optionCleanedBytes = new byte[] { 2, 4, 7 };
            toOneLineCleanerMock.Setup(tolc => tolc.ToOneLineAsync(cleanedBytes))
                .ReturnsAsync(optionCleanedBytes);

            pathGeneratorFactoryMock.Setup(pgf => pgf.CreatePathGenerator(ReadWriteType.FileSystem))
                .Returns(pathGeneratorMock.Object);

            var uniquePath = @"Unique\Test\Path";
            pathGeneratorMock.Setup(pg => pg.CreateUniquePath(subtitleFile.PathDestination + "\\foo.txt"))
                .Returns(uniquePath);

            asyncWriterFactoryMock.Setup(awf => awf.CreateAsyncWriter(ReadWriteType.FileSystem))
                .Returns(asyncWriterMock.Object);

            byte[] resultBytes = null;
            asyncWriterMock.Setup(aw => aw.WriteAsync(uniquePath, optionCleanedBytes))
                .Callback((string _, byte[] bytes) => resultBytes = bytes);

            var subtitleFileConverter = new SubtitleFileConverter(autoCleanerDefinerMock.Object, subtitleCleanerCreatorMock.Object, asyncReaderFactoryMock.Object,
                tagCleanerMock.Object, tagCollectionCreatorMock.Object, toOneLineCleanerMock.Object, pathGeneratorFactoryMock.Object, asyncWriterFactoryMock.Object);

            // Act
            await subtitleFileConverter.ConvertAsync(subtitleFile);

            // Assert
            autoCleanerDefinerMock.Verify(acd => acd.Define(It.IsAny<string>()), Times.Never());
            subtitleCleanerCreatorMock.Verify(scc => scc.Create(It.IsAny<SubtitleCleaners>()), Times.Once());
            asyncReaderFactoryMock.Verify(arf => arf.CreateAsyncReader(It.IsAny<ReadWriteType>()), Times.Once());
            asyncReaderMock.Verify(ar => ar.ReadAsync(It.IsAny<string>()), Times.Once());
            subtitleCleanerAsyncMock.Verify(sca => sca.DeleteFormattingAsync(It.IsAny<byte[]>()), Times.Once());
            tagCollectionCreatorMock.Verify(tcc => tcc.Create(It.IsAny<SubtitleCleaners>()), Times.Never());
            tagCleanerMock.Verify(tc => tc.DeleteTagsAsync(It.IsAny<byte[]>(), It.IsAny<Dictionary<byte, List<TxtTag>>>()), Times.Never());
            toOneLineCleanerMock.Verify(tolc => tolc.ToOneLineAsync(It.IsAny<byte[]>()), Times.Once());
            pathGeneratorFactoryMock.Verify(pgf => pgf.CreatePathGenerator(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterFactoryMock.Verify(awf => awf.CreateAsyncWriter(It.IsAny<ReadWriteType>()), Times.Once());
            asyncWriterMock.Verify(aw => aw.WriteAsync(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once());

            resultBytes.Should().NotBeNull()
                .And.BeSameAs(optionCleanedBytes);
        }
    }
}
