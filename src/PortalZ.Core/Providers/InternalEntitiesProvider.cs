using PortalZ.Abstractions;
using PortalZ.Abstractions.Entities;
using PortalZ.Configuration;
using PortalZ.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace PortalZ.Providers
{
    internal class InternalEntitiesProvider : IInternalEntitiesProvider
    {
        #region Fields
        
        private readonly GatewayConfiguration _gateWayConfiguration;
        private Type[] _availableEntities = default;

        #endregion Fields

        #region Constructors

        public InternalEntitiesProvider(IConfiguration configuration)
        {            
            this._gateWayConfiguration = configuration.GetGatewayConfiguration();
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
                                    && !type.IsInterface
                                    && !type.IsAbstract
                        )
                        .ToArray();
                }

                return _availableEntities;
            }
        }

        #endregion Properties

        public Type[] GetEntities()
        {
            return AvailableEntities;
        }
    }
}




