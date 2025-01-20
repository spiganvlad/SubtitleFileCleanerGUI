using System.Collections.Generic;
using System.Threading.Tasks;
using SubtitleBytesClearFormatting.TagsGenerate;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion
{
    public interface ITagCleaner
    {
        public Task<byte[]> DeleteTagsAsync(byte[] textInBytes, Dictionary<byte, List<TxtTag>> tagsDictionary);
    }
}
