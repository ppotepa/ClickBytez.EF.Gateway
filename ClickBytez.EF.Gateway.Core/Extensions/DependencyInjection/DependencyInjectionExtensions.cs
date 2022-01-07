using ClickBytez.EF.Gateway.Core.Controllers;
using ClickBytez.EF.Gateway.Core.Providers;
using ClickBytez.EF.Gateway.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClickBytez.EF.Gateway.Core.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection UseEFGateway(this IServiceCollection @this, Type contextType, IConfiguration configuration)
        {
            @this.AddSingleton<InternalEntitiesProvider>();

            @this.AddMvc(options =>
            {
                options.Conventions.Add(new CustomRoutingControllerModelConvention(configuration));
            });

            @this.AddScoped(contextType);

            @this.AddScoped(typeof(DbContext), (provider) =>
            {
                return provider.GetService(contextType);
            });

            @this.AddTransient(typeof(ActionController), (provider) =>
            {
                DbContext dbContext = provider.GetService(contextType) as DbContext;
                ActionController controller = ActivatorUtilities.CreateInstance(provider, typeof(ActionController)) as ActionController;
                controller.UseContext(dbContext);
                return controller;
            });

            return @this;

        }
    }
}
