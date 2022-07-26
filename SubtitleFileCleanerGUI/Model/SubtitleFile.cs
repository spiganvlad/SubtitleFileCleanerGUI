namespace SubtitleFileCleanerGUI.Model
{
    public class SubtitleFile : NotifyPropertyChangedObject, ICloneableInstance<SubtitleFile>, ILocatable, IDislocatable, ICleanable,
        IDeformatable, IMinifiable
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
        public SubtitleCleaners Cleaner
        {
            get => cleaner;
            set
            {
                cleaner = value;
                OnPropertyChanged("Cleaner");
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

        public SubtitleFile Clone() => CloneTo<SubtitleFile>();

        // The method allows you to clone an object to a derived base T
        protected virtual T CloneTo<T>() where T : SubtitleFile, new() => new()
        {
            PathLocation = PathLocation,
            PathDestination = PathDestination,
            Cleaner = Cleaner,
            DeleteTags = DeleteTags,
            ToOneLine = ToOneLine
        };
    }
}
