using ClickBytez.EF.Gateway.Core.Abstractions.Entities;

namespace ClickBytez.EF.Gateway.Tests.Controller
{
    public partial class ActionControllerTests
    {
        public class TestEntity : ExtendedEntity<Guid>
        {            
            public string Name { get; set; }            
        }
    }
}
