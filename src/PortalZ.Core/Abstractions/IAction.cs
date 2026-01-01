using PortalZ.Abstractions.Entities;
using PortalZ.Enumerations;

namespace PortalZ.Abstractions
{
    public interface IAction
    {
         ActionType Type {  get;  }
         void Execute();
    }

    public interface IAction<out TEntityType> : IAction
        where TEntityType : IEntity
    {
        TEntityType Entity { get; }
        string[] Filters { get; set; }
    }
}



