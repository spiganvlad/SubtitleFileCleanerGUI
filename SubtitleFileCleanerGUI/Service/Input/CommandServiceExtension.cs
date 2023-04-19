using Microsoft.Extensions.DependencyInjection;

namespace SubtitleFileCleanerGUI.Service.Input
{
    public static class CommandServiceExtension
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services
                .AddSingleton<IParameterizedCommandCreator, ParameterizedRelayCommandCreator>()
                .AddSingleton<IParameterlessCommandCreator, ParameterlessRelayCommandCreator>();
        }
    }
}
