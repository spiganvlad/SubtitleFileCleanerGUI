using System.Linq;
using SubtitleFileCleanerGUI.Service;

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

    public class SubtitleStatusFile : SubtitleFile, ICloneableInstance<SubtitleStatusFile>, IViewableStatus
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

        public SubtitleStatusFile() => StatusType = StatusTypes.WaitingProcess;
        public SubtitleStatusFile(StatusTypes statusType) => StatusType = statusType;

        protected virtual void SetStatusMeta()
        {
            var pathAttributes = EnumManipulator<StatusTypes>.GetEnumAttributes<SinglePathAttribute>(StatusType);
            ImagePath = pathAttributes.First().Path;

            var textAttributes = EnumManipulator<StatusTypes>.GetEnumAttributes<StatusTextInfoAttribute>(StatusType);
            TextInfo = textAttributes.First().TextInfo;
        }

        public new SubtitleStatusFile Clone() => CloneTo<SubtitleStatusFile>();
    }
}
