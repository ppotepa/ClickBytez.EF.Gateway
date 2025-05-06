using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;

namespace ClickBytez.EF.Gateway.Core.Controllers
{
    [ApiController]
    public class ActionController : ControllerBase
    {
        private const string EMPTY_STRING = "";
        private readonly GatewayConfiguration _configuration = default;
        private DbContext context = default;

        // Cache for DbSet types
        private static readonly ConcurrentDictionary<Type, object> DbSetCache = new();

        public ActionController(IConfiguration configuration)
        {
            _configuration = configuration.GetGatewayConfiguration();
        }

        [HttpPost]
        [Route(EMPTY_STRING)]
        [Consumes("application/json")]
        public object Execute(IAction<IEntity> action)
        {
            object resultEntity = default;

            if (action is ICreateEntityAction)
            {
                context.Add(action.Entity);
                resultEntity = action.Entity;
            }

            if (action is IReadEntityAction)
            {
                var entityType = action.Entity.GetType();                
                var dbSet = DbSetCache.GetOrAdd(entityType, type =>
                {
                    return context.GetType()
                                  .GetMethod("Set", Type.EmptyTypes)
                                  ?.MakeGenericMethod(type)
                                  .Invoke(context, null);
                });

                resultEntity = dbSet;
            }

            if (action is IUpdateEntityAction)
            {
                object id = action.Entity.Id;
                object id2 = action.Entity["Name"];
                object targetEntity = context.Find(action.Entity.GetType(), [action.Entity["Id"]]);

                resultEntity = context.Update(action.Entity).Entity;
            }

            int resultCount = context.SaveChanges();

            return new
            {
                recordCount = resultCount,
                entity = resultEntity
            };
        }

        internal void UseContext(DbContext context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }
    }
}
