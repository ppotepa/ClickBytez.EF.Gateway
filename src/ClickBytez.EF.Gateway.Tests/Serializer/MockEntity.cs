using ClickBytez.EF.Gateway.Core.Abstractions.Entities;

namespace ClickBytez.EF.Gateway.Tests.Serializer
{
    public class MockEntity : ExtendedEntity<int>
    {
        public string? Name { get; set; }
    }
}