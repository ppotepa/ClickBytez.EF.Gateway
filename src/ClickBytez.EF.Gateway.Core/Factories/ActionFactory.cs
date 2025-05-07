using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;
using ClickBytez.EF.Gateway.Core.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ClickBytez.EF.Gateway.Core.Extensions.DependencyInjection
{
    internal class ActionFactory 
    {
        internal static IAction<IEntity> CreateInternal(ActionType actionType, Type entityType, JToken entityToken, JToken jFilters)
        {
            Type targetGenericType = default;

            switch (actionType)
            {
                case ActionType.Create: targetGenericType = typeof(CreateEntityAction<>).MakeGenericType(entityType); break;
                case ActionType.Update: targetGenericType = typeof(UpdateEntityAction<>).MakeGenericType(entityType); break;
                case ActionType.Read: targetGenericType = typeof(ReadEntityAction<>).MakeGenericType(entityType); break;
                case ActionType.Delete: targetGenericType = typeof(DeleteEntityAction<>).MakeGenericType(entityType); break;
            }

            IAction<IEntity> resultActionInstance = Activator.CreateInstance(targetGenericType, new[] { entityToken, jFilters }) as IAction<IEntity>;
            return resultActionInstance;
        }

        internal static IAction<IEntity> CreateInstance(JObject jObject, IInternalEntitiesProvider provider)
        {
            Type entityType = null;

            try
            {
                EntityAction action = new EntityAction(jObject["type"].Value<string>());
                JToken jEntity = jObject["entity"];
                JToken jFilters = jObject["filters"];

                entityType = provider.AvailableEntities.FirstOrDefault
                (
                    entityType => entityType.Name.Equals(action.EntityName, StringComparison.OrdinalIgnoreCase)
                );

                if (entityType is null)
                {
                    throw new InvalidOperationException($"Entity type {action.EntityName} not found.");
                }

                IAction<IEntity> targetInstance = CreateInternal(action.ActionType, entityType, jEntity, jFilters);
                return targetInstance;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Error while resolving entity type: {ex.Message}", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException($"Error while resolving entity type: {ex.Message}", ex);
            }   
            catch (JsonSerializationException ex)
            {
                throw new JsonSerializationException($"Error while resolving entity type: {ex.Message}", ex);
            }
        }
    }
}