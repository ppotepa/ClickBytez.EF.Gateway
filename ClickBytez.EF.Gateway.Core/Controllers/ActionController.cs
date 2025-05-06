using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ClickBytez.EF.Gateway.Core.Controllers
{
    [ApiController]
    public class ActionController : ControllerBase
    {
        private const string EMPTY_STRING = "";
        private readonly GatewayConfiguration _configuration = default;
        private DbContext context = default;

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
            int resultCount = 0;

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
                    var methodInfo = context.GetType().GetMethod(nameof(context.Set), Type.EmptyTypes)?.MakeGenericMethod(type);
                    var instance = Expression.Constant(context);
                    var callExpression = Expression.Call(instance, methodInfo);
                    var lambda = Expression.Lambda<Func<object>>(callExpression).Compile();

                    return lambda();
                });

                resultEntity = dbSet;
            }

            if (action is IUpdateEntityAction)
            {
                resultEntity = context.Update(action.Entity).Entity;
            }

            if (action is IDeleteEntityAction)
            {
                resultEntity = context.Remove(action.Entity).Entity;
                resultCount = context.SaveChanges();

                return new
                {
                    recordCount = resultCount,
                    entity = resultEntity,
                    deleted = resultCount > 0
                };
            }
            else
            {
                resultCount = context.SaveChanges();

                return new
                {
                    resultCount = resultCount,
                    entity = resultEntity
                };
            }
        }

        internal void UseContext(DbContext context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }
    }
}
