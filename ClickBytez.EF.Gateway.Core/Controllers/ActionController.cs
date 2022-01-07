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
            context.Add(action.Entity);
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
