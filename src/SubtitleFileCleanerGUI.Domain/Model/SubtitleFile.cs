using CommunityToolkit.Mvvm.ComponentModel;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Domain.Model
{
    public class SubtitleFile : ObservableObject
    {
        private string pathLocation;
        private string pathDestination;
        private SubtitleCleaners cleaner;
        private bool deleteTags;
        private bool toOneLine;

        public string PathLocation
        {
            get => pathLocation;
            set
            {
                pathLocation = value;
                OnPropertyChanged(nameof(PathLocation));
            }
        }
        public string PathDestination
        {
            get => pathDestination;
            set
            {
                pathDestination = value;
                OnPropertyChanged(nameof(PathDestination));
            }
        }
        public SubtitleCleaners Cleaner
        {
            get => cleaner;
            set
            {
                cleaner = value;
                OnPropertyChanged(nameof(Cleaner));
            }
        }
        public bool DeleteTags
        {
            get => deleteTags;
            set
            {
                deleteTags = value;
                OnPropertyChanged(nameof(DeleteTags));
            }
        }
        public bool ToOneLine
        {
            get => toOneLine;
            set
            {
                toOneLine = value;
                OnPropertyChanged(nameof(ToOneLine));
            }
        }
    }
}
