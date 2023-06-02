using System.Threading.Tasks;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion
{
    public interface ISubtitleFileConverter
    {
        public Task ConvertAsync(SubtitleFile file);
    }
}
