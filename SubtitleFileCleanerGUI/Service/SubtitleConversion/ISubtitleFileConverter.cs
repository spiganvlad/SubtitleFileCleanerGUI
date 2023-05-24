using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface ISubtitleFileConverter
    {
        public Task ConvertAsync(SubtitleFile file);
    }
}
