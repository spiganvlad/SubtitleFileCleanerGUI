using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SubtitleFileCleanerGUI.Model
{
    // Supported subtitle cleaners enum
    public enum SubtitleCleaners
    {
        Auto,
        Srt,
        Ass,
        Vtt,
        Sbv,
        Sub
    }

    public class SubtitleFile : INotifyPropertyChanged
    {
        private string pathLocation;
        private string pathDestination;
        private SubtitleCleaners targetCleaner;
        private bool deleteTags;
        private bool toOneLine;

        public event PropertyChangedEventHandler PropertyChanged;

        public string PathLocation
        {
            get => pathLocation;
            set
            {
                pathLocation = value;
                OnPropertyChanged("PathLocation");
            }
        }

        public string PathDestination
        {
            get => pathDestination;
            set
            {
                pathDestination = value;
                OnPropertyChanged("PathDestination");
            }
        }

        public SubtitleCleaners TargetCleaner
        {
            get => targetCleaner;
            set
            {
                targetCleaner = value;
                OnPropertyChanged("TargetCleaner");
            }
        }

        public bool DeleteTags
        {
            get => deleteTags;
            set
            {
                deleteTags = value;
                OnPropertyChanged("DeleteTags");
            }
        }

        public bool ToOneLine
        {
            get => toOneLine;
            set
            {
                toOneLine = value;
                OnPropertyChanged("ToOneLine");
            }
        }

        public SubtitleFile() { }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
