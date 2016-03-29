using NSubstitute;
using NUnit.Framework;
using SAHL.Config.Core;
using SAHL.Config.Services.Core.Decorators;
using SAHL.Core;
using SAHL.Core.Services;
using StructureMap;

namespace SAHL.Config.Services.Tests.Decorators
{
    [TestFixture]
    public class TestSortQueryHandlerDecorator
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                            scanner.TheCallingAssembly();
                            scanner.WithDefaultConventions();
                        });

                    expression.For<IIocContainer>().Use<StructureMapIocContainer>();
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(iocContainer);
        }

        [Test]
        public void Constructor_GivenServiceQueryHandler_ShouldSetProperty()
        {
            //---------------Set up test pack-------------------
            var serviceQueryHandler = Substitute.For<IServiceQueryHandler<TestQuery>>();
            var queryParameterManager = this.iocContainer.GetInstance<IQueryParameterManager>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var decorator = new SortQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, queryParameterManager);
            //---------------Test Result -----------------------
            Assert.AreSame(serviceQueryHandler, decorator.InnerQueryHandler);
        }

        [Test]
        public void HandleQuery_ShouldCallInnerHandler()
        {
            //---------------Set up test pack-------------------
            var serviceQueryHandler = Substitute.For<IServiceQueryHandler<TestQuery>>();
            var queryParameterManager = this.iocContainer.GetInstance<IQueryParameterManager>();
            var decorator = new SortQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, queryParameterManager);
            var query = new TestQuery();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            decorator.HandleQuery(query);
            //---------------Test Result -----------------------
            serviceQueryHandler.Received(1).HandleQuery(query);
        }
    }
}