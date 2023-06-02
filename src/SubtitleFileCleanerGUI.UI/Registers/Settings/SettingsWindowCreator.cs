using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.UI.View; //Fix Application to UI dependency

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
            return host.Services.GetRequiredService<SettingsWindow>();
        }
    }
}
