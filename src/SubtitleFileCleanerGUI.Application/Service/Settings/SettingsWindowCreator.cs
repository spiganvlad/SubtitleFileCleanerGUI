using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Application.Service.Settings.Options;

namespace SubtitleFileCleanerGUI.Application.Service.Settings
{
    public class SettingsWindowCreator : ISettingsWindowCreator
    {
        private readonly SettingsWindowCreatorOptions options;
        private readonly IHost host;

        public SettingsWindowCreator(IOptions<SettingsWindowCreatorOptions> options, IHost host)
        {
            this.options = options.Value;
            this.host = host;
        }

        public Window Create()
        {
            if (options.SettingWindowType == null)
                throw new InvalidOperationException($"{nameof(options.SettingWindowType)} cannot be null");

            var windowObj = host.Services.GetRequiredService(options.SettingWindowType);
            if (windowObj is Window window)
                return window;

            throw new InvalidOperationException($"Service '{windowObj}' received from '{options.SettingWindowType}' " +
                    $"must be inherited from System.Window");
        }
    }
}