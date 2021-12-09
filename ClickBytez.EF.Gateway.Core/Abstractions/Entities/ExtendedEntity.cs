using System;
using System.Linq;

namespace ClickBytez.EF.Gateway.Core.Abstractions.Entities
{
    public class ExtendedEntity<TIdentityType> : IExtendedEntity<TIdentityType>
        where TIdentityType : struct
    {
        private readonly TIdentityType createdBy = default;
        private readonly DateTime createdOn = default;
        private readonly TIdentityType deletedBy = default;
        private readonly DateTime? deletedOn = default;
        private readonly TIdentityType modifiedBy = default;
        private readonly DateTime? modifiedOn = default;

        public TIdentityType CreatedBy => createdBy;
        public DateTime CreatedOn => createdOn;
        public TIdentityType DeletedBy => deletedBy;
        public DateTime? DeletedOn => deletedOn;
        public TIdentityType Id { get; init; } = default;
        public DateTime? ModifiedOn => modifiedOn;
        public TIdentityType ModifiedBy => modifiedBy;
    }

    public abstract class Entity<TIdentityType> : IEntity<TIdentityType>
          where TIdentityType : struct
    {
        private readonly Type[] ValidIdentityType = new[]
        {
            typeof(byte),
            typeof(byte?),
            typeof(short),
            typeof(short?),
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
            typeof(Guid),
            typeof(Guid?),
        };

        protected Entity()
        {
            if (ValidIdentityType.Any(type => type == typeof(TIdentityType)) is false)
            {
                throw new InvalidOperationException("Invalid Identity Type");
            }
        }

        public TIdentityType Id { get; init; } = default;
    }
}
