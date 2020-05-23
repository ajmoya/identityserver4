using System;
using System.Linq;
using IdentityServer.Models;
using IdentityServer.Utils;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Data
{
    public static class SeedData
    {
        public static void InicializarBasesDatos(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();


            // Clientes
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
                logger.LogInformation(("Clientes creados en la BD"));
            }

            // IdentityResources
            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.Ids)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
                logger.LogInformation(("IdentityResources creadas en la BD"));
            }

            // ApiResources
            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.Apis)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
                logger.LogInformation(("ApiResources creadas en la BD"));
            }

            // Usuarios
            var appDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            appDbContext.Database.Migrate();

            var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (appDbContext.Users.Any())
            {
                logger.LogInformation("Usuarios ya existentes en la BD");
                return;
            }

            foreach (var testUser in Config.Users)
            {
                var user = new ApplicationUser
                {
                    UserName = testUser.Username
                };

                var result = userMgr.CreateAsync(user, testUser.Password).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(user, testUser.Claims).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                logger.LogInformation(("Usuario creado en la BD"));
            }
        }
    }
}