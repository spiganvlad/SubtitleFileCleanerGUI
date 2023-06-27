using System.Linq;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Application.Service.SubtitleConversion
{
    public class SubtitleCleanerCreator : ISubtitleCleanerCreator
    {
        private readonly IAttributeManipulator attributeManipulator;

        public SubtitleCleanerCreator(IAttributeManipulator attributeManipulator)
        {
            this.attributeManipulator = attributeManipulator;
        }

        public ISubtitleCleanerAsync Create(SubtitleCleaners cleaner)
        {
            var attributes = attributeManipulator.GetAttributes<SubtitleCleaners, SubtitleCleanerAttribute>(cleaner);
            return attributes.First().GetAsyncCleaner();
        }
    }
}
