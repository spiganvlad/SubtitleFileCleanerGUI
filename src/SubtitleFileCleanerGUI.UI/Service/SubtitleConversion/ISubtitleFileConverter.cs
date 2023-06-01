using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Service.SubtitleConversion
{
    public interface ISubtitleFileConverter
    {
        public Task ConvertAsync(SubtitleFile file);
    }
}
