using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SubtitleFileCleanerGUI.Service;
using SubtitleFileCleanerGUI.View;
using SubtitleFileCleanerGUI.ViewModel;

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
            services.AddSerilog();
            services.AddInputCommands();
            services.AddIO();
            services.AddDialogs();
            services.AddUtilities();
            services.AddSettings();
            services.AddModelCreation();
            services.AddSubtitleConversion();
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
                Log.Information("Application Starting Up");

                var mainWindow = services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unable to start application");
                Log.CloseAndFlush();
            }
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            Log.Information("Application closed successfully");
            Log.CloseAndFlush();
        }
    }
}
