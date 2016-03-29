using System.Collections.Generic;

using StructureMap;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.Core.Services;
using SAHL.Config.Services.Core.Decorators;

namespace SAHL.Config.Services.Tests.Decorators
{
    [TestFixture]
    public class TestPaginationQueryHandlerDecorator
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
            var serviceQueryHandler   = Substitute.For<IServiceQueryHandler<TestQuery>>();
            var queryParameterManager = this.iocContainer.GetInstance<IQueryParameterManager>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var decorator = new PaginationQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, queryParameterManager);
            //---------------Test Result -----------------------
            Assert.AreSame(serviceQueryHandler, decorator.InnerQueryHandler);
        }

        [Test]
        public void HandleQuery_ShouldCallInnerHandler()
        {
            //---------------Set up test pack-------------------
            var serviceQueryHandler     = Substitute.For<IServiceQueryHandler<TestQuery>>();
            var queryParameterManager   = this.iocContainer.GetInstance<IQueryParameterManager>();
            var decorator               = new PaginationQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, queryParameterManager);
            var query                   = new TestQuery();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            decorator.HandleQuery(query);
            //---------------Test Result -----------------------
            serviceQueryHandler.Received(1).HandleQuery(query);
        }

        [Test]
        public void HandleQuery_GivenNoResults_ShouldSetResultPaginationProperties()
        {
            //---------------Set up test pack-------------------
            var query                   = new TestQuery();
            var serviceQueryHandler     = Substitute.For<IServiceQueryHandler<TestQuery>>();

            var queryModels        = new List<TestQueryModel>();
            var serviceQueryResult = new ServiceQueryResult<TestQueryModel>(queryModels);

            serviceQueryHandler.When(handler => handler.HandleQuery(query)).Do(info => query.Result = serviceQueryResult);

            var queryParameterManager   = this.iocContainer.GetInstance<IQueryParameterManager>();
            var decorator               = new PaginationQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, queryParameterManager);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            decorator.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, query.Result.ResultCountInAllPages);
            Assert.AreEqual(0, query.Result.NumberOfPages);
            Assert.AreEqual(0, query.Result.ResultCountInPage);
        }
    }
}
