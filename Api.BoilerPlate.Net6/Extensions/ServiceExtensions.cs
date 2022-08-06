using Api.BoilerPlate.Net6.Authorization;
using Api.BoilerPlate.Net6.Helpers;
using Api.BoilerPlate.Net6.Services;
using Contract;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Api.BoilerPlate.Net6.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCorsConfigurations(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void AddIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => { });
        }

        public static void AddDbContextConfigurations(this IServiceCollection services, IConfiguration config)
        {
            var connectionConfig = config["ConnectionStrings:SqlServer"];
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(connectionConfig));
        }

        public static void AddRepositoryConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddAppSettingsConfigurations(this IServiceCollection services, IConfiguration config)
        {
            // configure strongly typed settings object
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
        }

        public static void AddDIServicesConfigurations(this IServiceCollection services)
        {
            // configure DI for application services
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
