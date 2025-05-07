using ClickBytez.EF.Gateway.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace ClickBytez.EF.Gateway.Core.Extensions
{
    internal static class IConfigurationExtensions
    {
        public static GatewayConfiguration GetGatewayConfiguration(this IConfiguration @object)
        {
            IConfigurationSection section = @object.GetSection(nameof(GatewayConfiguration));
            GatewayConfiguration config =  GatewayConfiguration.FromSection(section);
            return config;
        }
    }
}
