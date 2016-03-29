using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Config.Services.Core.Decorators;

namespace SAHL.Config.Services.Tests.Decorators
{
    [TestFixture]
    public class TestLoggingCommandHandlerDecorator
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
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var commandHandler = Substitute.For<IServiceCommandHandler<TestCommand>>();
            var logger         = Substitute.For<ILogger>();
            var loggerSource   = Substitute.For<ILoggerSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var decorator = new LoggingCommandHandlerDecorator<TestCommand>(commandHandler, logger, this.iocContainer, loggerSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(decorator);
        }

        [Test]
        public void Constructor_GivenServiceQueryHandler_ShouldSetProperty()
        {
            //---------------Set up test pack-------------------
            var commandHandler = Substitute.For<IServiceCommandHandler<TestCommand>>();
            var logger         = Substitute.For<ILogger>();
            var loggerSource   = Substitute.For<ILoggerSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var decorator = new LoggingCommandHandlerDecorator<TestCommand>(commandHandler, logger, this.iocContainer, loggerSource);
            //---------------Test Result -----------------------
            Assert.AreSame(commandHandler, decorator.InnerCommandHandler);
        }

        [Test]
        public void HandleQuery_ShouldCallInnerHandler()
        {
            //---------------Set up test pack-------------------
            var commandHandler = Substitute.For<IServiceCommandHandler<TestCommand>>();
            var logger         = Substitute.For<ILogger>();
            var loggerSource   = Substitute.For<ILoggerSource>();
            var testCommand    = new TestCommand();
            var decorator      = new LoggingCommandHandlerDecorator<TestCommand>(commandHandler, logger, this.iocContainer, loggerSource);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            decorator.HandleCommand(testCommand, null);
            //---------------Test Result -----------------------
            commandHandler.Received(1).HandleCommand(testCommand, null);
        }

        [Test]
        public void HandleQuery_GivenExceptionIsThrown_ShouldLogExceptionAndThrowException()
        {
            //---------------Set up test pack-------------------
            const string testExceptionMsg = "Test Exception";
            var thrownException           = new Exception(testExceptionMsg);
            var testCommand               = new TestCommand();
            var commandHandler            = Substitute.For<IServiceCommandHandler<TestCommand>>();
            commandHandler.HandleCommand(testCommand, null).Returns(info =>
                {
                    throw thrownException;
                });

            var logger       = Substitute.For<ILogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var decorator    = new LoggingCommandHandlerDecorator<TestCommand>(commandHandler, logger, this.iocContainer, loggerSource);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => decorator.HandleCommand(testCommand, null));
            //---------------Test Result -----------------------
            Assert.AreEqual(testExceptionMsg, exception.Message);
            logger.Received(1).LogErrorWithException(loggerSource, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
                                                     thrownException, Arg.Any<IDictionary<string, object>>());
        }
    }
}
