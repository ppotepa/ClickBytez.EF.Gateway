using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;
using Newtonsoft.Json.Linq;
using System;

namespace ClickBytez.EF.Gateway.Core.Abstractions
{
    public abstract class ActionBase<TEntity> : ActionBase, IAction<TEntity>
        where TEntity : IEntity, new()
    {
        protected ActionBase()
        {
            Entity = new TEntity();
        }

        protected ActionBase(JToken token)
        {
            Entity = token.ToObject<TEntity>();
        }

        public abstract ActionType Type { get; }
        public TEntity Entity { get; set; }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class ActionBase
    {
    }

    public class CreateEntityAction<TEntity> : ActionBase<TEntity>, ICreateEntityAction
        where TEntity : IEntity, new()
    {
        public CreateEntityAction() : base() { }
        public CreateEntityAction(JToken token) : base(token) { }

        public override ActionType Type
        {
            get => ActionType.Create;
        }
    }

    public class UpdateEntityAction<TEntity> : ActionBase<TEntity>, IUpdateEntityAction
        where TEntity : IEntity, new()
    {
        public UpdateEntityAction() : base() { }
        public UpdateEntityAction(JToken token) : base(token) { }

        public override ActionType Type
        {
            get => ActionType.Update;
        }
    }

    public interface ICreateEntityAction { }
    public interface IDeleteEntityAction { }
    public interface IUpdateEntityAction { }

}
