using PortalZ.Abstractions;
using PortalZ.Controllers;
using PortalZ.Conventions;
using PortalZ.Drivers;
using PortalZ.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PortalZ.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering PortalZ gateway services.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Registers the core gateway services. Requires a specific driver to be registered separately.
        /// </summary>
        public static IServiceCollection UsePortalZGateway(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IInternalEntitiesProvider, InternalEntitiesProvider>();

            services.AddMvc(options =>
            {
                options.Conventions.Add(new CustomRoutingControllerModelConvention(configuration));
            });

            services.AddScoped<ActionController>();

            return services;
        }
    }
}



