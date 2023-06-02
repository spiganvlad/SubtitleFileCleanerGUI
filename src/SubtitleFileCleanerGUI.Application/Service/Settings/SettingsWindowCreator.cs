//Fix Application to UI dependency

/*using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubtitleFileCleanerGUI.UI.View;

namespace SubtitleFileCleanerGUI.Application.Service.Settings
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
*/