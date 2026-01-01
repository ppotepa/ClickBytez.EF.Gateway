using PortalZ.Configuration;
using Microsoft.Extensions.Configuration;

namespace PortalZ.Extensions
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




