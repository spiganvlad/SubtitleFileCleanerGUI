using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Service.ModelCreation
{
    public class SubtitleStatusFileFactory : ISubtitleStatusFileFactory
    {
        private readonly IDefaultFileManipulator defaultFileManipulator;
        private readonly IStatusInfoWatcherFactory statusInfoWatcherFactory;
        
        public SubtitleStatusFileFactory(IDefaultFileManipulator defaultFileManipulator,
            IStatusInfoWatcherFactory statusInfoWatcherFactory)
        {
            this.defaultFileManipulator = defaultFileManipulator;
            this.statusInfoWatcherFactory = statusInfoWatcherFactory;
        }

        public SubtitleStatusFile CreateWithStatusWatcher(DefaultFileTypes type)
        {
            var subtitleFile = defaultFileManipulator.GetDefaultFile(type);
            var statusInfo = new StatusInfo();

            var watcher = statusInfoWatcherFactory.Create();
            watcher.Watch(statusInfo);

            return new SubtitleStatusFile { File = subtitleFile, Status = statusInfo };
        }
    }
}
