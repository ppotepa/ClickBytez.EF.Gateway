using PortalZ.Enumerations;
using System;

namespace PortalZ.Extensions.DependencyInjection
{
    internal class EntityAction
    {
        private readonly string entityName;
        private readonly string actionName;

        public EntityAction(string actionString)
        {
            if (string.IsNullOrWhiteSpace(actionString))
            {
                throw new ArgumentException("Action string cannot be null or empty.", nameof(actionString));
            }

            string[] split = actionString.Split(".");

            if (split.Length != 2 || string.IsNullOrWhiteSpace(split[0]) || string.IsNullOrWhiteSpace(split[1]))
            {
                throw new FormatException("Action string must be in the format 'action.entity' (e.g., 'delete.user').");
            }

            this.actionName = split[0];
            this.entityName = split[1];
        }

        public ActionType ActionType => Enum.Parse<ActionType>(actionName, true);
        public string EntityName => entityName;
    }
}



