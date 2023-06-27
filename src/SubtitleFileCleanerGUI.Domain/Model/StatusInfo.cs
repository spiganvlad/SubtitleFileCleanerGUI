using CommunityToolkit.Mvvm.ComponentModel;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Domain.Model
{
    public class StatusInfo : ObservableObject
    {
        private StatusTypes statusType;
        private string imagePath;
        private string textInfo;

        public StatusTypes StatusType
        {
            get => statusType;
            set
            {
                statusType = value;
                OnPropertyChanged(nameof(StatusType));
            }
        }
        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        public string TextInfo
        {
            get => textInfo;
            set
            {
                textInfo = value;
                OnPropertyChanged(nameof(TextInfo));
            }
        }
    }
}
