using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Enumerations;
using System;

namespace ClickBytez.EF.Gateway.Core.Abstractions
{
    public class ActionBase<TEntity> : IAction<TEntity>
        where TEntity       : IEntity
    {
        public ActionType Type { get; init; }
        public TEntity Entity { get; set; }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
