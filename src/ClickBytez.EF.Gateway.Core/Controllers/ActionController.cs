﻿using ClickBytez.EF.DemoStore;
using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Filters.Querying;
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
        private readonly ExtendedDbContext context;

        public ActionController(IConfiguration configuration, DbContext context)
        {
            this._configuration = configuration;
            this.context = context as ExtendedDbContext;
        }

        [HttpPost]
        [Route(EMPTY_STRING)]
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
                    resultEntity = data.ApplyFiltersRegex(action.Filters);
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
    }
}
