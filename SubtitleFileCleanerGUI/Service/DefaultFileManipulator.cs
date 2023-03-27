using System.Linq;
using Microsoft.Extensions.Configuration;
using SubtitleFileCleanerGUI.Model;
using SubtitleFileCleanerGUI.Attributes;

namespace SubtitleFileCleanerGUI.Service
{
    public class DefaultFilesManipulator : IDefaultFileManipulator
    {
        private readonly IConfiguration configuration;
        private readonly IAttributeManipulator attributeManipulator;

        public DefaultFilesManipulator(IConfiguration configuration, IAttributeManipulator attributeManipulator)
        {
            this.configuration = configuration;
            this.attributeManipulator = attributeManipulator;
        }

        public T GetDefaultFile<T>(DefaultFileTypes fileType) where T: SubtitleFile, new()
        {
            var attributes = attributeManipulator.GetAttributes<DefaultFileTypes, SinglePathAttribute>(fileType);
            var path = attributes.First().Path;

            var destinationPath = configuration.GetValue<string>($"{path}:PathDestination");
            var cleaner = configuration.GetValue<SubtitleCleaners>($"{path}:SubtitleConverter");
            var deleteTags = configuration.GetValue<bool>($"{path}:DeleteTagsOption");
            var toOneLine = configuration.GetValue<bool>($"{path}:ToOneLineOption");

            return new T { PathDestination = destinationPath, Cleaner = cleaner, DeleteTags = deleteTags, ToOneLine = toOneLine };
        }

        public void SetDefaultFile(SubtitleFile file, DefaultFileTypes fileTypes)
        {
            var attributes = attributeManipulator.GetAttributes<DefaultFileTypes, SinglePathAttribute>(fileTypes);
            var path = attributes.First().Path;

            configuration[$"{path}:PathDestination"] = file.PathDestination;
            configuration[$"{path}:SubtitleConverter"] = file.Cleaner.ToString();
            configuration[$"{path}:DeleteTagsOption"] = file.DeleteTags.ToString();
            configuration[$"{path}:ToOneLineOption"] = file.ToOneLine.ToString();
        }
    }
}
