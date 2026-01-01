using PortalZ.Abstractions.Entities;

namespace PortalZ.Model
{
    public class User : ExtendedEntity<Guid>
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<User>? Friends { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; }
        public virtual ICollection<Article>? Articles { get; set; }
    }
}





