using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace ClickBytez.EF.Gateway.Core.Providers
{
    internal class InternalEntitiesProvider
    {
        #region Fields

        private readonly IConfiguration configuration;
        private readonly GatewayConfiguration GateWayConfiguration;
        private Type[] _availableEntities = default;

        #endregion Fields

        #region Constructors

        public InternalEntitiesProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.GateWayConfiguration = configuration.GetGatewayConfiguration();
        }

        public InternalEntitiesProvider() { }

        #endregion Constructors

        #region Properties

        public Type[] AvailableEntities
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
                            type => type.GetInterfaces().Contains(typeof(IEntity))
                                    && type.IsInterface is false
                                    && type.IsAbstract is false

                        )
                        .ToArray();
                }

                return _availableEntities;
            }
        }

        #endregion Properties
    }
}
