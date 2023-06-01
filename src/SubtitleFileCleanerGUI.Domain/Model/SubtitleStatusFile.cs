using CommunityToolkit.Mvvm.ComponentModel;

namespace SubtitleFileCleanerGUI.Domain.Model
{
    public class SubtitleStatusFile : ObservableObject
    {
        private SubtitleFile file;
        private StatusInfo status;

        public SubtitleFile File
        {
            get { return file; }
            set
            {
                file = value;
                OnPropertyChanged(nameof(File));
            }
        }
        public StatusInfo Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }
}
