using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SubtitleFileCleanerGUI.Model
{
    public enum SettingsTypes
    {
        [SinglePath("./Settings/customSettings.json")]
        Custom,
        [SinglePath("./Settings/defaultSettings.json")]
        Default
    }

    public class CustomSettings : ICustomSettings, IUpdatebleSettings, INotifyPropertyChanged
    {
        private string pathDestination;
        private SubtitleCleaners cleaner;
        private bool deleteTags;
        private bool toOneLine;

        public string PathDestination
        {
            get { return pathDestination; }
            set
            {
                pathDestination = value;
                OnPropertyChanged("PathDestination");
            }
        }
        public SubtitleCleaners Cleaner
        {
            get { return cleaner; }
            set
            {
                cleaner = value;
                OnPropertyChanged("Cleaner");
            }
        }
        public bool DeleteTags
        {
            get { return deleteTags; }
            set
            {
                deleteTags = value;
                OnPropertyChanged("DeleteTags");
            }
        }
        public bool ToOneLine
        {
            get { return toOneLine; }
            set
            {
                toOneLine = value;
                OnPropertyChanged("ToOneLine");
            }
        }
        public SettingsTypes SettingsType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CustomSettings() { }

        public void UpdateSettings(ICustomSettings newSettings, SettingsTypes targetType)
        {
            if (SettingsType == targetType)
            {
                PathDestination = newSettings.PathDestination;
                Cleaner = newSettings.Cleaner;
                DeleteTags = newSettings.DeleteTags;
                ToOneLine = newSettings.ToOneLine;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
