using System;

namespace ClickBytez.EF.Gateway.Core.Abstractions.Entities
{
    public interface IEntity
    {
        public object Id 
        {
            get; 
            set; 
        }
        object this[string index]
        {
            get => this.GetType().GetProperty(index).GetValue(this);
        }
    }

    public interface IEntity<TIdentityType> : IEntity
        where TIdentityType : struct
    {
        private const string ID_STRING = "Id";

        public new TIdentityType Id 
        {
            get => (TIdentityType)(this as IEntity).GetType().GetProperty(ID_STRING).GetValue(this);
            set => (this as IEntity).GetType().GetProperty(ID_STRING).SetValue(this, value);
        }
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
