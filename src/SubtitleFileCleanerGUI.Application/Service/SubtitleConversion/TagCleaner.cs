using System.Collections.Generic;
using System.Threading.Tasks;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;

namespace SubtitleFileCleanerGUI.Application.Service.SubtitleConversion
{
    public class TagCleaner : ITagCleaner
    {
        public async Task<byte[]> DeleteTagsAsync(byte[] textInBytes, Dictionary<byte, List<TxtTag>> tagsDictionary)
        {
            return await TxtCleaner.DeleteTagsAsync(textInBytes, tagsDictionary);
        }
    }
}
