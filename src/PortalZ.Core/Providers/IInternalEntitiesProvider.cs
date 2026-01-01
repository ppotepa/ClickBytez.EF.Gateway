using System;

namespace PortalZ.Abstractions
{
    internal interface IInternalEntitiesProvider
    {
        Type[] AvailableEntities { get; }
        Type[] GetEntities();
    }
}




