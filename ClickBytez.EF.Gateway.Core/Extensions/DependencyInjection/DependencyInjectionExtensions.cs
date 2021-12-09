using ClickBytez.EF.Gateway.Core.Abstractions.Utilities;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Controllers;
using ClickBytez.EF.Gateway.Core.Extensions;
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
            @this.AddMvcCore(options =>
            {
                options.Conventions.Add(new CustomRoutingControllerModelConvention(configuration));
            });

            @this.AddTransient(typeof(ActionController), (provider) =>
            {
                GatewayConfiguration gatewayConfiguration = configuration.GetGatewayConfiguration();
                DbContext ctx = ActivatorUtilities.CreateInstance(provider, contextType) as DbContext;
                ActionController controller = new ActionController(provider.GetService<IConfiguration>());
                controller.UseContext(ctx);

                return controller;
            });

            return @this;
        }
    }
}
