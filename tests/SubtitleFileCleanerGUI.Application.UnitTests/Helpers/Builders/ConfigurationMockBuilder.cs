using Microsoft.Extensions.Configuration;
using Moq;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Helpers.Builders
{
    public class ConfigurationMockBuilder
    {
        private readonly Mock<IConfiguration> configurationMock;

        public ConfigurationMockBuilder()
        {
            configurationMock = new Mock<IConfiguration>();
        }

        public Mock<IConfiguration> Build()
        {
            return configurationMock;
        }

        public ConfigurationMockBuilder SetupSection(string key, string value, out Mock<IConfigurationSection> configurationSection)
        {
            configurationSection = new Mock<IConfigurationSection>();
            configurationSection.SetupGet(cs => cs.Value).Returns(value);

            configurationMock.Setup(c => c.GetSection(key)).Returns(configurationSection.Object);

            return this;
        }
    }
}
