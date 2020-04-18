using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;

namespace IdentityServer.Extensiones
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddProveedores(this ILoggingBuilder builder)
        {
            return builder
                .ClearProviders()
                .AddConsole()
                .AddNLog();
        }
    }
}
