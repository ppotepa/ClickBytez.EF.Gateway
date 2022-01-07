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

        public abstract ActionType Type { get; set; }
        public TEntity Entity { get; set; }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class ActionBase
    {
    }

    public class CreateEntityAction<TEntity> : ActionBase<TEntity>
        where TEntity : IEntity, new()
    {
        public CreateEntityAction() : base() { }
        public CreateEntityAction(JToken token) : base(token) { }

        public override ActionType Type
        {
            get => ActionType.Create;
            set => _ = ActionType.Create;
        }
    }
}
