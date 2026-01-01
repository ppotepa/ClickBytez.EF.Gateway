using PortalZ.Configuration;
using PortalZ.Controllers;
using PortalZ.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;

namespace PortalZ.Conventions
{
    public class CustomRoutingControllerModelConvention : IControllerModelConvention
    {
        private const string Controller = "Controller";
        private const string DefaultRoute = "api/action/execute";

        private readonly IConfiguration _configuration;
        private readonly GatewayConfiguration _gatewayConfiguration;

        public CustomRoutingControllerModelConvention(IConfiguration configuration)
        {
            _configuration = configuration;
            _gatewayConfiguration = configuration.GetGatewayConfiguration();
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerName.Equals(nameof(ActionController).Replace(Controller, string.Empty)))
            {
                string RouteTemplate = _gatewayConfiguration.EndpointUrl ?? DefaultRoute;

                SelectorModel firstSelector = controller.Selectors[0];

                if (firstSelector.AttributeRouteModel is null)
                    firstSelector.AttributeRouteModel = new AttributeRouteModel();

                firstSelector.AttributeRouteModel.Template = RouteTemplate;
            }
        }
    }
}




