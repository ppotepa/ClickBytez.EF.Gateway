using PortalZ.Abstractions;
using PortalZ.Abstractions.Entities;
using PortalZ.Converters;
using PortalZ.Enumerations;
using PortalZ.Providers;
using Moq;
using Newtonsoft.Json;

namespace PortalZ.Tests.Serializer
{
    public partial class ActionSerializerTests
    {
        #region Fields

        private readonly ActionJsonConverter _converter;
        private readonly Mock<IInternalEntitiesProvider> _entitiesProviderMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        #endregion Fields

        #region Constructors

        public ActionSerializerTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _entitiesProviderMock = new Mock<IInternalEntitiesProvider>();

            _entitiesProviderMock
               .Setup(provider => provider.AvailableEntities)
               .Returns([typeof(MockEntity)]);

            _serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IInternalEntitiesProvider)))
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
            var json = @"{""type"":""create.MockEntity"",""entity"":{""name"":""test""}}";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            var serializer = new JsonSerializer();

            var result = _converter.ReadJson(reader, typeof(IAction<IEntity>), null, false, serializer);

            Assert.NotNull(result);
            Assert.That(result.Type, Is.EqualTo(ActionType.Create));
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




