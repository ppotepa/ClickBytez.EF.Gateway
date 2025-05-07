using ClickBytez.EF.Gateway.Core.Abstractions.Entities;

namespace ClickBytez.EF.Gateway.Tests.Serializer
{
    public partial class ActionSerializerTests
    {
        #region Classes

        private class MockEntity : ExtendedEntity<int>
        {
            #region Properties

            public int Id { get; set; }            
            public string Name { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}
