using System;

namespace ClickBytez.EF.Gateway.Core.Abstractions.Entities
{
    public interface IEntity
    {
    }

    public interface IEntity<TIdentityType> : IEntity
        where TIdentityType : struct
    {
        public TIdentityType Id { get; init; }
    }

    public interface IExtendedEntity<TIdentityType> : IEntity<TIdentityType>
         where TIdentityType : struct
    {
        public TIdentityType CreatedBy { get; }
        public DateTime CreatedOn { get; }
        public TIdentityType DeletedBy { get; }
        public DateTime? DeletedOn { get; }
        public TIdentityType ModifiedBy { get; }
        public DateTime? ModifiedOn { get; }
    }

    public interface IExtendedEntity
    {
        public object CreatedBy { get; }
        public object CreatedOn { get; }
        public object DeletedBy { get; }
        public object DeletedOn { get; }
        public object ModifiedBy { get; }
        public object ModifiedOn { get; }
    }
}
