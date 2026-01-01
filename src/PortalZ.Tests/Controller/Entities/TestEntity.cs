using PortalZ.Abstractions.Entities;

namespace PortalZ.Tests.Controller
{
    public partial class ActionControllerTests
    {
        public class TestEntity : ExtendedEntity<Guid>
        {            
            public string? Name { get; set; }      
            public int Age { get; set; }
        }
    }
}




