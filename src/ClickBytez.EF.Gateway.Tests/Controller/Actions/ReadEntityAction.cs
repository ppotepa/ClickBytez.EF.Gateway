using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;

namespace ClickBytez.EF.Gateway.Tests.Controller
{
    public partial class ActionControllerTests
    {
        private class ReadEntityAction : IAction<IEntity>, IReadEntityAction
        {
            public ActionType Type => ActionType.Read;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public ReadEntityAction(IEntity entity, string[] filters = null)
            {
                Entity = entity;
                Filters = filters ?? Array.Empty<string>();
            }

            public void Execute() { }
        }
    }
}
