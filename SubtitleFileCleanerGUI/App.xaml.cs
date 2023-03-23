using System;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubtitleFileCleanerGUI.View;

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
