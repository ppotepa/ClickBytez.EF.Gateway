using ClickBytez.EF.DemoStore;
using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace ClickBytez.EF.Gateway.Core.Controllers
{
    [ApiController]
    public class ActionController : ControllerBase
    {
        private const string EMPTY_STRING = "";
        private readonly IConfiguration _configuration;

        private DbContext context = default;

        public ActionController(IConfiguration configuration, ExtendedDbContext context)
        {
            this._configuration = configuration;
            this.context = context;
        }

        [HttpPost]
        [Route(EMPTY_STRING)]
        [Consumes("application/json")]
        public object Execute(IAction<IEntity> action)
        {
            dynamic resultEntity = default;
            int resultCount = 0;

            if (action is ICreateEntityAction)
            {
                context.Add(action.Entity);
                resultEntity = action.Entity;
                resultCount = context.SaveChanges();
            }
            
            if (action is IReadEntityAction)
            {
                Type entityType = action.Entity.GetType();

                MethodInfo methodInfo = context.GetType().GetMethod(nameof(context.Set), Type.EmptyTypes)?.MakeGenericMethod(entityType);
                IQueryable data = methodInfo?.Invoke(context, null) as IQueryable; 

                resultEntity = data;

                if (action.Filters.Any())
                {
                    resultEntity = data.ApplyRequest(action.Filters);
                    resultCount = Queryable.Count(resultEntity);
                }
            }
            if (action is IUpdateEntityAction)
            {
                resultEntity = context.Update(action.Entity).Entity;
                resultCount = context.SaveChanges();
            }
            if (action is IDeleteEntityAction)
            {
                resultEntity = context.Remove(action.Entity).Entity;
                resultCount = context.SaveChanges();
            }

            return new
            {
                resultCount = resultCount,
                entity = resultEntity,
                deleted = resultCount > 0
            };

            throw new InvalidOperationException();
        }
        
        internal void UseContext(DbContext context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }
    }
}
