using System;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SubtitleFileCleanerGUI.View;

namespace SubtitleFileCleanerGUI.Service.Settings
{
    public class SettingsWindowCreator : ISettingsWindowCreator
    {
        private readonly IHost host;

        public SettingsWindowCreator(IHost host)
        {
            this.host = host;
        }

        public Window Create()
        {
            var window = host.Services.GetService<SettingsWindow>() ??
                throw new InvalidOperationException($"{nameof(SettingsWindow)} service was not found");
            return window;
        }
    }
}
