using Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using NSwag;
using NSwag.Generation.Processors.Security;
using Service.Core;
using System.Linq;

namespace AuthorizationServiceWithCasbin.Extensions
{
    public static class StartupExtensions
    {
        public static void AddPostgreDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration["AppDatabaseSettings:ConnectionString"]));
        }
        public static void ConfigSwagger(this IServiceCollection services)
        {
            services.AddOpenApiDocument(document =>
            {
                document.Title = "Authorization Service";
                document.Version = "3.0";
                document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                        options.SerializerSettings.Converters.Add(new StringEnumConverter()));
        }
        public static void AddBusinessServices(this IServiceCollection services)
        {
            // service
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICasbinService, CasbinService>();
        }
    }
}
