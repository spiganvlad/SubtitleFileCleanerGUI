using System.Threading.Tasks;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion
{
    public interface IToOneLineCleaner
    {
        public Task<byte[]> ToOneLineAsync(byte[] testInBytes);
    }
}
