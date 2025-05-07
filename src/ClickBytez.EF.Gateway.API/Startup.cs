using ClickBytez.EF.DemoStore;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Conventions;
using ClickBytez.EF.Gateway.Core.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace ClickBytez.EF.Gateway.API
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

            services.UseEFGateway(typeof(ReflectiveInMemoryContext), Configuration);

            services.AddTransient<ReflectiveInMemoryContext>();
            services.AddSingleton<IConfigureOptions<MvcNewtonsoftJsonOptions>, ActionJsonOptions>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClickBytez.EF.API", Version = "v1" });
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
