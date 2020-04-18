using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using IdentityServer.Extensiones;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
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