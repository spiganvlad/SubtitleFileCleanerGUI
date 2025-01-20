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
    public class TestWriterFactory
    {
        private readonly Mock<IHost> hostMock;

        public TestWriterFactory()
        {
            hostMock = new Mock<IHost>();
        }

        [Fact]
        public void CreateAsyncWriter_WithFileSystemType_ReturnFileSystemWriter()
        {
            // Arrange
            var type = ReadWriteType.FileSystem;

            var mockWriter = new Mock<IAsyncWriter>();
            var writer = new FileSystemAsyncWriter();

            hostMock.Setup(h => h.Services.GetService(typeof(IEnumerable<IAsyncWriter>)))
                .Returns(new[] { mockWriter.Object, writer });

            var writerFactory = new WriterFactory(hostMock.Object);

            // Act
            var result = writerFactory.CreateAsyncWriter(type);

            // Assert
            hostMock.Verify(h => h.Services.GetService(typeof(IEnumerable<IAsyncWriter>)), Times.Once());

            result.Should().NotBeNull()
                .And.BeOfType<FileSystemAsyncWriter>()
                .And.Be(writer);
        }

        [Fact]
        public void CreateAsyncWriter_WithX_ThrowNotImplementedException()
        {
            // Arrange
            var type = (ReadWriteType)(-1);

            var writerFactory = new WriterFactory(hostMock.Object);

            // Act
            Action act = () => writerFactory.CreateAsyncWriter(type);

            // Assert
            act.Should().Throw<NotImplementedException>()
                .WithMessage("Creating a writer is not implemented for type: -1.");
        }
    }
}
