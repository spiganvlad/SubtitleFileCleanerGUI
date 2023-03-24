using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service
{
    public interface ISubtitleFileConverter
    {
        public Task ConvertAsync(SubtitleFile file);
    }
}
