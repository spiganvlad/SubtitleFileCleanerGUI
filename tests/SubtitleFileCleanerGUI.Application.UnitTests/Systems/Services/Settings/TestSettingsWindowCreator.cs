using System;
using System.Windows;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using SubtitleFileCleanerGUI.Application.Service.Settings;
using SubtitleFileCleanerGUI.Application.Service.Settings.Options;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Settings
{
    public class TestSettingsWindowCreator
    {
        private readonly Mock<IOptions<SettingsWindowCreatorOptions>> settingsWindowOptionsMock;
        private readonly Mock<IHost> hostMock;

        public TestSettingsWindowCreator()
        {
            settingsWindowOptionsMock = new Mock<IOptions<SettingsWindowCreatorOptions>>();
            hostMock = new Mock<IHost>();
        }

        [StaFact]
        public void Create_WithWindowType_ReturnValidWindow()
        {
            // Arrange
            var window = new Window();
            var windowType = window.GetType();

            var settingsWindowOptions = new SettingsWindowCreatorOptions { SettingWindowType = windowType };
            settingsWindowOptionsMock.SetupGet(o => o.Value).Returns(settingsWindowOptions);

            hostMock.Setup(h => h.Services.GetService(windowType)).Returns(window);

            var settingsWindowCreator = new SettingsWindowCreator(settingsWindowOptionsMock.Object, hostMock.Object);

            // Act
            var result = settingsWindowCreator.Create();

            // Assert
            settingsWindowOptionsMock.VerifyGet(o => o.Value, Times.Once());
            hostMock.Verify(h => h.Services.GetService(It.IsAny<Type>()), Times.Once());

            result.Should().NotBeNull().And.Be(window);
        }

        [Fact]
        public void Create_WithStringType_ThrowInvalidOperationException()
        {
            // Arrange
            var notWindow = "This is not a window";
            var notWindowType = notWindow.GetType();

            var settingsWindowOptions = new SettingsWindowCreatorOptions { SettingWindowType = notWindowType };
            settingsWindowOptionsMock.SetupGet(o => o.Value).Returns(settingsWindowOptions);

            hostMock.Setup(h => h.Services.GetService(notWindowType)).Returns(notWindow);

            var settingsWindowCreator = new SettingsWindowCreator(settingsWindowOptionsMock.Object, hostMock.Object);

            // Act
            Action act = () => settingsWindowCreator.Create();

            // Assert
            settingsWindowOptionsMock.VerifyGet(o => o.Value, Times.Once());

            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"Service '{notWindow}' received from " +
                $"'{settingsWindowOptions.SettingWindowType}' must be inherited from System.Window");
        }

        [Fact]
        public void Create_WithNull_ThrowInvalidOperationException()
        {
            // Arrange
            var settingsWindowOptions = new SettingsWindowCreatorOptions { SettingWindowType = null };
            settingsWindowOptionsMock.SetupGet(o => o.Value).Returns(settingsWindowOptions);

            var settingWindowCreator = new SettingsWindowCreator(settingsWindowOptionsMock.Object, hostMock.Object);

            // Act
            Action act = () => settingWindowCreator.Create();

            // Assert
            settingsWindowOptionsMock.VerifyGet(o => o.Value, Times.Once());

            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"{nameof(settingsWindowOptions.SettingWindowType)} cannot be null");
        }

        [StaFact]
        public void Create_WithNotRegisteredType_ThrowInvalidOperationException()
        {
            // Arrange
            var window = new Window();
            var windowType = window.GetType();

            var settingsWindowOptions = new SettingsWindowCreatorOptions { SettingWindowType = windowType };
            settingsWindowOptionsMock.SetupGet(o => o.Value).Returns(settingsWindowOptions);

            hostMock.Setup(h => h.Services.GetService(windowType));

            var settingsWindowCreator = new SettingsWindowCreator(settingsWindowOptionsMock.Object, hostMock.Object);

            // Act
            Action act = () => settingsWindowCreator.Create();

            // Assert
            settingsWindowOptionsMock.VerifyGet(o => o.Value, Times.Once());

            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"No service for type '{windowType}' has been registered.");
        }
    }
}
