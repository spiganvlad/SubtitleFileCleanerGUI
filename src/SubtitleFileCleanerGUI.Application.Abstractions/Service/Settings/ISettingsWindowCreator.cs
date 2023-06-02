using System.Windows;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings
{
    public interface ISettingsWindowCreator
    {
        public Window Create();
    }
}