using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;

namespace SubtitleFileCleanerGUI.Application.Service.ModelCreation
{
    public class StatusInfoWatcherFactory : IStatusInfoWatcherFactory
    {
        private readonly IHost host;

        public StatusInfoWatcherFactory(IHost host)
        {
            this.host = host;
        }

        public IStatusInfoWatcher Create()
        {
            return host.Services.GetRequiredService<IStatusInfoWatcher>();
        }
    }
}
