using PortalZ.Abstractions.Entities;

namespace PortalZ.Tests.Serializer
{
    public class MockEntity : ExtendedEntity<int>
    {
        public string? Name { get; set; }
    }
}



