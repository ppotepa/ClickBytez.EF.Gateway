using PortalZ.Abstractions;
using PortalZ.Abstractions.Entities;
using PortalZ.Enumerations;

namespace PortalZ.Tests.Controller
{
    public partial class ActionControllerTests
    {
        private class ReadEntityAction : IAction<IEntity>, IReadEntityAction
        {
            public ActionType Type => ActionType.Read;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public ReadEntityAction(IEntity entity, string[]? filters = null)
            {
                Entity = entity;
                Filters = filters ?? Array.Empty<string>();
            }

            public void Execute() { }
        }
    }
}




