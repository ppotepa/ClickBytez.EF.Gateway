using PortalZ.Abstractions;
using PortalZ.Abstractions.Entities;
using PortalZ.Enumerations;

namespace PortalZ.Tests.Controller
{
    public partial class ActionControllerTests
    {
        private class CreateEntityAction : IAction<IEntity>, ICreateEntityAction
        {
            public ActionType Type => ActionType.Create;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public CreateEntityAction(IEntity entity)
            {
                Entity = entity;
            }

            public void Execute() { }
        }
    }
}




