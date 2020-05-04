using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using IdentityServer.Data;
using IdentityServer.Extensiones;
using IdentityServer.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;


namespace IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            // migration assembly required as DbContext's are in a different assembly
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string conexion = Configuration.GetConnectionString("Default");

            void OptionsBuilder(DbContextOptionsBuilder options) =>
                options.UseSqlite(conexion, sql => sql.MigrationsAssembly(migrationsAssembly));


            // if you want to add an MVC-based UI
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(conexion));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            var builder = services
                .AddIdentityServer()
                //.AddInMemoryIdentityResources(Config.Ids)
                //.AddInMemoryApiResources(Config.Apis)
                //.AddInMemoryClients(Config.Clients)
                //.AddTestUsers(Config.Users);

                .AddConfigurationStore(options => options.ConfigureDbContext = OptionsBuilder)
                .AddOperationalStore(options => options.ConfigureDbContext = OptionsBuilder)
                .AddAspNetIdentity<ApplicationUser>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            builder.AddProfileService<ProfileService>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (bool.Parse(Configuration["seed"]))
                SeedData.InicializarBasesDatos(app);

            if (env.EsDesarrollo())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDeveloperExceptionPage();

            // if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            // if you want to add MVC-based
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}