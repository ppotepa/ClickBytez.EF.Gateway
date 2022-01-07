using ClickBytez.EF.Gateway.Core.Enumerations;
using System;

namespace ClickBytez.EF.Gateway.Core.Extensions.DependencyInjection
{
    internal class EntityAction
    {
        private readonly string entityName;
        private readonly string actionName;

        public EntityAction(string v)
        {
            string[] split = v.Split(".");
            this.entityName = split[1];
            this.actionName = split[0];
        }

        public ActionType ActionType => Enum.Parse<ActionType>(actionName, true);
        public string EntityName => entityName;
    }
}