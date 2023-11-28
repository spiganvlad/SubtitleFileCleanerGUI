using FluentAssertions;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Application.Service.ModelCreation;
using SubtitleFileCleanerGUI.Domain.Model;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.ModelCreation
{
    public class TestSubtitleStatusFileFactory
    {
        private readonly Mock<IDefaultFileManipulator> defaultFileManipulatorMock;
        private readonly Mock<IStatusInfoWatcherFactory> statusInfoWatcherFactoryMock;
        private readonly Mock<IStatusInfoWatcher> watcher;

        public TestSubtitleStatusFileFactory()
        {
            defaultFileManipulatorMock = new Mock<IDefaultFileManipulator>();
            statusInfoWatcherFactoryMock = new Mock<IStatusInfoWatcherFactory>();
            watcher = new Mock<IStatusInfoWatcher>();
        }

        [Fact]
        public void Create_WithValidState_ReturnSubtitleStatusFile()
        {
            // Arrange
            var defaultFileType = (DefaultFileTypes)(-1);

            var file = new SubtitleFile();
            defaultFileManipulatorMock.Setup(dfm => dfm.GetDefaultFile(defaultFileType))
                .Returns(file);

            StatusInfo watchedInfo = null;
            watcher.Setup(w => w.Watch(It.IsAny<StatusInfo>()))
                .Callback((StatusInfo info) => watchedInfo = info);

            statusInfoWatcherFactoryMock.Setup(iw => iw.Create())
                .Returns(watcher.Object);

            var subtitleStatusFileCreator = new SubtitleStatusFileFactory(defaultFileManipulatorMock.Object, statusInfoWatcherFactoryMock.Object);

            // Act
            var result = subtitleStatusFileCreator.CreateWithStatusWatcher(defaultFileType);

            // Assert
            defaultFileManipulatorMock.Verify(dfm => dfm.GetDefaultFile(It.IsAny<DefaultFileTypes>()), Times.Once());
            watcher.Verify(w => w.Watch(It.IsAny<StatusInfo>()), Times.Once());
            statusInfoWatcherFactoryMock.Verify(iw => iw.Create(), Times.Once());

            result.Should().NotBeNull();

            result.File.Should().NotBeNull()
                .And.Be(file);

            result.Status.Should().NotBeNull()
                .And.Be(watchedInfo);
        }
    }
}
