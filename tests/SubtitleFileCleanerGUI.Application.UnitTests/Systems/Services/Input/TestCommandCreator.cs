using System;
using System.Windows.Input;
using FluentAssertions;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Input;
using SubtitleFileCleanerGUI.Application.Service.Input;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Input
{
    public class TestCommandCreator
    {
        private readonly Mock<IParameterlessCommandCreator> parameterlessCreatorMock;
        private readonly Mock<IParameterizedCommandCreator> parameterizedCreatorMock;

        public TestCommandCreator()
        {
            parameterlessCreatorMock = new Mock<IParameterlessCommandCreator>();
            parameterizedCreatorMock = new Mock<IParameterizedCommandCreator>();
        }

        [Fact]
        public void Create_WithValidExecute_ReturnValidICommand()
        {
            // Arrange
            Action execute = () => { };

            var command = new Mock<ICommand>().Object;
            parameterlessCreatorMock.Setup(pc => pc.Create(execute, null))
                .Returns(command);

            var commandCreator = new CommandCreator(parameterlessCreatorMock.Object, parameterizedCreatorMock.Object);

            // Act
            var result = commandCreator.Create(execute);

            // Assert
            parameterlessCreatorMock.Verify(pc => pc.Create(It.IsAny<Action>(), It.IsAny<Func<bool>>()), Times.Once());

            result.Should().Be(command);
        }

        [Fact]
        public void Create_WithValidExecuteAndCanExecute_ReturnValidICommand()
        {
            // Arrange
            Action execute = () => { };
            Func<bool> canExecute = () => true;

            var command = new Mock<ICommand>().Object;
            parameterlessCreatorMock.Setup(pc => pc.Create(execute, canExecute)).Returns(command);

            var commandCreator = new CommandCreator(parameterlessCreatorMock.Object, parameterizedCreatorMock.Object);

            // Act
            var result = commandCreator.Create(execute, canExecute);

            // Assert
            parameterlessCreatorMock.Verify(pc => pc.Create(It.IsAny<Action>(), It.IsAny<Func<bool>>()), Times.Once());

            result.Should().Be(command);
        }

        [Fact]
        public void CreateTParameter_WithValidExecute_ReturnValidICommand()
        {
            // Arrange
            Action<bool> execute = (parameter) => { };

            var command = new Mock<ICommand>().Object;
            parameterizedCreatorMock.Setup(pc => pc.Create(execute, null))
                .Returns(command);

            var commandCreator = new CommandCreator(parameterlessCreatorMock.Object, parameterizedCreatorMock.Object);

            // Act
            var result = commandCreator.Create(execute);

            // Assert
            parameterizedCreatorMock.Verify(pc => pc.Create(It.IsAny<Action<bool>>(), It.IsAny<Predicate<bool>>()), Times.Once());

            result.Should().Be(command);
        }

        [Fact]
        public void CreateTParameter_WithValidExecuteAndCanExecute_ReturnValidICommand()
        {
            // Arrange
            Action<bool> execute = (parameter) => { };
            Predicate<bool> canExecute = (parameter) => true;

            var command = new Mock<ICommand>().Object;
            parameterizedCreatorMock.Setup(pc => pc.Create(execute, canExecute)).Returns(command);

            var commandCreator = new CommandCreator(parameterlessCreatorMock.Object, parameterizedCreatorMock.Object);

            // Act
            var result = commandCreator.Create(execute, canExecute);

            // Assert
            parameterizedCreatorMock.Verify(pc => pc.Create(It.IsAny<Action<bool>>(), It.IsAny<Predicate<bool>>()), Times.Once());

            result.Should().Be(command);
        }
    }
}
