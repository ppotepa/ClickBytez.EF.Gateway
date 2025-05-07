using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Extensions.DependencyInjection;
using ClickBytez.EF.Gateway.Core.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ClickBytez.EF.Gateway.Core.Converters
{
    public class ActionJsonConverter : JsonConverter<IAction<IEntity>>
    {
        private readonly IServiceProvider provider;
        private InternalEntitiesProvider _entitiesProvider;

        private InternalEntitiesProvider EntitiesProvider
        {
            get
            {
                if (_entitiesProvider is null)
                {
                    _entitiesProvider = provider.GetService(typeof(InternalEntitiesProvider)) as InternalEntitiesProvider;
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
                throw;
            }
            catch (InvalidOperationException)
            {
                throw; 
            }
        }

        public override void WriteJson(JsonWriter writer, IAction<IEntity> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
