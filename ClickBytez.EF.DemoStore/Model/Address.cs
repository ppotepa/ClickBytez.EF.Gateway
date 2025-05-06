using ClickBytez.EF.Gateway.Core.Abstractions.Entities;

namespace ClickBytez.EF.DemoStore.Model
{
    public class Address : ExtendedEntity<Guid>
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}


