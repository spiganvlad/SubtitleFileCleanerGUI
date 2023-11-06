using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Dialog;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Input;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Application.Service.Dialog;
using SubtitleFileCleanerGUI.Application.Service.Extensions;
using SubtitleFileCleanerGUI.Application.Service.Input;
using SubtitleFileCleanerGUI.Application.Service.ModelCreation;
using SubtitleFileCleanerGUI.Application.Service.ReadWrite;
using SubtitleFileCleanerGUI.Application.Service.ReadWrite.FileSystem;
using SubtitleFileCleanerGUI.Application.Service.Settings;
using SubtitleFileCleanerGUI.Application.Service.Settings.Options;
using SubtitleFileCleanerGUI.Application.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Service.Utility;
using SubtitleFileCleanerGUI.UI.View;

namespace SubtitleFileCleanerGUI.UI.Registers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettingsJson()
                .CreateLogger();

            return services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(dispose: true);
            });
        }

        public static IServiceCollection AddInputCommands(this IServiceCollection services)
        {
            return services
                .AddTransient<IParameterizedCommandCreator, ParameterizedRelayCommandCreator>()
                .AddTransient<IParameterlessCommandCreator, ParameterlessRelayCommandCreator>()
                .AddTransient<ICommandCreator, CommandCreator>();
        }

        public static IServiceCollection AddReadWrite(this IServiceCollection services)
        {
            return services
                .AddTransient<IAsyncReader, FileSystemAsyncReader>()
                .AddTransient<IAsyncReaderFactory, ReaderFactory>()

                .AddTransient<IAsyncWriter, FileSystemAsyncWriter>()
                .AddTransient<IAsyncWriterFactory, WriterFactory>()

                .AddTransient<IPathGenerator, FileSystemPathGenerator>()
                .AddTransient<IPathGeneratorFactory, PathGeneratorFactory>();
        }

        public static IServiceCollection AddDialogs(this IServiceCollection services)
        {
            return services
                .AddTransient<IOpenFileDialog, OokiiOpenFileDialog>()
                .AddTransient<IOpenFolderDialog, OokiiOpenFolderDialog>()
                .AddTransient<IDialogOpener, DialogOpener>();
        }

        public static IServiceCollection AddUtilities(this IServiceCollection services)
        {
            return services
                .AddTransient<IAttributeManipulator, AttributeManipulator>()
                .AddTransient<IEnumManipulator, EnumManipulator>();
        }

        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            services.Configure<SettingsWindowCreatorOptions>(options =>
                options.SettingWindowType = typeof(SettingsWindow));

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
                .AddTransient<ISubtitleAsyncCleanerCreator, SubtitleAsyncCleanerCreator>()
                .AddTransient<ITagCleaner, TagCleaner>()
                .AddTransient<ITagCollectionCreator, TagCollectionCreator>()
                .AddTransient<IToOneLineCleaner, ToOneLineCleaner>()
                .AddTransient<ISubtitleFileConverter, SubtitleFileConverter>();
        }
    }
}
