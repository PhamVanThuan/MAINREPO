using System;
using System.Data;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.DomainProcess;
using SAHL.Core.Logging;
using SAHL.Core.Web.Services;
using SAHL.Services.DomainProcessManager.Data;

namespace SAHL.Services.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestDomainProcessRepository
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var jsonActivator = Substitute.For<IJsonActivator>();
            var iocContainer = Substitute.For<IIocContainer>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = new DomainProcessRepository(jsonActivator, iocContainer);
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
        }

        [Test]
        public void Add_GivenNullDomainProcess_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.Add((FakeDomainProcess)null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("domainProcess", exception.ParamName);
        }

        [Test]
        public void Add_GivenDomainProcess_ShouldInsertDomainProcessInDatabase()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var sqlRepository = Substitute.For<IReadWriteSqlRepository>();

            this.SetupDbConnectionMocks(sqlRepository);
            var repository = this.CreateRepository(false);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => repository.Add(domainProcess));
            //---------------Test Result -----------------------
            sqlRepository.Received(1).Insert(Arg.Any<DomainProcessDataModel>());
        }

        [Test]
        public void Update_GivenNullDomainProcess_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            FakeDomainProcess domainProcess = null;
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.Update(domainProcess));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("domainProcess", exception.ParamName);
        }

        [Test]
        public void Update_GivenDomainProcess_ShouldUpdateDomainProcessInDatabase()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var sqlRepository = Substitute.For<IReadWriteSqlRepository>();

            this.SetupDbConnectionMocks(sqlRepository);
            var repository = this.CreateRepository(false);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => repository.Update(domainProcess));
            //---------------Test Result -----------------------
            sqlRepository.Received(1).Update(Arg.Any<DomainProcessDataModel>());
        }

        [Test]
        public void Delete_GivenNullDomainProcess_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.Delete((FakeDomainProcess)null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("domainProcess", exception.ParamName);
        }

        [Test]
        public void Delete_GivenDomainProcess_ShouldDeleteDomainProcessInDatabase()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var sqlRepository = Substitute.For<IReadWriteSqlRepository>();

            this.SetupDbConnectionMocks(sqlRepository);
            var repository = this.CreateRepository(false);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => repository.Delete(domainProcess));
            //---------------Test Result -----------------------
            sqlRepository.Received(1).DeleteWhere<FakeDomainProcess>(Arg.Any<string>(), domainProcess.DomainProcessId);
        }

        [Test]
        public void Find_GivenNullDomainProcessId_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.Find<IDomainProcess>(Guid.Empty));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("domainProcessId", exception.ParamName);
        }

        [Test]
        public void Find_GivenDomainProcessDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);

            var readOnlyRepository = Substitute.For<IReadOnlySqlRepository>();
            readOnlyRepository.GetByKey<IDomainProcess, Guid>(Arg.Is(domainProcess.DomainProcessId)).Returns(info => null);

            this.SetupDbConnectionMocks(null, readOnlyRepository);
            var repository = this.CreateRepository(false);
            IDomainProcess returnValue = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => returnValue = repository.Find<IDomainProcess>(domainProcess.DomainProcessId));
            //---------------Test Result -----------------------
            Assert.IsNull(returnValue);
        }

        [Test]
        public void Find_GivenDomainProcessDoesExist_ShouldReturnDomainProcess()
        {
            //---------------Set up test pack-------------------
            var processState = new FakeDomainModel { Id = Guid.NewGuid() };
            var startResultData = new FakeDomainModel { Id = Guid.NewGuid() };
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource)
            {
                ProcessState = processState,
                StartResultData = startResultData,
            };
            var domainProcessDataModel = this.CreateDomainProcessDataModel(domainProcess);

            var readOnlyRepository = Substitute.For<IReadOnlySqlRepository>();
            readOnlyRepository.GetByKey<DomainProcessDataModel, Guid>(Arg.Is(domainProcess.DomainProcessId)).Returns(domainProcessDataModel);
            //jsonActivator.DeserializeObject < IDataModel >

            this.SetupDbConnectionMocks(null, readOnlyRepository);

            var repository = this.CreateRepository(false);
            IDomainProcess returnValue = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => returnValue = repository.Find<IDomainProcess>(domainProcess.DomainProcessId));
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(returnValue.DomainProcessId, domainProcess.DomainProcessId);
            Assert.IsNotNull(returnValue.ProcessState);
            Assert.IsNotNull(returnValue.StartResultData);

            var returnedProcessState = returnValue.ProcessState as FakeDomainModel;
            var returnedStartResultData = returnValue.StartResultData as FakeDomainModel;

            Assert.AreEqual(returnedProcessState.Id, processState.Id);
            Assert.AreEqual(returnedStartResultData.Id, startResultData.Id);
        }

        private DomainProcessRepository CreateRepository(bool setupDbConnectionMocks = true, IIocContainer iocContainer = null)
        {
            if (setupDbConnectionMocks)
            {
                this.SetupDbConnectionMocks();
            }

            var jsonActivator = new JsonActivator();

            if (iocContainer == null)
            {
                iocContainer = Substitute.For<IIocContainer>();
                var rawLogger = Substitute.For<IRawLogger>();
                var loggerSource = Substitute.For<ILoggerSource>();
                var loggerAppSource = Substitute.For<ILoggerAppSource>();
                iocContainer.GetInstance(typeof(FakeDomainProcess)).Returns(new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource));
            }

            var repository = new DomainProcessRepository(jsonActivator, iocContainer);
            return repository;
        }

        private DomainProcessDataModel CreateDomainProcessDataModel<T>(T domainProcess) where T : class, IDomainProcess
        {
            var jsonActivator = new JsonActivator();
            var domainProcessType = domainProcess.GetType().AssemblyQualifiedName;
            var processState = jsonActivator.SerializeObject(domainProcess.ProcessState);
            var dataModel = jsonActivator.SerializeObject(domainProcess.GetDataModel());
            var startResultData = jsonActivator.SerializeObject(domainProcess.StartResultData);

            var domainProcessDataModel = new DomainProcessDataModel(domainProcess.DomainProcessId,
                domainProcessType,
                processState,
                startResultData,
                (int)DomainProcessStatus.Processing,
                String.Empty,
                dataModel,
                domainProcess.DateCreated,
                domainProcess.DateModified);
            return domainProcessDataModel;
        }

        private void SetupDbConnectionMocks(IReadWriteSqlRepository readWriteSqlRepository = null, IReadOnlySqlRepository readOnlySqlRepository = null,
            IDdlRepository ddlRepository = null)
        {
            var dbConnection = Substitute.For<IDbConnection>();
            var connectionProvider = Substitute.For<IDbConnectionProvider>();
            connectionProvider.RegisterContext(Arg.Any<string>()).Returns(dbConnection);

            var repository = ddlRepository ?? Substitute.For<IDdlRepository>();
            var readWriteRepository = readWriteSqlRepository ?? Substitute.For<IReadWriteSqlRepository>();
            var readOnlyRepository = readOnlySqlRepository ?? Substitute.For<IReadOnlySqlRepository>();

            var sqlRepositoryFactory = Substitute.For<ISqlRepositoryFactory>();
            sqlRepositoryFactory.GetNewDdlRepository().Returns(repository);
            sqlRepositoryFactory.GetNewReadWriteRepository().Returns(readWriteRepository);
            sqlRepositoryFactory.GetNewReadOnlyRepository().Returns(readOnlyRepository);

            var dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();
            dbConnectionProviderStorage.RegisterConnectionProvider(connectionProvider);

            DbContextConfiguration.Instance.DbConnectionProviderStorage = dbConnectionProviderStorage;
            DbContextConfiguration.Instance.RepositoryFactory = sqlRepositoryFactory;
        }
    }
}
