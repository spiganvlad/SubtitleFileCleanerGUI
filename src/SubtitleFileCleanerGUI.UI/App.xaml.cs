using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SubtitleFileCleanerGUI.UI.Registers;
using SubtitleFileCleanerGUI.UI.View;
using SubtitleFileCleanerGUI.UI.ViewModel;

namespace SubtitleFileCleanerGUI.UI
{
    public partial class App : System.Windows.Application
    {
        private readonly string jsonConfigPath = "appsettings.json";
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
            services.AddSerilog(jsonConfigPath);
            services.AddInputCommands();
            services.AddReadWrite();
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
            builder.AddJsonFile(jsonConfigPath);
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
