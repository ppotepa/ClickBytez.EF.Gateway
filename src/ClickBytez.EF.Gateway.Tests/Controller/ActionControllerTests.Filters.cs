

using Moq;

namespace ClickBytez.EF.Gateway.Tests.Controller
{
    [TestFixture]
    public partial class ActionControllerTests
    {
        [Test]
        public void Execute_ReadEntityAction_With_StartsWith_Filter_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.startswith(Test)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);

            _dbContextMock.Verify(dbContext => dbContext.SaveChanges(), Times.Once);
        }

        [Test]
        public void Execute_ReadEntityAction_With_EndsWith_Filter_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.endswith(Entity)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);

            _dbContextMock.Verify(dbContext => dbContext.SaveChanges(), Times.Once);
        }

        [Test]
        public void Execute_ReadEntityAction_With_ExactMatch_Filter_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.eq(Test Entity 1)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);

            _dbContextMock.Verify(dbContext => dbContext.SaveChanges(), Times.Once);
        }

    }
}
