using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using IdentityServer.Extensiones;
using Microsoft.Extensions.Hosting;
using Unatec.Arquitectura.Comun.Activities.Helpers;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnatecActivityHelpers.SetupDefaultActivityTracking(0);

            try
            {
                Console.Title = "IdentityServer4";
            }
            catch (Exception e) when (e is IOException)
            {
                // Estamos como Srv Windows
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder => builder.AddProveedores())
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}