using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClickBytez.EF.Gateway.Core.Binders
{
    public class GenericActionBinder : IModelBinder
    {
        private readonly IConfiguration configuration;
        private readonly GatewayConfiguration gatewayConfiguration;
        private static Type[] _availableEntities;

        private static Type[] AvailableEntities
        {
            get
            {
                if (_availableEntities is null)
                {
                    _availableEntities = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where
                        (
                            type =>     type.GetInterfaces().Contains(typeof(IEntity)) 
                                    &&  type.IsInterface is false 
                                    &&  type.IsAbstract is false
                                    &&  type.Namespace.Contains("ClickBytez.EF.Gateway") is false
                        ).ToArray();
                }

                return _availableEntities;
            }
        }

        public GenericActionBinder(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.gatewayConfiguration = configuration.GetGatewayConfiguration();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var entities = AvailableEntities;
            return Task.CompletedTask;
        }
    }
}