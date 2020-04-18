using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    // Url del servidor de autorización (identityserver4)
                    options.Authority = "http://kronos:5010";
                    options.RequireHttpsMetadata = false;

                    // nombre del ApiResource
                    options.ApiName = "api.iya";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("esAdmin", builder => builder.RequireClaim("rol", "admin"));
                options.AddPolicy("esFull", builder =>
                {
                    // require scope
                    builder.RequireScope("api.iya.full");
                    // and require scope2 or scope3
                    //builder.RequireScope("scope2", "scope3");
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}