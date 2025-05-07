using Moq;

namespace ClickBytez.EF.Gateway.Tests.Controller
{
    [TestFixture]
    public partial class ActionControllerTests
    {
        #region Methods

        [Test]
        public void Execute_ReadEntityAction_With_AgeRangeFilter_AppliesFilters()
        {
            TestEntity sampleEntity = new TestEntity();
            string[] filters = new[] { "age.gt(10)", "age.lt(15)" };

            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);
            var dbSetResult = _dbContextMock.Object.Users.Where(user => user.Age > 10 && user.Age < 15).ToList(); 

            dynamic result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.resultCount);
            Assert.AreEqual(dbSetResult, result.entity);
        }

        [Test]
        public void Execute_ReadEntityAction_With_ArrayOfFilters_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.contains(a)", "age.eq(18)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Execute_ReadEntityAction_With_EndsWith_Filter_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.endswith(Entity)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Execute_ReadEntityAction_With_ExactMatch_Filter_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.eq(Test Entity 1)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Execute_ReadEntityAction_With_StartsWith_Filter_AppliesFilters()
        {
            var sampleEntity = new TestEntity();
            var filters = new[] { "name.startswith(Test)" };
            var readActionWithFilters = new ReadEntityAction(sampleEntity, filters);

            var result = _controller.Execute(readActionWithFilters);

            Assert.IsNotNull(result);
        }

        #endregion Methods
    }
}
