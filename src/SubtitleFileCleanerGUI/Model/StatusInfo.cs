using CommunityToolkit.Mvvm.ComponentModel;
using SubtitleFileCleanerGUI.Attributes;

namespace SubtitleFileCleanerGUI.Model
{
    // Supported status types
    public enum StatusTypes
    {
        [SinglePath("/Images/WaitingProcess.png")]
        [StatusTextInfo("Waiting for a process to start")]
        WaitingProcess,
        [SinglePath("/Images/ConvertingProcess.png")]
        [StatusTextInfo("The conversion has begun")]
        ConvertingProcess,
        [SinglePath("/Images/CompletedProcess.png")]
        [StatusTextInfo("The conversion was successful")]
        CompletedProcess,
        [SinglePath("/Images/FailedProcess.png")]
        [StatusTextInfo("An error has occurred")]
        FailedProcess
    }

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
