using System.ComponentModel;
using System.Runtime.CompilerServices;
using SubtitleBytesClearFormatting.Cleaner;

namespace SubtitleFileCleanerGUI.Model
{
    // Supported subtitle cleaners enum
    public enum SubtitleCleaners
    {
        Auto,
        [SubtitleCleaner(typeof(SrtCleaner))]
        [SubtitleTags("GetBasicTags")]
        [SubtitleExtension(".srt")]
        Srt,
        [SubtitleCleaner(typeof(AssCleaner))]
        [SubtitleTags("GetAssSpecificTags")]
        [SubtitleExtension(".ass")]
        Ass,
        [SubtitleCleaner(typeof(VttCleaner))]
        [SubtitleTags("GetBasicTags")]
        [SubtitleExtension(".vtt")]
        Vtt,
        [SubtitleCleaner(typeof(SbvCleaner))]
        [SubtitleTags("GetBasicTags")]
        [SubtitleExtension(".sbv")]
        Sbv,
        [SubtitleCleaner(typeof(SubCleaner))]
        [SubtitleTags("GetSubSpecificTags")]
        [SubtitleExtension(".sub")]
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
        public SubtitleFile(CustomSettings settings)
        {
            PathDestination = settings.PathDestination;
            TargetCleaner = settings.Cleaner;
            DeleteTags = settings.DeleteTags;
            ToOneLine = settings.ToOneLine;
        }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
