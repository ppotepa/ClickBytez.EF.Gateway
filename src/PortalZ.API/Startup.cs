using PortalZ;
using PortalZ.Configuration;
using PortalZ.Conventions;
using PortalZ.Extensions.DependencyInjection;
using PortalZ.Drivers.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace PortalZ.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore(options => options.Conventions.Add(new CustomRoutingControllerModelConvention(Configuration)))
                .AddNewtonsoftJson()
                .AddControllersAsServices();

            // Register core gateway services
            services.UsePortalZGateway(Configuration);

            // Register EF Core driver
            services.AddEFCoreDriver(typeof(ReflectiveInMemoryContext));

            services.AddTransient<ReflectiveInMemoryContext>();
            services.AddSingleton<IConfigureOptions<MvcNewtonsoftJsonOptions>, ActionJsonOptions>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PortalZ.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseStaticFiles();

            
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".yaml"] = "application/x-yaml";
            provider.Mappings[".yml"] = "application/x-yaml";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider,                
            });

            if (env.IsDevelopment())
            {                
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "swagger";
                    c.SwaggerEndpoint("/swagger/openapi.yaml", "My API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}




