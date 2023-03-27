using System.Linq;
using SubtitleFileCleanerGUI.Service;
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

    public class SubtitleStatusFile : SubtitleFile
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

        public SubtitleStatusFile()
        {
            StatusType = StatusTypes.WaitingProcess;
        }

        //Redesign (but how?)
        protected virtual void SetStatusMeta()
        {
            var pathAttributes = new AttributeManipulator().GetAttributes<StatusTypes, SinglePathAttribute>(StatusType);
            ImagePath = pathAttributes.First().Path;

            var textAttributes = new AttributeManipulator().GetAttributes<StatusTypes, StatusTextInfoAttribute>(StatusType);
            TextInfo = textAttributes.First().TextInfo;
        }
    }
}
