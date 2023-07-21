using System;
using CommunityToolkit.Mvvm.Input;
using FluentAssertions;
using SubtitleFileCleanerGUI.Application.Service.Input;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Input
{
    public class TestParameterlessRelayCommandCreator
    {
        [Fact]
        public void Create_WithValidExecute_ReturnValidRelayCommand()
        {
            // Arrange
            var executeResult = false;
            Action execute = () => { executeResult = true; };

            var commandCreator = new ParameterlessRelayCommandCreator();

            // Act
            var result = commandCreator.Create(execute);
            result.Execute(true);

            // Assert
            result.Should().NotBeNull().And.BeOfType<RelayCommand>();

            executeResult.Should().BeTrue();
        }

        [Fact]
        public void Create_WithValidExecuteAndCanExecute_ReturnValidRelayCommand()
        {
            // Arrange
            var executeResult = false;
            Action execute = () => { executeResult = true; };

            var canExecuteResult = false;
            Func<bool> canExecute = () =>
            {
                canExecuteResult = true;
                return true;
            };

            var commandCreator = new ParameterlessRelayCommandCreator();

            // Act
            var result = commandCreator.Create(execute, canExecute);
            result.Execute(true);
            result.CanExecute(true);

            // Assert
            result.Should().NotBeNull().And.BeOfType<RelayCommand>();

            executeResult.Should().BeTrue();
            canExecuteResult.Should().BeTrue();
        }
    }
}
