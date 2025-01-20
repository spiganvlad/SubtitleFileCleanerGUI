using System;
using System.Windows.Input;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Input;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;
using SubtitleFileCleanerGUI.UI.ViewModel;
using Xunit;

namespace SubtitleFileCleanerGUI.UI.UnitTests.Systems.ViewModel
{
    public class TestMainVM
    {
        private readonly Mock<ILogger<MainVM>> loggerMock;
        private readonly Mock<ISubtitleFileConverter> subtitleFileConverterMock;
        private readonly Mock<IEnumManipulator> enumManipulatorMock;
        private readonly Mock<ISubtitleStatusFileFactory> subtitleStatusFileFactoryMock;
        private readonly Mock<ISettingsWindowCreator> settingsWindowCreatorMock;
        private readonly Mock<ICommandCreator> commandCreatorMock;
        private readonly Mock<IDialogOpener> dialogOpenerMock;

        public TestMainVM()
        {
            loggerMock = new();
            subtitleFileConverterMock = new();
            enumManipulatorMock = new();
            subtitleStatusFileFactoryMock = new();
            settingsWindowCreatorMock = new();
            commandCreatorMock = new();
            dialogOpenerMock = new();
        }

        [Fact]
        public void AddFileCommand_WithValidState_AddValidSubtitleStatusFile()
        {
            // Arrange
            var subtitleStatusFile = new SubtitleStatusFile
            {
                Status = new()
            };
            Action actionToExecute = null;
            
            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.IsAny<object>()))
                .Callback((object _) => actionToExecute.Invoke());

