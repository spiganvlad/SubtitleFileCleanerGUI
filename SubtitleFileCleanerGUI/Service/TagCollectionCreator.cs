using System.Linq;
using System.Collections.Generic;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Attributes;

namespace SubtitleFileCleanerGUI.Service
{
    public class TagCollectionCreator : ITagCollectionCreator
    {
        private readonly IAttributeManipulator attributeManipulator;

        public TagCollectionCreator(IAttributeManipulator attributeManipulator)
        {
            this.attributeManipulator = attributeManipulator;
        }

        public Dictionary<byte, List<TxtTag>> Create(SubtitleCleaners cleaner)
        {
            var attributes = attributeManipulator.GetAttributes<SubtitleCleaners, SubtitleTagsAttribute>(cleaner);
            return attributes.First().GetSubtitleTagsDictionary();
        }
    }
}
