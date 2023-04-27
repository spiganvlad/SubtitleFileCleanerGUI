using Microsoft.Extensions.DependencyInjection;
using SubtitleFileCleanerGUI.Service.IO;
using SubtitleFileCleanerGUI.Service.Input;
using SubtitleFileCleanerGUI.Service.Utility;
using SubtitleFileCleanerGUI.Service.Settings;
using SubtitleFileCleanerGUI.Service.SubtitleConversion;

namespace SubtitleFileCleanerGUI.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInputCommands(this IServiceCollection services)
        {
            return services
                .AddTransient<IParameterizedCommandCreator, ParameterizedRelayCommandCreator>()
                .AddTransient<IParameterlessCommandCreator, ParameterlessRelayCommandCreator>();
        }

        public static IServiceCollection AddIO(this IServiceCollection services)
        {
            return services
                .AddTransient<IFileManipulator, FileManipulator>()
                .AddTransient<IUniquePathCreator, UniquePathCreator>();
        }

        public static IServiceCollection AddUtilities(this IServiceCollection services)
        {
            return services
                .AddTransient<IAttributeManipulator, AttributeManipulator>()
                .AddTransient<IEnumManipulator, EnumManipulator>();
        }

        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            return services
                .AddTransient<ISettingsWindowCreator, SettingsWindowCreator>()
                .AddTransient<IDefaultFileManipulator, DefaultFilesManipulator>();
        }

        public static IServiceCollection AddSubtitleConversion(this IServiceCollection services)
        {
            return services
                .AddTransient<IAutoCleanerDefiner, AutoCleanerDefiner>()
                .AddTransient<ISubtitleCleanerCreator, SubtitleCleanerCreator>()
                .AddTransient<ITagCollectionCreator, TagCollectionCreator>()
                .AddTransient<ISubtitleFileConverter, SubtitleFileConverter>();
        }
    }
}
