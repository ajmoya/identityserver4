using System;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Extensiones
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool EsDesarrollo(this IHostEnvironment env)
        {
            if (env == null)
                throw new ArgumentNullException(nameof(env));
            return env.IsDevelopment() || env.IsEnvironment("dev");
        }

        public static bool EsPre(this IHostEnvironment env)
        {
            if (env == null)
                throw new ArgumentNullException(nameof(env));
            return !env.IsDevelopment() && !env.IsProduction() && env.IsEnvironment("pre");
        }

        public static bool EsPro(this IHostEnvironment env)
        {
            if (env == null)
                throw new ArgumentNullException(nameof(env));
            return env.IsProduction() || env.IsEnvironment("pro");
        }

        public static string ObtenerNombreEntornoActual(this IHostEnvironment env) =>
            env.EsDesarrollo() ? "dev" : env.EsPre() ? "pre" : "pro";
    }
}
