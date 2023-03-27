using System.Linq;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Attributes;

namespace SubtitleFileCleanerGUI.Service
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
