using Microsoft.Extensions.DependencyInjection;

namespace SubtitleFileCleanerGUI.Service.Input
{
    public static class CommandServiceExtension
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services
                .AddSingleton<IGenericCommandCreator, GenericRelayCommandCreator>()
                .AddSingleton<ICommandCreator, RelayCommandCreator>();
        }
    }
}
