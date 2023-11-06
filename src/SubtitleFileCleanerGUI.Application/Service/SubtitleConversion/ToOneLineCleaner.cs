using System.Threading.Tasks;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;

namespace SubtitleFileCleanerGUI.Application.Service.SubtitleConversion
{
    public class ToOneLineCleaner : IToOneLineCleaner
    {
        public async Task<byte[]> ToOneLineAsync(byte[] testInBytes)
        {
            return await TxtCleaner.ToOneLineAsync(testInBytes);
        }
    }
}
