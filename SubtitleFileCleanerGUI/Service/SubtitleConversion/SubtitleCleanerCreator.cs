using System.Linq;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Attributes;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service.Utility;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
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
