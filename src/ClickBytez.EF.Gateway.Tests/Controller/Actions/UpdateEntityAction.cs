using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;

namespace ClickBytez.EF.Gateway.Tests.Controller
{
    public partial class ActionControllerTests
    {
        private class UpdateEntityAction : IAction<IEntity>, IUpdateEntityAction
        {
            public ActionType Type => ActionType.Update;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public UpdateEntityAction(IEntity entity)
            {
                Entity = entity;
            }

            public void Execute() { }
        }
    }
}
