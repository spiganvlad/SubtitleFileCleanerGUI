using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SubtitleFileCleanerGUI.Service;

namespace SubtitleFileCleanerGUI.Model
{
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

    public class StatusInfo : IViewableStatus, INotifyPropertyChanged
    {
        private StatusTypes statusType;
        private string imagePath;
        private string textInfo;

        public event PropertyChangedEventHandler PropertyChanged;

        public StatusTypes StatusType
        {
            get => statusType;
            set
            {
                statusType = value;
                SetStatusMeta();
            }
        }
        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        public string TextInfo
        {
            get => textInfo;
            set
            {
                textInfo = value;
                OnPropertyChanged("TextInfo");
            }
        }

        public StatusInfo(StatusTypes statusType = StatusTypes.WaitingProcess) => StatusType = statusType;

        private void SetStatusMeta()
        {
            var pathAttributes = EnumAttributeManipulator<StatusTypes>.GetEnumAttributes<SinglePathAttribute>(StatusType);
            ImagePath = pathAttributes.First().Path;

            var textAttributes = EnumAttributeManipulator<StatusTypes>.GetEnumAttributes<StatusTextInfoAttribute>(StatusType);
            TextInfo = textAttributes.First().TextInfo;
        }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
