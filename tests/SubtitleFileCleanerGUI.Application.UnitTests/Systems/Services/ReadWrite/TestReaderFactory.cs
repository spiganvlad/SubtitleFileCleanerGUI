using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;
using SubtitleFileCleanerGUI.Application.Service.ReadWrite;
using SubtitleFileCleanerGUI.Application.Service.ReadWrite.FileSystem;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.ReadWrite
{
    public class TestReaderFactory
    {
        private readonly Mock<IHost> hostMock;

        public TestReaderFactory()
        {
            hostMock = new Mock<IHost>();
        }

        [Fact]
        public void CreateAsyncReader_WithFileSystemType_ReturnFileSystemReader()
        {
            // Arrange
            var type = ReadWriteType.FileSystem;

            var mockReader = new Mock<IAsyncReader>();
            var reader = new FileSystemAsyncReader();

            hostMock.Setup(h => h.Services.GetService(typeof(IEnumerable<IAsyncReader>)))
                .Returns(new[] { mockReader.Object, reader });

            var readerFactory = new ReaderFactory(hostMock.Object);

            // Act
            var result = readerFactory.CreateAsyncReader(type);

            // Assert
            hostMock.Verify(h => h.Services.GetService(typeof(IEnumerable<IAsyncReader>)), Times.Once());

            result.Should().NotBeNull()
                .And.BeOfType<FileSystemAsyncReader>()
                .And.Be(reader);
        }

        [Fact]
        public void CreateAsyncReader_WithNotImplementedType_ThrowNotImplementedException()
        {
            // Arrange
            var type = (ReadWriteType)(-1);

            var readerFactory = new ReaderFactory(hostMock.Object);

            // Act
            Action act = () => readerFactory.CreateAsyncReader(type);

            // Assert
            act.Should().Throw<NotImplementedException>()
                .WithMessage("Creating a reader is not implemented for type: -1.");
        }
    }
}
