using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.DomainProcess;
using SAHL.Core.Events;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.DomainProcessManager.Data;
using SAHL.Services.DomainProcessManager.Services;

namespace SAHL.Services.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestDomainProcessCoordinatorService
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IDomainProcessRepository>();
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = new DomainProcessCoordinatorService(repository, rawLogger, loggerSource, loggerAppSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }

        [TestCase("domainProcessRepository")]
        [TestCase("rawLogger")]
        [TestCase("loggerSource")]
        [TestCase("loggerAppSource")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<DomainProcessCoordinatorService>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Instance_ShouldImplementIStartable()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IDomainProcessRepository>();
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = new DomainProcessCoordinatorService(repository, rawLogger, loggerSource, loggerAppSource);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IStartable), service);
        }

        [Test]
        public void Instance_ShouldImplementIStoppable()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IDomainProcessRepository>();
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = new DomainProcessCoordinatorService(repository, rawLogger, loggerSource, loggerAppSource);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IStoppable), service);
        }

        [Test]
        public void Start_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var service = this.CreateService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(service.Start);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Stop_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var service = this.CreateService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(service.Stop);
            //---------------Test Result -----------------------
        }

        [Test]
        public void AddDomainProcess_GivenNullDomainProcess_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var service = this.CreateService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => service.AddDomainProcess(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("domainProcess", exception.ParamName);
        }

        [Test]
        public void AddDomainProcess_GivenValidDomainProcess_ShouldAddDomainProcessToQueue()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var service = this.CreateService(true) as FakeDomainProcessCoordinatorService;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, service.DomainProcessesCache.Count);
            //---------------Execute Test ----------------------
            service.AddDomainProcess(domainProcess);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, service.DomainProcessesCache.Count);
            Assert.IsTrue(service.DomainProcessesCache.ContainsKey(domainProcess.DomainProcessId));
        }

        [Test]
        public void AddDomainProcess_GivenDomainProcess_ShouldAddDomainProcessToDatabase()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var repository = Substitute.For<IDomainProcessRepository>();
            repository.Find<IDomainProcess>(domainProcess.DomainProcessId).Returns(info => null);
            var service = this.CreateService(domainProcessRepository: repository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            service.AddDomainProcess(domainProcess);
            //---------------Test Result -----------------------
            repository.Received(1).Add(domainProcess);
        }

        [Test]
        public void AddDomainProcess_GivenExistingDomainProcess_ShouldNotAddDomainProcessToDatabase()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var repository = Substitute.For<IDomainProcessRepository>();
            repository.Find<IDomainProcess>(domainProcess.DomainProcessId).Returns(domainProcess);

            var service = this.CreateService(domainProcessRepository: repository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            service.AddDomainProcess(domainProcess);
            //---------------Test Result -----------------------
            repository.DidNotReceive().Add(domainProcess);
        }

        [Test]
        public void FindDomainProcess_GivenEmptyGuid_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IDomainProcessRepository>();
            Guid domainProcessId = Guid.Empty;
            IDomainProcess nullDomainProcess = null;
            repository.Find<IDomainProcess>(domainProcessId).Returns(nullDomainProcess);

            var service = this.CreateService(false, repository);

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => service.FindDomainProcess(domainProcessId));
            //---------------Test Result -----------------------
            Assert.AreEqual("domainProcessId", exception.ParamName);
        }

        [Test]
        public void FindDomainProcess_GivenGuidThatDoesNotExists_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------

            var repository = Substitute.For<IDomainProcessRepository>();
            Guid domainProcessId = Guid.NewGuid();
            IDomainProcess nullDomainProcess = null;
            repository.Find<IDomainProcess>(domainProcessId).Returns(nullDomainProcess);

            var service = this.CreateService(false, repository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var domainProcess = service.FindDomainProcess(domainProcessId);
            //---------------Test Result -----------------------
            Assert.IsNull(domainProcess);
        }

        [Test]
        public void FindDomainProcess_GivenGuidDoesNotExists_ShouldReturnDomainProcess()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var service = this.CreateService(true) as FakeDomainProcessCoordinatorService;
            service.AddDomainProcess(domainProcess);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var foundDomainProcess = service.FindDomainProcess(domainProcess.DomainProcessId);
            //---------------Test Result -----------------------
            Assert.IsNotNull(foundDomainProcess);
            Assert.AreSame(foundDomainProcess, domainProcess);
            Assert.AreEqual(foundDomainProcess.DomainProcessId, domainProcess.DomainProcessId);
        }

        [Test]
        public void HandleEvent_GivenNullDomainProcessEvent_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            WrappedEvent<FakeEvent1> @event = null;
            var service = this.CreateService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => service.HandleEvent(@event));
            //---------------Test Result -----------------------
            Assert.AreEqual("domainProcessEvent", exception.ParamName);
        }

        [Test]
        public void HandleEvent_GivenDomainProcessEvent_AndDomainProcessDoesNotExistShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IDomainProcessRepository>();
            Guid domainProcessId = Guid.NewGuid();
            IDomainProcess nullDomainProcess = null;
            repository.Find<IDomainProcess>(domainProcessId).Returns(nullDomainProcess);

            var service = this.CreateService(false, repository);
            var @event = new FakeEvent1(DateTime.Now);
            var metadata = new ServiceRequestMetadata
            {
                { CoreGlobals.DomainProcessIdName, domainProcessId.ToString() }
            };

            var wrappedEvent = new WrappedEvent<FakeEvent1>(domainProcessId, @event, metadata);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => service.HandleEvent(wrappedEvent));
            //---------------Test Result -----------------------
            StringAssert.Contains("Domain Process not found", exception.Message);
        }

        [Test]
        public void HandleEvent_GivenNoHandleMethodForEvent_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var @event = new FakeEvent3(DateTime.Now);
            var metadata = new ServiceRequestMetadata
            {
                { CoreGlobals.DomainProcessIdName, domainProcess.DomainProcessId.ToString() }
            };

            var wrappedEvent = new WrappedEvent<FakeEvent3>(Guid.NewGuid(), @event, metadata);
            var service = this.CreateService();
            service.AddDomainProcess(domainProcess);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => service.HandleEvent(wrappedEvent));
            //---------------Test Result -----------------------
            Trace.WriteLine(exception.Message);
            StringAssert.Contains("Domain Process Handler event not found", exception.Message);
        }

        [Test]
        public void HandleEvent_GivenHandleMethodForEvent_ShouldCallHandleMethod()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var @event = new FakeEvent1(DateTime.Now);
            var metadata = new ServiceRequestMetadata
            {
                { CoreGlobals.DomainProcessIdName, domainProcess.DomainProcessId.ToString() }
            };

            var wrappedEvent = new WrappedEvent<FakeEvent1>(Guid.NewGuid(), @event, metadata);
            var service = this.CreateService();
            service.AddDomainProcess(domainProcess);
            domainProcess.Start(Substitute.For<IDataModel>(), "SomeEventName");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            service.HandleEvent(wrappedEvent);
            //---------------Test Result -----------------------
            Assert.IsTrue(domainProcess.Event1Completed);
        }

        [Test]
        public void Completed_GivenCompletedEvent_ShouldRemoveDomainProcessFromCacheAndUpdateDatabase()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var @event = new FakeEvent2(DateTime.Now);

            var repository = Substitute.For<IDomainProcessRepository>();
            repository.Find<IDomainProcess>(domainProcess.DomainProcessId).Returns(info => null);

            var metadata = new ServiceRequestMetadata
            {
                { CoreGlobals.DomainProcessIdName, domainProcess.DomainProcessId.ToString() }
            };

            var wrappedEvent = new WrappedEvent<FakeEvent2>(Guid.NewGuid(), @event, metadata);

            var service = this.CreateService(createFake: true, domainProcessRepository: repository) as FakeDomainProcessCoordinatorService;
            service.AddDomainProcess(domainProcess);
            domainProcess.Start(Substitute.For<IDataModel>(), "SomeEventName");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, service.DomainProcessesCache.Count);
            //---------------Execute Test ----------------------
            service.HandleEvent(wrappedEvent);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, service.DomainProcessesCache.Count);
            repository.Received().Update(Arg.Is<IDomainProcess>(process =>
                process.DomainProcessId == domainProcess.DomainProcessId &&
                    process.Status == Core.BusinessModel.Enums.DomainProcessStatus.Complete));
        }

        [Test]
        public void ErrorOccurred_GivenErrorOccurredEvent_ShouldRemoveDomainProcessFromCacheAndUpdateDatabase()
        {
            //---------------Set up test pack-------------------
            var rawLogger = Substitute.For<IRawLogger>();
            var loggerSource = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var @event = new FakeEvent4(DateTime.Now);

            var repository = Substitute.For<IDomainProcessRepository>();
            repository.Find<IDomainProcess>(domainProcess.DomainProcessId).Returns(info => null);

            var metadata = new ServiceRequestMetadata
            {
                { CoreGlobals.DomainProcessIdName, domainProcess.DomainProcessId.ToString() }
            };

            var wrappedEvent = new WrappedEvent<FakeEvent4>(Guid.NewGuid(), @event, metadata);

            var service = this.CreateService(createFake: true, domainProcessRepository: repository) as FakeDomainProcessCoordinatorService;
            service.AddDomainProcess(domainProcess);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, service.DomainProcessesCache.Count);
            //---------------Execute Test ----------------------
            Assert.Throws<Exception>(() => service.HandleEvent(wrappedEvent));
            //---------------Test Result -----------------------
            Assert.AreEqual(0, service.DomainProcessesCache.Count);
            repository.Received().Update(Arg.Is<IDomainProcess>(process =>
                process.DomainProcessId == domainProcess.DomainProcessId &&
                    process.Status == Core.BusinessModel.Enums.DomainProcessStatus.Error));
        }

        private IDomainProcessCoordinatorService CreateService(bool createFake = false,
            IDomainProcessRepository domainProcessRepository = null,
            IRawLogger rawLogger = null, ILoggerSource loggerSource = null, ILoggerAppSource loggerAppSource = null)
        {
            var repository = domainProcessRepository ?? Substitute.For<IDomainProcessRepository>();
            var logger = rawLogger ?? Substitute.For<IRawLogger>();
            var source = loggerSource ?? Substitute.For<ILoggerSource>();
            var appSource = loggerAppSource ?? Substitute.For<ILoggerAppSource>();

            return createFake
                ? new FakeDomainProcessCoordinatorService(repository, logger, source, appSource)
                : new DomainProcessCoordinatorService(repository, logger, source, appSource);
        }

        private class FakeDomainProcessCoordinatorService : DomainProcessCoordinatorService
        {
            public FakeDomainProcessCoordinatorService(IDomainProcessRepository domainProcessRepository, IRawLogger rawLogger,
                ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
                : base(domainProcessRepository, rawLogger, loggerSource, loggerAppSource)
            {
            }

            public new ConcurrentDictionary<Guid, IDomainProcess> DomainProcessesCache
            {
                get { return base.DomainProcessesCache; }
            }
        }
    }
}
