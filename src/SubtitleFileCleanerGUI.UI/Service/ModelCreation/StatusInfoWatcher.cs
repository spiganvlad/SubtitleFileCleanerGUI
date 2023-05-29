using System.ComponentModel;
using System.Linq;
using SubtitleFileCleanerGUI.Attributes;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service.Utility;

namespace SubtitleFileCleanerGUI.Service.ModelCreation
{
    public class StatusInfoWatcher : IStatusInfoWatcher
    {
        private readonly IAttributeManipulator attributeManipulator;

        public StatusInfoWatcher(IAttributeManipulator attributeManipulator)
        {
            this.attributeManipulator = attributeManipulator;
        }

        public void Watch(StatusInfo status)
        {
            status.PropertyChanged += (object _, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName != nameof(StatusInfo.StatusType))
                    return;

                var pathAttributes = attributeManipulator.GetAttributes<StatusTypes, SinglePathAttribute>(status.StatusType);
                status.ImagePath = pathAttributes.First().Path;

                var textAttributes = attributeManipulator.GetAttributes<StatusTypes, StatusTextInfoAttribute>(status.StatusType);
                status.TextInfo = textAttributes.First().TextInfo;
            };
        }
    }
}
