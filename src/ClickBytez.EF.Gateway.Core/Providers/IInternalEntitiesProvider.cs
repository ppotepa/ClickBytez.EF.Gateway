using System;

namespace ClickBytez.EF.Gateway.Core.Abstractions
{
    internal interface IInternalEntitiesProvider
    {
        Type[] AvailableEntities { get; }
        Type[] GetEntities();
    }
}
