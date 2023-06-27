using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation
{
    public interface IStatusInfoWatcher
    {
        public void Watch(StatusInfo status);
    }
}
