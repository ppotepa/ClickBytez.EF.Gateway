using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClickBytez.EF.Gateway.Core.Controllers
{
    [ApiController]
    public class ActionController : ControllerBase
    {
        private const string EMPTY_STRING = "";
        private readonly GatewayConfiguration _configuration = default;
        private DbContext context = default;

        public ActionController(IConfiguration configuration)
        {
            _configuration = configuration.GetGatewayConfiguration();
        }

        [HttpPost]
        [Route(EMPTY_STRING)]
        [Consumes("application/json")]
        public object Execute(IAction<IEntity> action)
        {
            if (action is ICreateEntityAction)
            {
                context.Add(action.Entity);
            }

            if (action is IReadEntityAction)
            {
                context.Add(action.Entity);
            }

            if (action is IUpdateEntityAction)
            {
                object id = action.Entity.Id;
                object id2 = action.Entity["Name"];
                object targetEntity = context.Find(action.Entity.GetType(), [action.Entity["Id"]]);

                context.Update(action.Entity);
            }

            int resultCount = context.SaveChanges();

            return new
            {
                recordCount = resultCount,
                entity = action.Entity
            };
        }

        internal void UseContext(DbContext context)
        {
            this.context = context;
        }
    }
}
