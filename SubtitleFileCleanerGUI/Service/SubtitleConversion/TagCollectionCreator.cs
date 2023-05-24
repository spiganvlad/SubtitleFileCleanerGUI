using System.Collections.Generic;
using System.Linq;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Attributes;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Service.Utility;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
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
