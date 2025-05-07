using System;

namespace ClickBytez.EF.Gateway.Core.Abstractions.Entities
{
    public abstract class ExtendedEntity<TIdentityType> : IExtendedEntity<TIdentityType>
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

        object IEntity.Id
        {
            get => this.Id;
            set => ((IEntity)this).Id = value;
        }
    }

    public abstract class Entity<TIdentityType> : IEntity<TIdentityType>
          where TIdentityType : struct
    {
        public TIdentityType Id { get; init; } = default;
        dynamic IEntity.Id { get; set; }
    }
}
