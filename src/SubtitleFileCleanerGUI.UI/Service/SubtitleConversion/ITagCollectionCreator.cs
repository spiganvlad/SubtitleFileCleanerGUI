using System.Collections.Generic;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface ITagCollectionCreator
    {
        public Dictionary<byte, List<TxtTag>> Create(SubtitleCleaners cleaner);
    }
}
