using System;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubtitleFileCleanerGUI.View;
using SubtitleFileCleanerGUI.Service;
using SubtitleFileCleanerGUI.ViewModel;
using SubtitleFileCleanerGUI.Service.Input;

namespace SubtitleFileCleanerGUI
{
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            host = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddCommands();
            services.AddSingleton<IUniquePathCreator, UniquePathCreator>();
            services.AddSingleton<IFileManipulator, FileManipulator>();
            services.AddSingleton<IEnumManipulator, EnumManipulator>();
            services.AddSingleton<IAttributeManipulator, AttributeManipulator>();
            services.AddSingleton<IDefaultFileManipulator, DefaultFilesManipulator>();
            services.AddSingleton<IAutoCleanerDefiner, AutoCleanerDefiner>();
            services.AddSingleton<ISubtitleCleanerCreator, SubtitleCleanerCreator>();
            services.AddSingleton<ITagCollectionCreator, TagCollectionCreator>();
            services.AddSingleton<ISubtitleFileConverter, SubtitleFileConverter>();
            services.AddSingleton<ISettingsWindowCreator, SettingsWindowCreator>();
            services.AddSingleton<MainVM>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<SettingsVM>();
            services.AddTransient<SettingsWindow>();
        }

        private void ConfigureAppConfiguration(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("appsettings.json");
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            
            try
            {
                var mainWindow = services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                //Implement logging
            }
        }
    }
}
