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
    public class TestPathGeneratorFactory
    {
        private readonly Mock<IHost> hostMock;

        public TestPathGeneratorFactory()
        {
            hostMock = new Mock<IHost>();
        }

        [Fact]
        public void CreatePathGenerator_WithFileSystemType_ReturnFileSystemPathGenerator()
        {
            // Arrange
            var type = ReadWriteType.FileSystem;

            var mockPathGenerator = new Mock<IPathGenerator>();
            var pathGenerator = new FileSystemPathGenerator();

            hostMock.Setup(h => h.Services.GetService(typeof(IEnumerable<IPathGenerator>)))
                .Returns(new[] { mockPathGenerator.Object, pathGenerator });

            var pathGeneratorFactory = new PathGeneratorFactory(hostMock.Object);

            // Act
            var result = pathGeneratorFactory.CreatePathGenerator(type);

            // Assert
            hostMock.Verify(h => h.Services.GetService(typeof(IEnumerable<IPathGenerator>)), Times.Once());

            result.Should().NotBeNull()
                .And.BeOfType<FileSystemPathGenerator>()
                .And.Be(pathGenerator);
        }

        [Fact]
        public void CreatePathGenerator_WithNotImplementedType_ThrowNotImplementedException()
        {
            // Arrange
            var type = (ReadWriteType)(-1);

            var pathGeneratorFactory = new PathGeneratorFactory(hostMock.Object);

            // Act
            Action act = () => pathGeneratorFactory.CreatePathGenerator(type);

            // Assert
            act.Should().Throw<NotImplementedException>()
                .WithMessage("Creating a path generator is not implemented for type: -1.");
        }
    }
}
