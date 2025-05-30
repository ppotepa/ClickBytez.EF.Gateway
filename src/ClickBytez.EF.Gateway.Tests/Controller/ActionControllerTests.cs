using ClickBytez.EF.Gateway.Core.Configuration;
using ClickBytez.EF.Gateway.Core.Controllers;
using ClickBytez.EF.Gateway.Tests.Shared;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ClickBytez.EF.Gateway.Tests.Controller
{
    [TestFixture]
    public partial class ActionControllerTests
    {
        private Mock<IConfiguration> _gatewayConfigurationMock;
        private Mock<InMemoryTestsDbContext> _dbContextMock;
        private ActionController _controller;

        private Mock<IConfigurationSection> _configurationSectionMock;
        private Mock<IConfiguration> _appConfigurationMock;

        [SetUp]
        public void Setup()
        {
            _configurationSectionMock = new Mock<IConfigurationSection>();
            _appConfigurationMock = new Mock<IConfiguration>();

            var gatewayConfiguration = new GatewayConfiguration
            {
                EndpointUrl = "/",
                ModelsNamespace = "ClickBytez.EF.Models",
                UseModelDll = true
            };

            _gatewayConfigurationMock = new Mock<IConfiguration>();

            _gatewayConfigurationMock
                .Setup(configuration => configuration.GetSection(It.IsAny<string>()))
                .Returns((string sectionKey) =>
                {
                    _configurationSectionMock
                        .Setup(section => section[nameof(GatewayConfiguration.EndpointUrl)])
                        .Returns(gatewayConfiguration.EndpointUrl);

                    _configurationSectionMock
                        .Setup(section => section[nameof(GatewayConfiguration.ModelsNamespace)])
                        .Returns(gatewayConfiguration.ModelsNamespace);

                    _configurationSectionMock
                        .Setup(section => section[nameof(GatewayConfiguration.UseModelDll)])
                        .Returns(gatewayConfiguration.UseModelDll.ToString());

                    _configurationSectionMock
                        .Setup(section => section.Get<GatewayConfiguration>())
                        .Returns(gatewayConfiguration);

                    return _configurationSectionMock.Object;
                });

            _dbContextMock = new Mock<InMemoryTestsDbContext>() { CallBase = true };

            _controller = new ActionController(_appConfigurationMock.Object, _dbContextMock.Object);            
        }

        [Test]
        public void Execute_CreateEntityAction_AddsEntityToContext()
        {
            TestEntity newEntity = new TestEntity { Id = Guid.NewGuid(), Name = "New Entity" };
            CreateEntityAction createAction = new CreateEntityAction(newEntity);

            object result = _controller.Execute(createAction);

            _dbContextMock.Verify(dbContext => dbContext.Add(createAction.Entity), Times.Once);
            _dbContextMock.Verify(dbContext => dbContext.SaveChanges(), Times.Once);

            Assert.IsNotNull(result);
            dynamic dynamicResult = result;
            Assert.AreEqual(newEntity, dynamicResult.entity);
        }

        [Test]
        public void Execute_ReadEntityAction_RetrievesEntitiesFromContext()
        {
            var sampleEntity = new TestEntity();
            var readAction = new ReadEntityAction(sampleEntity);

            var result = _controller.Execute(readAction);

            Assert.IsNotNull(result);
            dynamic dynamicResult = result;
            Assert.IsNotNull(dynamicResult.entity);
        }

        [Test]
        public void Execute_ReadEntityAction_With_Contains_Filter_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.contains(T)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Execute_UpdateEntityAction_UpdatesEntityInContext()
        {
            var updateAction = new UpdateEntityAction(InMemoryTestsDbContext.UserToUpdate);

            var result = _controller.Execute(updateAction);

            _dbContextMock.Verify(dbContext => dbContext.Update(InMemoryTestsDbContext.UserToUpdate), Times.Once);
            _dbContextMock.Verify(dbContext => dbContext.SaveChanges(), Times.Once);

            Assert.IsNotNull(result);
            dynamic dynamicResult = result;
            Assert.AreEqual(InMemoryTestsDbContext.UserToUpdate, dynamicResult.entity);
        }

        [Test]
        public void Execute_DeleteEntityAction_RemovesEntityFromContext()
        {
            var deleteAction = new DeleteEntityAction(InMemoryTestsDbContext.UserToDelete);
            var result = _controller.Execute(deleteAction);

            _dbContextMock.Verify(dbContext => dbContext.Remove(InMemoryTestsDbContext.UserToDelete), Times.Once);
            _dbContextMock.Verify(dbContext => dbContext.SaveChanges(), Times.Once);

            Assert.IsNotNull(result);
            dynamic dynamicResult = result;

            Assert.AreEqual(InMemoryTestsDbContext.UserToDelete, dynamicResult.entity);
            Assert.AreEqual(1, dynamicResult.resultCount);
            Assert.IsTrue(dynamicResult.deleted);
        }
    }
}
