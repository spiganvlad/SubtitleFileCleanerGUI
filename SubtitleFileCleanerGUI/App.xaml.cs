using System;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubtitleFileCleanerGUI.View;
using SubtitleFileCleanerGUI.Service;
using SubtitleFileCleanerGUI.ViewModel;

namespace SubtitleFileCleanerGUI
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUniquePathCreator, UniquePathCreator>();
            services.AddSingleton<IFileManipulator, FileManipulator>();
            services.AddSingleton<IEnumManipulator, EnumManipulator>();
            services.AddSingleton<IAttributeManipulator, AttributeManipulator>();
            services.AddSingleton<IAutoCleanerDefiner, AutoCleanerDefiner>();
            services.AddSingleton<ISubtitleCleanerCreator, SubtitleCleanerCreator>();
            services.AddSingleton<ITagCollectionCreator, TagCollectionCreator>();
            services.AddSingleton<ISubtitleFileConverter, SubtitleFileConverter>();
            services.AddSingleton<MainVM>();
            services.AddSingleton<MainWindow>();
        }

        private void ConfigureAppConfiguration(IConfigurationBuilder builder)
        {
            //Implement configuration
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            using var serviceScope = _host.Services.CreateScope();
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
