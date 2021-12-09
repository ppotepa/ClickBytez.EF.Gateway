using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Binders;
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
        private static GatewayConfiguration _configuration = default;
        private DbContext context = default;

        public ActionController(IConfiguration configuration)
        {
            Configuration = configuration.GetGatewayConfiguration();
        }

        private static GatewayConfiguration Configuration
        {
            get => _configuration;
            set
            {
                if (_configuration is null)
                {
                    _configuration = value;
                }
            }
        }

        [HttpPost]
        [Route(EMPTY_STRING)]
        public string Execute([ModelBinder(BinderType = typeof(GenericActionBinder))] ActionBase<IEntity> action)
        {
            return string.Empty;
        }

        internal void UseContext(DbContext context)
        {
            this.context = context;
        }
    }
}
