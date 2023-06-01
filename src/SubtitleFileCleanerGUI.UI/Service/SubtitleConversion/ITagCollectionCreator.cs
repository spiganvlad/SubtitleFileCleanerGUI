using System.Collections.Generic;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface ITagCollectionCreator
    {
        public Dictionary<byte, List<TxtTag>> Create(SubtitleCleaners cleaner);
    }
}
