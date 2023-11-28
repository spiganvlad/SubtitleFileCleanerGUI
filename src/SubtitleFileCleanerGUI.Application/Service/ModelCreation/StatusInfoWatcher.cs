using System;
using System.ComponentModel;
using System.Linq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Service.ModelCreation
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
                if (!pathAttributes.Any())
                {
                    throw new InvalidOperationException("The attribute defining the image path " +
                        $"was not found for status type: {status.StatusType}.");
                }
                status.ImagePath = pathAttributes.First().Path;

                var textAttributes = attributeManipulator.GetAttributes<StatusTypes, StatusTextInfoAttribute>(status.StatusType);
                if (!textAttributes.Any())
                {
                    throw new InvalidOperationException("The attribute defining the status info text " +
                        $"was not found for status type: {status.StatusType}.");
                }
                status.TextInfo = textAttributes.First().TextInfo;
            };
        }
    }
}
