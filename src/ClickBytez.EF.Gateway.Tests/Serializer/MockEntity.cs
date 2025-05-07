using ClickBytez.EF.Gateway.Core.Abstractions.Entities;

namespace ClickBytez.EF.Gateway.Tests.Serializer
{
    public partial class ActionSerializerTests
    {
        #region Classes

        private class MockEntity : IEntity<int>
        {
            #region Properties

            public int Id { get; set; }
            object IEntity.Id { get => Id; set => Id = (int)value; }

            #endregion Properties
        }

        #endregion Classes
    }
}
