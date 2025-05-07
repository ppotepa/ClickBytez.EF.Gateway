using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;
using Newtonsoft.Json;
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

        protected ActionBase(JToken token, JToken filters)
        {
            Entity = token.ToObject<TEntity>();

            if (filters is not null) 
            {
                Filters = JsonConvert.DeserializeObject<string[]>(filters?.ToString()) ?? [];
            }
        }

        public abstract ActionType Type { get; }
        public TEntity Entity { get; set; }
        public string[] Filters { get; set; }

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
        public CreateEntityAction(JToken token, JToken filter) : base(token, filter) { }

        public override ActionType Type
        {
            get => ActionType.Create;
        }
    }

    public class ReadEntityAction<TEntity> : ActionBase<TEntity>, IReadEntityAction
        where TEntity : IEntity, new()
    {
        public ReadEntityAction() : base() { }
        public ReadEntityAction(JToken token, JToken filter) : base(token, filter) { }

        public override ActionType Type
        {
            get => ActionType.Read;
        }
    }

    public class UpdateEntityAction<TEntity> : ActionBase<TEntity>, IUpdateEntityAction
        where TEntity : IEntity, new()
    {
        public UpdateEntityAction() : base() { }
        public UpdateEntityAction(JToken token, JToken filter) : base(token, filter) { }

        public override ActionType Type
        {
            get => ActionType.Update;
        }
    }

    public class DeleteEntityAction<TEntity> : ActionBase<TEntity>, IDeleteEntityAction
        where TEntity : IEntity, new()
    {
        public DeleteEntityAction() : base() { }
        public DeleteEntityAction(JToken token, JToken filter) : base(token, filter) { }

        public override ActionType Type
        {
            get => ActionType.Delete;
        }
    }

    public interface IReadEntityAction { }
    public interface ICreateEntityAction { }
    public interface IDeleteEntityAction { }
    public interface IUpdateEntityAction { }
}
