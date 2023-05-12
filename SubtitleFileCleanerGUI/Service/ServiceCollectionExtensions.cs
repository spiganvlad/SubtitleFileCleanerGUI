using Microsoft.Extensions.DependencyInjection;
using SubtitleFileCleanerGUI.Service.Dialog;
using SubtitleFileCleanerGUI.Service.Input;
using SubtitleFileCleanerGUI.Service.IO;
using SubtitleFileCleanerGUI.Service.ModelCreation;
using SubtitleFileCleanerGUI.Service.Settings;
using SubtitleFileCleanerGUI.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Service.Utility;

namespace SubtitleFileCleanerGUI.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInputCommands(this IServiceCollection services)
        {
            return services
                .AddTransient<IParameterizedCommandCreator, ParameterizedRelayCommandCreator>()
                .AddTransient<IParameterlessCommandCreator, ParameterlessRelayCommandCreator>()
                .AddTransient<ICommandCreator, RelayCommandCreator>();
        }

        public static IServiceCollection AddIO(this IServiceCollection services)
        {
            return services
                .AddTransient<IFileManipulator, FileManipulator>()
                .AddTransient<IUniquePathCreator, UniquePathCreator>();
        }

        public static IServiceCollection AddDialogs(this IServiceCollection services)
        {
            return services
                .AddTransient<IOpenFileDialog, OokiiOpenFileDialog>()
                .AddTransient<IOpenFolderDialog, OokiiOpenFolderDialog>();
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

        public static IServiceCollection AddModelCreation(this IServiceCollection services)
        {
            return services
                .AddTransient<IStatusInfoWatcher, StatusInfoWatcher>()
                .AddTransient<ISubtitleStatusFileCreator, SubtitleStatusFileCreator>();
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
