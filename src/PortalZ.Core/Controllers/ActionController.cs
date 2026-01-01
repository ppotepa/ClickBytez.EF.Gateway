using PortalZ;
using PortalZ.Abstractions;
using PortalZ.Abstractions.Entities;
using PortalZ.Drivers;
using PortalZ.Filters.Querying;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace PortalZ.Controllers
{
    /// <summary>
    /// Generic CRUD controller that delegates to a pluggable data driver.
    /// Supports Create, Read, Update, Delete operations via JSON action requests.
    /// </summary>
    [ApiController]
    public class ActionController : ControllerBase
    {
        private const string EMPTY_STRING = "";
        private readonly IDataDriver _driver;

        public ActionController(IDataDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        [HttpPost]
        [Route(EMPTY_STRING)]
        public object Execute(IAction<IEntity> action)
        {
            dynamic resultEntity = default;
            int resultCount = 0;

            if (action is ICreateEntityAction)
            {
                resultCount = _driver.Create(action.Entity);
                resultEntity = action.Entity;
            }
            
            if (action is IReadEntityAction)
            {
                Type entityType = action.Entity.GetType();
                var results = _driver.Read(entityType, action.Filters);
                resultEntity = results;
                resultCount = results.Count();
            }

            if (action is IUpdateEntityAction)
            {
                resultCount = _driver.Update(action.Entity);
                resultEntity = action.Entity;
            }

            if (action is IDeleteEntityAction)
            {
                resultCount = _driver.Delete(action.Entity);
                resultEntity = action.Entity;
            }

            return new
            {
                resultCount = resultCount,
                entity = resultEntity,
                deleted = resultCount > 0
            };
        }
    }
}




