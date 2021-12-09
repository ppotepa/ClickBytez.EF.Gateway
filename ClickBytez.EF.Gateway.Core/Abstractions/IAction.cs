using ClickBytez.EF.Gateway.Core.Abstractions.Entities;

namespace ClickBytez.EF.Gateway.Core.Abstractions
{
    public interface IAction
    {
        public void Execute();
    }

    public interface IAction<TEntityType> : IAction
        where TEntityType : IEntity
    {

    }
}