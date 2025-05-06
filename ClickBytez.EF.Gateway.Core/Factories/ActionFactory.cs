using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;
using ClickBytez.EF.Gateway.Core.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ClickBytez.EF.Gateway.Core.Extensions.DependencyInjection
{
    internal class ActionFactory 
    {
        internal static IAction<IEntity> CreateInternal(ActionType actionType, Type entityType,  JToken entityToken)
        {
            Type targetGenericType = default;

            switch (actionType)
            {
                case ActionType.Create: targetGenericType = typeof(CreateEntityAction<>).MakeGenericType(entityType); break;
                case ActionType.Update: targetGenericType = typeof(UpdateEntityAction<>).MakeGenericType(entityType); break;
                case ActionType.Read: targetGenericType = typeof(ReadEntityAction<>).MakeGenericType(entityType); break;
                case ActionType.Delete: targetGenericType = typeof(DeleteEntityAction<>).MakeGenericType(entityType); break;
            }

            IAction<IEntity> resultActionInstance = Activator.CreateInstance(targetGenericType, new[] { entityToken }) as IAction<IEntity>;
            return resultActionInstance;
        }

        internal static IAction<IEntity> CreateInstance(JObject jObject, InternalEntitiesProvider provider)
        {
            EntityAction action = new EntityAction(jObject["type"].Value<string>());
            JToken jEntity = jObject["entity"];

            Type entityType = provider.AvailableEntities.FirstOrDefault
            (
                entityType => entityType.Name.Equals(action.EntityName, StringComparison.OrdinalIgnoreCase)
            );

            IAction<IEntity> targetInstance = CreateInternal(action.ActionType, entityType, jEntity);
            return targetInstance;
        }
    }
}