using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;

namespace ClickBytez.EF.Gateway.Core.Abstractions
{
    public interface IAction
    {
         ActionType Type {  get;  }
         void Execute();
    }

    public interface IAction<out TEntityType> : IAction
        where TEntityType : IEntity
    {
        TEntityType Entity {  get; }
        
    }
}