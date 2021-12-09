using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using System;
using System.Collections.Generic;

namespace ClickBytez.EF.Gateway.API.Model
{
    public class User : ExtendedEntity<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<User> Friends { get; set; }
    }
}
