using AuthorizationServiceWithCasbin.Configurations;
using AuthorizationServiceWithCasbin.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Mapping;

namespace AuthorizationServiceWithCasbin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigSwagger();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddBusinessServices();
            services.AddPostgreDbContext(Configuration);

            //services.AddScoped<AppDbContext, AppDbContext>();

            // register (Authentication - JWT) Module 
            services.RegisterSecurityModule(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwaggerUi3();
            app.UseOpenApi();

            app.UseApplicationSecurity();
            app.UseAuthentication();
        }
    }
}
