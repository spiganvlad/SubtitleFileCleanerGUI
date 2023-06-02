using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Service.ModelCreation
{
    public class SubtitleStatusFileCreator : ISubtitleStatusFileCreator
    {
        private readonly IHost host;
        private readonly IDefaultFileManipulator defaultFileManipulator;
        
        public SubtitleStatusFileCreator(IHost host, IDefaultFileManipulator defaultFileManipulator)
        {
            this.host = host;
            this.defaultFileManipulator = defaultFileManipulator;
        }

        public SubtitleStatusFile Create()
        {
            var subtitleFile = defaultFileManipulator.GetDefaultFile(DefaultFileTypes.Custom);
            var statusInfo = new StatusInfo();

            var watcher = host.Services.GetRequiredService<IStatusInfoWatcher>();
            watcher.Watch(statusInfo);

            statusInfo.StatusType = StatusTypes.WaitingProcess;

            return new SubtitleStatusFile { File = subtitleFile, Status = statusInfo };
        }
    }
}
