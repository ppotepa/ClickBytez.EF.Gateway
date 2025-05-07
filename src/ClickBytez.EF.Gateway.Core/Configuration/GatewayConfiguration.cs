using Microsoft.Extensions.Configuration;
using System;

namespace ClickBytez.EF.Gateway.Core.Configuration
{
    public class GatewayConfiguration
    {
        public string EndpointUrl { get; internal set; }
        public string ModelsNamespace { get; internal set; }
        public bool? UseModelDll { get; internal set; }

        internal static GatewayConfiguration FromSection(IConfigurationSection section)
        {
            if (section is null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            return new GatewayConfiguration
            {
                EndpointUrl = section[nameof(EndpointUrl)],
                ModelsNamespace = section[nameof(ModelsNamespace)],
                UseModelDll = bool.Parse(section?[nameof(UseModelDll)] ?? "false")
            };
        }
    }
}