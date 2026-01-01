using PortalZ.Abstractions;
using PortalZ.Abstractions.Entities;
using PortalZ.Extensions.DependencyInjection;
using PortalZ.Providers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace PortalZ.Converters
{
    public class ActionJsonConverter : JsonConverter<IAction<IEntity>>
    {
        private readonly IServiceProvider provider;
        private IInternalEntitiesProvider _entitiesProvider;

        private IInternalEntitiesProvider EntitiesProvider
        {
            get
            {
                if (_entitiesProvider is null)
                {
                    _entitiesProvider = provider.GetService<IInternalEntitiesProvider>();
                }

                return _entitiesProvider;
            }
        }

        public ActionJsonConverter(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public override IAction<IEntity> ReadJson(JsonReader reader, Type objectType, IAction<IEntity> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                JObject @object = JObject.Load(reader);
                IAction<IEntity> instance = ActionFactory.CreateInstance(@object, EntitiesProvider);
                return instance;
            }
            catch (JsonSerializationException)
            {
                throw new JsonSerializationException("Failed to deserialize action.");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Something went wrong", ex);
            }
        }

        public override void WriteJson(JsonWriter writer, IAction<IEntity> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}