            var methodName = "AddFile";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action>(x => x.Method.Name == methodName),
                    It.IsAny<Func<bool>>()))
                .Returns((Action action, Func<bool> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            subtitleStatusFileFactoryMock.Setup(
                x => x.CreateWithStatusWatcher(
                    DefaultFileTypes.Custom))
                .Returns(subtitleStatusFile);

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            // Act
            mainVM.AddFileCommand.Execute(null);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action>(x => x.Method.Name == methodName),
                    It.IsAny<Func<bool>>()),
                Times.Once());

            subtitleStatusFileFactoryMock.Verify(
                x => x.CreateWithStatusWatcher(It.IsAny<DefaultFileTypes>()),
                Times.Once());

            var file = mainVM.Files.Should().ContainSingle()
                .Which; 

            file.Should().BeSameAs(subtitleStatusFile);
            file.Status.StatusType.Should().Be(StatusTypes.WaitingProcess);
        }

        [Fact]
        public void RemoveFileCommand_WithValidState_RemoveSubtitleStatusFile()
        {
            // Arrange
            var subtitleStatusFile = new SubtitleStatusFile();
            Action<SubtitleStatusFile> actionToExecute = null;

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.Is<object>(x => x == subtitleStatusFile)))
                .Callback((object file) => actionToExecute.Invoke((SubtitleStatusFile)file));

            var methodName = "RemoveFile";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action<SubtitleStatusFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleStatusFile>>()))
                .Returns((Action<SubtitleStatusFile> action, Predicate<SubtitleStatusFile> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            mainVM.Files.Add(subtitleStatusFile);

            // Act
            mainVM.RemoveFileCommand.Execute(subtitleStatusFile);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action<SubtitleStatusFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleStatusFile>>()),
                Times.Once());

            mainVM.Files.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAllFileCommand_WithValidState_RemoveAllSubtitleStatusFiles()
        {
            // Arrange
            Action actionToExecute = null;

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.IsAny<object>()))
                .Callback((object _) => actionToExecute.Invoke());

            var methodName = "RemoveAllFile";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action>(x => x.Method.Name == methodName),
                    It.IsAny<Func<bool>>()))
                .Returns((Action action, Func<bool> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            mainVM.Files.Add(new SubtitleStatusFile());
            mainVM.Files.Add(new SubtitleStatusFile());
            mainVM.Files.Add(new SubtitleStatusFile());

            // Act
            mainVM.RemoveAllFileCommand.Execute(null);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action>(x => x.Method.Name == methodName),
                    It.IsAny<Func<bool>>()),
                Times.Once());

            mainVM.Files.Should().BeEmpty();
        }

        [Fact]
        public void ConvertFileCommand_WithValidSubtitleStatusFile_WorkValid()
        {
            // Arrange
            var subtitleStatusFile = new SubtitleStatusFile
            {
                File = new SubtitleFile
                {
                    PathLocation = "X:\\TestRoot\\TestFolder\\testFile.test"
                },
                Status = new()
            };

            commandCreatorMock.Setup(
                x => x.Create(
                    It.IsAny<Action<SubtitleStatusFile>>(),
                    It.IsAny<Predicate<SubtitleStatusFile>>()))
                .Returns((Action<SubtitleStatusFile> action, Predicate<SubtitleStatusFile> _) =>
                {
                    var command = new Mock<ICommand>();
                    command.Setup(
                        x => x.Execute(
                            It.Is<object>(x => x == subtitleStatusFile)))
                        .Callback((object x) => action.Invoke((SubtitleStatusFile)x));

                    return command.Object;
                });

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            // Act
            mainVM.ConvertFileCommand.Execute(subtitleStatusFile);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(
                        l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>(
                        (value, _) => value.ToString() == "Start converting \"testFile.test\" file."),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());

            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(
                        l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>(
                        (value, _) => value.ToString() == "\"testFile.test\" file conversion completed successfully."),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());

            loggerMock.Verify(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Exactly(2));

            subtitleFileConverterMock.Verify(
                x => x.ConvertAsync(
                    It.IsAny<SubtitleFile>()),
                Times.Once());

            subtitleStatusFile.Status.StatusType.Should().Be(StatusTypes.CompletedProcess);
        }

        [Fact]
        public void ConvertFileCommand_WithFileConverterException_HandleException()
        {
            // Arrange
            var subtitleStatusFile = new SubtitleStatusFile
            {
                File = new SubtitleFile
                {
                    PathLocation = "X:\\TestRoot\\TestFolder\\testFile.test"
                },
                Status = new()
            };

            commandCreatorMock.Setup(
                x => x.Create(
                    It.IsAny<Action<SubtitleStatusFile>>(),
                    It.IsAny<Predicate<SubtitleStatusFile>>()))
                .Returns((Action<SubtitleStatusFile> action, Predicate<SubtitleStatusFile> _) =>
                {
                    var command = new Mock<ICommand>();
                    command.Setup(
                        x => x.Execute(
                            It.Is<object>(x => x == subtitleStatusFile)))
                        .Callback((object x) => action.Invoke((SubtitleStatusFile)x));

                    return command.Object;
                });

            var exception = new Exception("Unexpected test exception occurred.");

            subtitleFileConverterMock.Setup(
                x => x.ConvertAsync(
                    It.IsAny<SubtitleFile>()))
                .ThrowsAsync(exception);

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            // Act
            mainVM.ConvertFileCommand.Execute(subtitleStatusFile);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(
                        l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>(
                        (value, _) => value.ToString() == "Start converting \"testFile.test\" file."),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());

            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(
                        l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>(
                        (value, _) => value.ToString() == "An error occurred while converting the \"testFile.test\" file."),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());

            loggerMock.Verify(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Exactly(2));

            subtitleFileConverterMock.Verify(
                x => x.ConvertAsync(
                    It.IsAny<SubtitleFile>()),
                Times.Once());

            subtitleStatusFile.Status.StatusType.Should().Be(StatusTypes.FailedProcess);
            subtitleStatusFile.Status.TextInfo.Should().Be("\n" + exception.Message);
        }

        [Fact]
        public void ConvertAllFilesCommand_WithThreeSubtitleStatusFiles_WorkValid()
        {
            // Arrange
            var firstFile = new SubtitleStatusFile
            {
                File = new SubtitleFile
                {
                    PathLocation = "X:\\TestRoot\\TestFolder\\firstTestFile.test"
                },
                Status = new()
            };

            var secondFile = new SubtitleStatusFile
            {
                File = new SubtitleFile
                {
                    PathLocation = "X:\\TestRoot\\TestFolder\\secondTestFile.test"
                },
                Status = new()
            };

            var thirdFile = new SubtitleStatusFile
            {
                File = new SubtitleFile
                {
                    PathLocation = "X:\\TestRoot\\TestFolder\\thirdTestFile.test"
                },
                Status = new()
            };

            commandCreatorMock.Setup(
                x => x.Create(
                    It.IsAny<Action>(),
                    It.IsAny<Func<bool>>()))
                .Returns((Action action, Func<bool> _) =>
                {
                    var command = new Mock<ICommand>();
                    command.Setup(
                        x => x.Execute(
                            It.IsAny<object>()))
                        .Callback((object _) => action.Invoke());

                    return command.Object;
                });

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            mainVM.Files.Add(firstFile);
            mainVM.Files.Add(secondFile);
            mainVM.Files.Add(thirdFile);

            // Act
            mainVM.ConvertAllFilesCommand.Execute(null);

            // Assert
            mainVM.Files.Should().HaveCount(3)
                .And.AllSatisfy(x => x.Status.StatusType.Should().Be(StatusTypes.CompletedProcess));
        }

        [Fact]
        public void GetFileLocationCommand_WithDialogOpenerReturnsTrue_SetPathLocation()
        {
            // Arrange
            var subtitleFile = new SubtitleFile();
            Action<SubtitleFile> actionToExecute = null;

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.Is<object>(x => x == subtitleFile)))
                .Callback((object file) => actionToExecute.Invoke((SubtitleFile)file));

            var methodName = "GetFileLocation";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()))
                .Returns((Action<SubtitleFile> action, Predicate<SubtitleFile> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            string fileLocation = "Test location";
            dialogOpenerMock.Setup(
                x => x.ShowFileDialog(
                    out fileLocation))
                .Returns(true);

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            // Act
            mainVM.GetFileLocationCommand.Execute(subtitleFile);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()),
                Times.Once());

            dialogOpenerMock.Verify(
                x => x.ShowFileDialog(
                    out It.Ref<string>.IsAny),
                Times.Once());

            subtitleFile.PathLocation.Should().Be(fileLocation);
        }

        [Fact]
        public void GetFileLocationCommand_WithDialogOpenerReturnsFalse_NotSetFilePath()
        {
            // Arrange
            var subtitleFile = new SubtitleFile();
            Action<SubtitleFile> actionToExecute = null;

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.Is<object>(x => x == subtitleFile)))
                .Callback((object file) => actionToExecute.Invoke((SubtitleFile)file));

            var methodName = "GetFileLocation";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()))
                .Returns((Action<SubtitleFile> action, Predicate<SubtitleFile> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            string fileLocation = "Test location";
            dialogOpenerMock.Setup(
                x => x.ShowFileDialog(
                    out fileLocation))
                .Returns(false);

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            // Act
            mainVM.GetFileLocationCommand.Execute(subtitleFile);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()),
                Times.Once());

            dialogOpenerMock.Verify(
                x => x.ShowFileDialog(
                    out It.Ref<string>.IsAny),
                Times.Once());

            subtitleFile.PathLocation.Should().BeNull();
        }

        [Fact]
        public void GetFileDestinationCommand_WithDialogOpenerReturnsTrue_SetPathDestination()
        {
            // Arrange
            var subtitleFile = new SubtitleFile();
            Action<SubtitleFile> actionToExecute = null;

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.Is<object>(x => x == subtitleFile)))
                .Callback((object file) => actionToExecute.Invoke((SubtitleFile)file));

            var methodName = "GetFileDestination";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()))
                .Returns((Action<SubtitleFile> action, Predicate<SubtitleFile> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            string fileDestination = "Test destination";
            dialogOpenerMock.Setup(
                x => x.ShowFolderDialog(
                    out fileDestination))
                .Returns(true);

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            // Act
            mainVM.GetFileDestinationCommand.Execute(subtitleFile);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()),
                Times.Once());

            dialogOpenerMock.Verify(
                x => x.ShowFolderDialog(
                    out It.Ref<string>.IsAny),
                Times.Once());

            subtitleFile.PathDestination.Should().Be(fileDestination);
        }

        [Fact]
        public void GetFileDestinationCommand_WithDialogOpenerReturnsFalse_NotSetPathDestination()
        {
            // Arrange
            var subtitleFile = new SubtitleFile();
            Action<SubtitleFile> actionToExecute = null;

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(
                x => x.Execute(
                    It.Is<object>(x => x == subtitleFile)))
                .Callback((object file) => actionToExecute.Invoke((SubtitleFile)file));

            var methodName = "GetFileDestination";
            commandCreatorMock.Setup(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()))
                .Returns((Action<SubtitleFile> action, Predicate<SubtitleFile> _) =>
                {
                    actionToExecute = action;
                    return commandMock.Object;
                });

            string fileDestination = "Test destination";
            dialogOpenerMock.Setup(
                x => x.ShowFolderDialog(
                    out fileDestination))
                .Returns(false);

            var mainVM = new MainVM(
                loggerMock.Object,
                subtitleFileConverterMock.Object,
                enumManipulatorMock.Object,
                subtitleStatusFileFactoryMock.Object,
                settingsWindowCreatorMock.Object,
                commandCreatorMock.Object,
                dialogOpenerMock.Object);

            // Act
            mainVM.GetFileDestinationCommand.Execute(subtitleFile);

            // Assert
            commandMock.Verify(
                x => x.Execute(It.IsAny<object>()),
                Times.Once());

            commandCreatorMock.Verify(
                x => x.Create(
                    It.Is<Action<SubtitleFile>>(x => x.Method.Name == methodName),
                    It.IsAny<Predicate<SubtitleFile>>()),
                Times.Once());

            dialogOpenerMock.Verify(
                x => x.ShowFolderDialog(
                    out It.Ref<string>.IsAny),
                Times.Once());

            subtitleFile.PathDestination.Should().BeNull();
        }
    }
}
