using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;

namespace ClickBytez.EF.Gateway.Tests.Controller
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
