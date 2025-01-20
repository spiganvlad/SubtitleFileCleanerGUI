using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Service.ModelCreation;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.ModelCreation
{
    public class TestStatusInfoWatcherFactory
    {
        private readonly Mock<IHost> host;

        public TestStatusInfoWatcherFactory()
        {
            host = new Mock<IHost>();
        }

        [Fact]
        public void Create_WithValidHost_ReturnStatusInfoWatcher()
        {
            // Arrange
            var watcher = new Mock<IStatusInfoWatcher>().Object;
            host.Setup(h => h.Services.GetService(typeof(IStatusInfoWatcher)))
                .Returns(watcher);

            var statusInfoWatcherFactory = new StatusInfoWatcherFactory(host.Object);

            // Act
            var result = statusInfoWatcherFactory.Create();

            // Assert
            result.Should().NotBeNull()
                .And.Be(watcher);
        }
    }
}
