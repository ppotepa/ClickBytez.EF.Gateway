using ClickBytez.EF.DemoStore;
using ClickBytez.EF.Gateway.Core.Abstractions;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using ClickBytez.EF.Gateway.Core.Controllers;
using ClickBytez.EF.Gateway.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ClickBytez.EF.Gateway.Tests
{
    [TestFixture]
    public class ActionControllerTests
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<DbContext> _mockDbContext;
        private ActionController _controller;

        public class TestEntity : IEntity
        {
            public object Id { get; set; }
            public string Name { get; set; }
        }

        private class CreateEntityAction : IAction<IEntity>, ICreateEntityAction
        {
            public ActionType Type => ActionType.Create;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public CreateEntityAction(IEntity entity)
            {
                Entity = entity;
            }

            public void Execute() { }
        }

        private class ReadEntityAction : IAction<IEntity>, IReadEntityAction
        {
            public ActionType Type => ActionType.Read;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public ReadEntityAction(IEntity entity, string[] filters = null)
            {
                Entity = entity;
                Filters = filters ?? Array.Empty<string>();
            }

            public void Execute() { }
        }

        private class UpdateEntityAction : IAction<IEntity>, IUpdateEntityAction
        {
            public ActionType Type => ActionType.Update;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public UpdateEntityAction(IEntity entity)
            {
                Entity = entity;
            }

            public void Execute() { }
        }

        private class DeleteEntityAction : IAction<IEntity>, IDeleteEntityAction
        {
            public ActionType Type => ActionType.Delete;
            public IEntity Entity { get; private set; }
            public string[] Filters { get; set; } = Array.Empty<string>();

            public DeleteEntityAction(IEntity entity)
            {
                Entity = entity;
            }

            public void Execute() { }
        }

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockDbContext = new Mock<DbContext>();

            var mockDbSet = new Mock<DbSet<TestEntity>>();

            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Test Entity 1" },
                new TestEntity { Id = 2, Name = "Test Entity 2" }
            }
            .AsQueryable();

            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockDbContext.Setup(c => c.Set<TestEntity>()).Returns(mockDbSet.Object);
            _mockDbContext.Setup(m => m.Add(It.IsAny<object>())).Returns((object entity) =>
            {
                var entry = new Mock<EntityEntry<object>>();
                entry.Setup(e => e.Entity).Returns(entity);
                return entry.Object;
            });

            _mockDbContext.Setup(m => m.Update(It.IsAny<object>())).Returns((object entity) =>
            {
                var entry = new Mock<EntityEntry<object>>();
                entry.Setup(e => e.Entity).Returns(entity);
                return entry.Object;
            });

            _mockDbContext.Setup(m => m.Remove(It.IsAny<object>())).Returns((object entity) =>
            {
                var entry = new Mock<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<object>>();
                entry.Setup(e => e.Entity).Returns(entity);
                return entry.Object;
            });

            _mockDbContext.Setup(m => m.SaveChanges()).Returns(1);

            var mockDatabase = new Mock<DatabaseFacade>(_mockDbContext.Object);
            mockDatabase.Setup(d => d.EnsureCreated()).Returns(true);
            _mockDbContext.Setup(c => c.Database).Returns(mockDatabase.Object);

            _controller = new ActionController(_mockConfiguration.Object);
            _controller.UseContext(_mockDbContext.Object);
        }

        [Test]
        public void Execute_CreateEntityAction_AddsEntityToContext()
        {
            var entity = new TestEntity { Id = 3, Name = "New Entity" };
            var action = new CreateEntityAction(entity);

            var result = _controller.Execute(action);

            _mockDbContext.Verify(m => m.Add(entity), Times.Once);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsNotNull(result);
            dynamic dynamicResult = result;
            Assert.AreEqual(entity, dynamicResult.entity);
        }

        [Test]
        public void Execute_ReadEntityAction_RetrievesEntitiesFromContext()
        {
            var entity = new TestEntity();
            var action = new ReadEntityAction(entity);

            var result = _controller.Execute(action);

            Assert.IsNotNull(result);
            dynamic dynamicResult = result;
            Assert.IsNotNull(dynamicResult.entity);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void Execute_ReadEntityAction_WithFilters_AppliesFilters()
        {
            var entity = new TestEntity();
            var filters = new[] { "Name eq 'Test Entity 1'" };
            var action = new ReadEntityAction(entity, filters);

            var result = _controller.Execute(action);

            Assert.IsNotNull(result);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void Execute_UpdateEntityAction_UpdatesEntityInContext()
        {
            var entity = new TestEntity { Id = 1, Name = "Updated Entity" };
            var action = new UpdateEntityAction(entity);

            var result = _controller.Execute(action);

            _mockDbContext.Verify(m => m.Update(entity), Times.Once);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsNotNull(result);
            dynamic dynamicResult = result;
            Assert.AreEqual(entity, dynamicResult.entity);
        }

        [Test]
        public void Execute_DeleteEntityAction_RemovesEntityFromContext()
        {
            var entity = new TestEntity { Id = 1, Name = "Entity to Delete" };
            var action = new DeleteEntityAction(entity);

            var result = _controller.Execute(action);

            _mockDbContext.Verify(m => m.Remove(entity), Times.Once);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsNotNull(result);
            dynamic dynamicResult = result;
            Assert.AreEqual(entity, dynamicResult.entity);
            Assert.AreEqual(1, dynamicResult.recordCount);
            Assert.IsTrue(dynamicResult.deleted);
        }

        [Test]
        public void UseContext_SetsContextAndEnsuresCreated()
        {
            var mockContext = new Mock<ExtendedDbContext>();
            var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
            mockDatabase.Setup(d => d.EnsureCreated()).Returns(true);
            mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

            _controller.UseContext(mockContext.Object);

            mockDatabase.Verify(d => d.EnsureCreated(), Times.Once);
        }
    }
}
