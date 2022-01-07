using ClickBytez.EF.Gateway.API.Data;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Extensions.DependencyInjection;
using ClickBytez.EF.Gateway.Core.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;

namespace ClickBytez.EF.Gateway.API
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
            
            services
                .AddMvcCore(options => options.Conventions.Add(new CustomRoutingControllerModelConvention(Configuration)))
                .AddNewtonsoftJson()
                .AddControllersAsServices();

            services.UseEFGateway(typeof(ApplicationContext), Configuration);
            services.AddSingleton<IConfigureOptions<MvcNewtonsoftJsonOptions>, ActionJsonOptions>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClickBytez.EF.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClickBytez.EF.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
