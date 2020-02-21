using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuakeLog.Application;
using QuakeLog.Domain;
using QuakeLog.Domain.Contracts.Services;
using QuakeLog.Infra.Services;
using QuakeLog.Tools.WebApi;

namespace QuakeLog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersCustom();

            services.AddAPIResult();

            services.AddServiceMappingsFromAssemblies<BaseApplication, IBaseService, LogProcessInfraServices>(srv =>
            {
                srv.AddSingleton<Config>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.AllowAnyMethod()
                                          .AllowAnyOrigin()
                                          .AllowAnyHeader());

            app.UseHttpsRedirection();
            
            app.UseRouting();
            
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
