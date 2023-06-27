using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

namespace SubtitleFileCleanerGUI.Application.Service.Extensions
{
    public static class LoggerSettingsConfigurationExtensions
    {
        public static LoggerConfiguration AppSettingsJson(this LoggerSettingsConfiguration loggerSettings)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return loggerSettings.Configuration(configuration);
        }
    }
}
