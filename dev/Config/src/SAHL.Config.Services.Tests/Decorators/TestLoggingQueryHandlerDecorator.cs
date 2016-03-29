using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SAHL.Config.Core;
using SAHL.Config.Services.Core.Decorators;
using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using StructureMap;

namespace SAHL.Config.Services.Tests.Decorators
{
    [TestFixture]
    public class TestLoggingQueryHandlerDecorator
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
            var logger = Substitute.For<ILogger>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var decorator = new LoggingQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, logger, this.iocContainer);
            //---------------Test Result -----------------------
            Assert.AreSame(serviceQueryHandler, decorator.InnerQueryHandler);
        }

        [Test]
        public void HandleQuery_ShouldCallInnerHandler()
        {
            //---------------Set up test pack-------------------
            var serviceQueryHandler = Substitute.For<IServiceQueryHandler<TestQuery>>();
            var logger = Substitute.For<ILogger>();
            var decorator = new LoggingQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, logger, this.iocContainer);
            var query = new TestQuery();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            decorator.HandleQuery(query);
            //---------------Test Result -----------------------
            serviceQueryHandler.Received(1).HandleQuery(query);
        }

        [Test]
        public void HandleQuery_GivenExceptionIsThrown_ShouldLogExceptionAndThrowException()
        {
            //---------------Set up test pack-------------------
            const string testExceptionMsg = "Test Exception";
            var testQuery = new TestQuery();
            var thrownException = new Exception(testExceptionMsg);

            var serviceQueryHandler = Substitute.For<IServiceQueryHandler<TestQuery>>();
            serviceQueryHandler.HandleQuery(testQuery).Returns(info =>
                {
                    throw thrownException;
                });

            var logger = Substitute.For<ILogger>();
            var decorator = new LoggingQueryHandlerDecorator<TestQuery, TestQueryModel>(serviceQueryHandler, logger, this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => decorator.HandleQuery(testQuery));
            //---------------Test Result -----------------------
            Assert.AreEqual(testExceptionMsg, exception.Message);
            logger.Received(1).LogErrorWithException(Arg.Any<ILoggerSource>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                                                     thrownException, Arg.Any<IDictionary<string, object>>());
        }
    }
}