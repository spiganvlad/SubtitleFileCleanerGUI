using System;
using CommunityToolkit.Mvvm.Input;
using FluentAssertions;
using SubtitleFileCleanerGUI.Application.Service.Input;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Input
{
    public class TestParameterizedRelayCommandCreator
    {
        [Fact]
        public void CreateTParameter_WithValidExecute_ReturnValidRelayCommand()
        {
            // Arrange
            var executeResult = false;
            Action<bool> execute = (parameter) => { executeResult = parameter; };

            var commandCreator = new ParameterizedRelayCommandCreator();

            // Act
            var result = commandCreator.Create(execute);
            result.Execute(true);

            // Assert
            result.Should().NotBeNull().And.BeOfType<RelayCommand<bool>>();

            executeResult.Should().BeTrue();
        }

        [Fact]
        public void CreateTParameter_WithValidExecuteAndCanExecute_ReturnValidRelayCommand()
        {
            // Arrange
            var executeResult = false;
            Action<bool> execute = (parameter) => { executeResult = parameter; };

            var canExecuteResult = false;
            Predicate<bool> canExecute = (parameter) =>
            {
                canExecuteResult = parameter;
                return parameter;
            };

            var commandCreator = new ParameterizedRelayCommandCreator();

            // Act
            var result = commandCreator.Create(execute, canExecute);
            result.Execute(true);
            result.CanExecute(true);

            // Assert
            result.Should().NotBeNull().And.BeOfType<RelayCommand<bool>>();

            executeResult.Should().BeTrue();
            canExecuteResult.Should().BeTrue();
        }
    }
}
