using System;
using System.Windows.Input;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Input;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Model;
using SubtitleFileCleanerGUI.UI.ViewModel;
using Xunit;

namespace SubtitleFileCleanerGUI.UI.UnitTests.Systems.ViewModel
{
    public class TestSettingsVM
    {
        private readonly Mock<ILogger<SettingsVM>> loggerMock;
        private readonly Mock<IDefaultFileManipulator> defaultFilesManipulatorMock;
        private readonly Mock<IEnumManipulator> enumManipulatorMock;
        private readonly Mock<ICommandCreator> commandCreatorMock;

        public TestSettingsVM()
        {
            loggerMock = new();
            defaultFilesManipulatorMock = new();
            enumManipulatorMock = new();
            commandCreatorMock = new();
        }

        [Fact]
        public void RestoreSettingsCommand_WithValidDefaultFileManipulator_SetDefaultFile()
        {
            // Arrange
            Action actionToExecute = null;

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.IsAny<object>()))
                .Callback((object _) => actionToExecute.Invoke());

            var methodName = "RestoreSettings";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action>(x => x.Method.Name == methodName),
                    It.IsAny<Func<bool>>()))
                .Returns((Action action, Func<bool> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            var subtitleFile = new SubtitleFile();
            defaultFilesManipulatorMock.Setup(x => x.GetDefaultFile(DefaultFileTypes.Default))
                .Returns(subtitleFile);

            var settingsVM = new SettingsVM(
                loggerMock.Object,
                defaultFilesManipulatorMock.Object,
                enumManipulatorMock.Object,
                commandCreatorMock.Object);

            // Act
            settingsVM.RestoreSettingsCommand.Execute(null);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action>(x => x.Method.Name == methodName),
                    It.IsAny<Func<bool>>()),
                Times.Once());

            defaultFilesManipulatorMock.Verify(
                x => x.GetDefaultFile(DefaultFileTypes.Default),
                Times.Once());

            settingsVM.DefaultFile.Should().BeSameAs(subtitleFile);
        }
    }
}
