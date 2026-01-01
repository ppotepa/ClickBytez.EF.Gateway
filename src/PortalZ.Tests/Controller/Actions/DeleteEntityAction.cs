using PortalZ.Abstractions;
using PortalZ.Abstractions.Entities;
using PortalZ.Enumerations;

namespace PortalZ.Tests.Controller
{
    public partial class ActionControllerTests
    {
        private class DeleteEntityAction : IAction<IEntity>, IDeleteEntityAction
        {
            public ActionType Type => ActionType.Delete;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public DeleteEntityAction(IEntity entity)
            {
                Entity = entity;
            }

            public void Execute() { }
        }
    }
}




