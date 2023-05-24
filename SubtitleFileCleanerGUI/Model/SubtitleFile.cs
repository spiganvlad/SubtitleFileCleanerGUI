using CommunityToolkit.Mvvm.ComponentModel;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Attributes;

namespace SubtitleFileCleanerGUI.Model
{
    // Supported subtitle cleaners
    public enum SubtitleCleaners
    {
        Auto,
        [SubtitleCleaner(typeof(SrtCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".srt")]
        Srt,
        [SubtitleCleaner(typeof(AssCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetAssSpecificTags))]
        [SubtitleExtension(".ass")]
        Ass,
        [SubtitleCleaner(typeof(VttCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".vtt")]
        Vtt,
        [SubtitleCleaner(typeof(SbvCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".sbv")]
        Sbv,
        [SubtitleCleaner(typeof(SubCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetSubSpecificTags))]
        [SubtitleExtension(".sub")]
        Sub
    }

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
