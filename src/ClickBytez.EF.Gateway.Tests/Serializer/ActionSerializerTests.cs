using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Converters;
using ClickBytez.EF.Gateway.Core.Enumerations;
using ClickBytez.EF.Gateway.Core.Providers;
using Moq;
using Newtonsoft.Json;

namespace ClickBytez.EF.Gateway.Tests.Serializer
{
    public partial class ActionSerializerTests
    {
        #region Fields

        private readonly ActionJsonConverter _converter;
        private readonly Mock<InternalEntitiesProvider> _entitiesProviderMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        #endregion Fields

        #region Constructors

        public ActionSerializerTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _entitiesProviderMock = new Mock<InternalEntitiesProvider>();

            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(InternalEntitiesProvider)))
                .Returns(_entitiesProviderMock.Object);

            _converter = new ActionJsonConverter(_serviceProviderMock.Object);
        }

        #endregion Constructors

        #region Methods

        [Test]
        public void ReadJson_InvalidJson_ThrowsInvalidOperationException()
        {
            var invalidJson = @"{ ""Invalid"": ""Data"" }";
            var reader = new JsonTextReader(new System.IO.StringReader(invalidJson));
            var serializer = new JsonSerializer();
            
            Assert.Throws<InvalidOperationException>(() =>
                _converter.ReadJson(reader, typeof(IAction<IEntity>), null, false, serializer));
        }

        [Test]
        public void ReadJson_InvalidJson_ThrowsJsonSerializationException()
        {
            var invalidJson = @"{ x }";
            var reader = new JsonTextReader(new System.IO.StringReader(invalidJson));
            var serializer = new JsonSerializer();

            Assert.Throws<JsonReaderException>(() =>
                _converter.ReadJson(reader, typeof(IAction<IEntity>), null, false, serializer));
        }

        [Test]
        public void ReadJson_NullReader_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _converter.ReadJson(null, typeof(IAction<IEntity>), null, false, new JsonSerializer()));
        }

        [Test]
        public void ReadJson_ValidJson_ReturnsActionInstance()
        {
            var json = @"{ ""Type"": ""Create"", ""Entity"": { ""Id"": 1 }, ""Filters"": [""filter1""] }";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            var serializer = new JsonSerializer();

            _entitiesProviderMock
                .Setup(ep => ep.AvailableEntities)
                .Returns(new[] { typeof(MockEntity) });

            var result = _converter.ReadJson(reader, typeof(IAction<IEntity>), null, false, serializer);

            Assert.NotNull(result);
            Assert.Equals(ActionType.Create, result.Type);
        }

        [Test]
        public void WriteJson_ThrowsNotImplementedException()
        {
            var writer = new JsonTextWriter(new System.IO.StringWriter());
            var serializer = new JsonSerializer();
            var mockAction = new Mock<IAction<IEntity>>();

            Assert.Throws<NotImplementedException>(() =>
                _converter.WriteJson(writer, mockAction.Object, serializer));
        }

        #endregion Methods   
    }
}
