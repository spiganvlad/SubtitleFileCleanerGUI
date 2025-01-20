using FluentAssertions;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog;
using SubtitleFileCleanerGUI.Application.Service.Dialog;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Dialog
{
    public class TestDialogOpener
    {
        private readonly Mock<IOpenFileDialog> openFileDialogMock;
        private readonly Mock<IOpenFolderDialog> openFolderDialogMock;

        public TestDialogOpener()
        {
            openFileDialogMock = new Mock<IOpenFileDialog>();
            openFolderDialogMock = new Mock<IOpenFolderDialog>();
        }

        [Fact]
        public void ShowFileDialog_WithValidState_ReturnTrue()
        {
            // Arrange
            string filePath = null;
            openFileDialogMock.Setup(x => x.ShowDialog(out filePath))
                .Returns(true);

            var dialogOpener = new DialogOpener(openFileDialogMock.Object, openFolderDialogMock.Object);

            // Act
            var result = dialogOpener.ShowFileDialog(out filePath);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShowFolderDialog_WithValidState_ReturnTrue()
        {
            // Arrange
            string folderPath = null;
            openFolderDialogMock.Setup(x => x.ShowDialog(out folderPath))
                .Returns(true);

            var dialogOpener = new DialogOpener(openFileDialogMock.Object, openFolderDialogMock.Object);

            // Act
            var result = dialogOpener.ShowFolderDialog(out folderPath);

            // Assert
            result.Should().BeTrue();

        }
    }
}
