using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Application.Service.SubtitleConversion
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
            if (!attributes.Any())
                throw new InvalidOperationException($"No subtitle tags collection found for cleaner type: {cleaner}");

            var tagsMethodName = attributes.First().GetTagsMethodName();

            var tagsMethodInfo = typeof(TagsCollectionGeneretor).GetMethod(tagsMethodName) ?? 
                throw new InvalidOperationException($"Method \"{tagsMethodName}\" does not exist on type " +
                    $"\"{typeof(TagsCollectionGeneretor)}\" for cleaner type: {cleaner}.");

            return (Dictionary<byte, List<TxtTag>>)tagsMethodInfo.Invoke(null, null);
        }
    }
}
