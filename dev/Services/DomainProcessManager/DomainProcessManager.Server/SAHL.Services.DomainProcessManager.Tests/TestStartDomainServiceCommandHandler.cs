using System;
using System.Data;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.DomainProcess;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.DomainProcessManager.CommandHandlers;
using SAHL.Services.DomainProcessManager.Services;
using SAHL.Services.Interfaces.DomainProcessManager;
using StructureMap;

namespace SAHL.Services.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestStartDomainServiceCommandHandler
    {
        private IIocContainer iocContainer;

        [TearDown]
        public void Teardown()
        {
            this.iocContainer = null;
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var commandHandler = new StartDomainServiceCommandHandler(container);
            //---------------Test Result -----------------------
            Assert.IsNotNull(commandHandler);
        }

        [TestCase("iocContainer")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<StartDomainServiceCommandHandler>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void HandleCommand_GivenNullCommand_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            this.SetupIocContainer();
            var commandHandler = new StartDomainServiceCommandHandler(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.HandleCommand(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("command", exception.ParamName);
        }

        [Test]
        public void HandleCommand_GivenCommand_ShouldNotThrowExceptionAndHaveNoReturnExceptionMessages()
        {
            //---------------Set up test pack-------------------
            this.SetupIocContainer(true);

            var dataModel = new FakeDomainModel();
            var command = new StartDomainProcessCommand(dataModel, typeof(FakeEvent1).Name);
            var commandHandler = new StartDomainServiceCommandHandler(this.iocContainer);
            ISystemMessageCollection commandResult = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => commandResult = commandHandler.HandleCommand(command));
            //---------------Test Result -----------------------
            Assert.IsNotNull(commandResult);
            Assert.IsFalse(commandResult.HasErrors);
            Assert.AreEqual(0, commandResult.ExceptionMessages().Count());
            Assert.AreEqual(0, commandResult.ErrorMessages().Count());
        }

        [Test]
        public void HandleCommand_GivenInvalidDomainProcess_ShouldAddErrorToSystemMessagesCollectionReturned()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance(Arg.Any<Type>()).Returns(null);

            var dataModel = new FakeDomainModel();
            var command = new StartDomainProcessCommand(dataModel, typeof(FakeEvent1).Name);
            var commandHandler = new StartDomainServiceCommandHandler(iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var systemMessageCollection = commandHandler.HandleCommand(command);
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsTrue(systemMessageCollection.HasErrors);
            Assert.AreEqual(1, systemMessageCollection.ExceptionMessages().Count());
            StringAssert.Contains("Unable to find the Domain Process", systemMessageCollection.ExceptionMessages().First().Message);
        }

        [Test]
        public void HandleCommand_GivenValidCommand_ShouldCallAddDomainProcessOnDomainProcessQueueService()
        {
            //---------------Set up test pack-------------------
            var queueService = Substitute.For<IDomainProcessCoordinatorService>();
            var dataModel = new FakeDomainModel();
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance(Arg.Any<Type>()).Returns(domainProcess);
            iocContainer.GetInstance<IDomainProcessCoordinatorService>().Returns(queueService);

            var command = new StartDomainProcessCommand(dataModel, typeof(FakeEvent1).Name);
            var commandHandler = new StartDomainServiceCommandHandler(iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var systemMessageCollection = commandHandler.HandleCommand(command);
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsFalse(systemMessageCollection.HasErrors);
            Assert.AreEqual(0, systemMessageCollection.ExceptionMessages().Count());
            queueService.Received(1).AddDomainProcess(domainProcess);
        }

        [Test]
        public void HandleCommand_GivenValidCommand_ShouldCallStartOnDomainProcess()
        {
            //---------------Set up test pack-------------------
            var dataModel = new FakeDomainModel();
            var domainProcess = this.CreateMockDomainProcess();
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance(typeof(IDomainProcess<FakeDomainModel>)).Returns(domainProcess);

            var command = new StartDomainProcessCommand(dataModel, typeof(FakeEvent1).Name);
            var commandHandler = new StartDomainServiceCommandHandler(iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var systemMessageCollection = commandHandler.HandleCommand(command);
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsFalse(systemMessageCollection.HasErrors);
            Assert.AreEqual(0, systemMessageCollection.ExceptionMessages().Count());
            domainProcess.Received(1).Start(dataModel, typeof(FakeEvent1).Name);
        }

        [Test]
        public void HandleCommand_GivenValidCommandThatStartReturnsFalse_ShouldSetreturnValuesInCommand()
        {
            //---------------Set up test pack-------------------
            this.SetupIocContainer(true, false);

            var dataModel = new FakeDomainModel();
            var command = new StartDomainProcessCommand(dataModel, typeof(FakeEvent1).Name);
            var commandHandler = new StartDomainServiceCommandHandler(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            commandHandler.HandleCommand(command);
            //---------------Test Result -----------------------
            Assert.IsFalse(command.Result.Result);
        }

        [Test]
        public void HandleCommand_GivenExceptionOccursInStart_ShouldReturnExceptionInSystemMessageCollection()
        {
            //---------------Set up test pack-------------------
            var domainProcess = Substitute.For<IDomainProcess<NewPurchaseApplicationCreationModel>>();
            domainProcess.Start(Arg.Any<IDataModel>(), Arg.Any<string>()).Returns(info =>
            {
                throw new Exception("Test exception");
            });

            var container = Substitute.For<IIocContainer>();
            container.GetInstance(Arg.Any<Type>()).Returns(domainProcess);

            var dataModel = new FakeDomainModel();
            var command = new StartDomainProcessCommand(dataModel, typeof(FakeEvent1).Name);
            var commandHandler = new StartDomainServiceCommandHandler(container);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var systemMessageCollection = commandHandler.HandleCommand(command);
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsTrue(systemMessageCollection.HasErrors);
            Assert.AreEqual(1, systemMessageCollection.ExceptionMessages().Count());
        }

        private void SetupIocContainer(bool createFakeDomainProcess = false, bool startResult = true, IDataModel startResultData = null)
        {
            ObjectFactory.Initialize(expression =>
            {
                expression.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });

                var rawLogger = Substitute.For<IRawLogger>();
                var loggerSource = Substitute.For<ILoggerSource>();
                var loggerAppSource = Substitute.For<ILoggerAppSource>();

                expression.For<IRawLogger>().Use(rawLogger);
                expression.For<ILoggerSource>().Use(loggerSource);
                expression.For<ILoggerAppSource>().Use(loggerAppSource);

                if (createFakeDomainProcess)
                {
                    expression.For<IDomainProcess<FakeDomainModel>>()
                        .OnCreationForAll(process =>
                        {
                            ((FakeDomainProcess)process).StartResult = startResult;
                            ((FakeDomainProcess)process).StartResultData = startResultData;
                        });
                }
            });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);

            this.SetupDbConnectionMocks();
        }

        private void SetupDbConnectionMocks(IReadWriteSqlRepository readWriteSqlRepository = null, IDdlRepository ddlRepository = null)
        {
            var dbConnection = Substitute.For<IDbConnection>();
            var connectionProvider = Substitute.For<IDbConnectionProvider>();
            connectionProvider.RegisterContext(Arg.Any<string>()).Returns(dbConnection);

            var repository = ddlRepository ?? Substitute.For<IDdlRepository>();
            var readWriteRepository = readWriteSqlRepository ?? Substitute.For<IReadWriteSqlRepository>();

            var sqlRepositoryFactory = Substitute.For<ISqlRepositoryFactory>();
            sqlRepositoryFactory.GetNewDdlRepository().Returns(repository);
            sqlRepositoryFactory.GetNewReadWriteRepository().Returns(readWriteRepository);

            var dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();
            dbConnectionProviderStorage.RegisterConnectionProvider(connectionProvider);

            DbContextConfiguration.Instance.DbConnectionProviderStorage = dbConnectionProviderStorage;
            DbContextConfiguration.Instance.RepositoryFactory = sqlRepositoryFactory;
        }

        private IDomainProcess CreateMockDomainProcess()
        {
            var domainProcess = Substitute.For<IDomainProcess<FakeDomainModel>>();
            return domainProcess;
        }
    }
}
