using NSubstitute;
using NUnit.Framework;
using SAHL.Core.DomainProcess;
using SAHL.Core.Events;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Core.Testing;
using SAHL.Services.DomainProcessManager.Services;
using StructureMap;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestDomainProcessEventHandlerService
    {
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression => expression.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                }));
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var domainProcessCoordinatorService = Substitute.For<IDomainProcessCoordinatorService>();
            var messageBus = Substitute.For<IMessageBusAdvanced>();
            var domainProcesses = new List<IDomainProcess>();
            var logger = Substitute.For<IRawLogger>();
            var source = Substitute.For<ILoggerSource>();
            var appSource = Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = new DomainProcessEventHandlerService(domainProcessCoordinatorService, messageBus, domainProcesses, logger, source, appSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }

        [Test]
        public void Instance_ShouldImplementIStartable()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = this.CreateService();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<IStartable>(service);
        }

        [Test]
        public void Instance_ShouldImplementIStoppable()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = this.CreateService();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<IStoppable>(service);
        }

        [TestCase("domainProcessCoordinatorService")]
        [TestCase("messageBus")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<DomainProcessEventHandlerService>(parameterName);
            //---------------Test Result -----------------------
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
        public void Handle_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var service = this.CreateService();
            var eventData = new FakeEvent1(DateTime.Now);
            var wrappedEvent = new WrappedEvent<FakeEvent1>(Guid.NewGuid(), eventData, null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => service.Handle(wrappedEvent));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Handle_GivenAnEvent_ShouldCallHandleOnDomainProcessCoordinatorService()
        {
            //---------------Set up test pack-------------------
            var domainProcessCoordinatorService = Substitute.For<IDomainProcessCoordinatorService>();
            var service = this.CreateService(domainProcessCoordinatorService);
            var eventData = new FakeEvent1(DateTime.Now);
            var wrappedEvent = new WrappedEvent<FakeEvent1>(Guid.NewGuid(), eventData, null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            service.Handle(wrappedEvent);
            //---------------Test Result -----------------------
            domainProcessCoordinatorService.Received(1).HandleEvent(wrappedEvent);
        }

        [Test]
        public void Handle_GivenHandleEventThrowsException_ShouldCatchAndLogException()
        {
            //---------------Set up test pack-------------------
            var exceptionToThrow = new Exception("Encountered an error.");
            var domainProcessCoordinatorService = Substitute.For<IDomainProcessCoordinatorService>();
            domainProcessCoordinatorService.When(x => x.HandleEvent(Arg.Any<WrappedEvent<FakeEvent1>>()))
               .Do(x => { throw exceptionToThrow; });
            var service = this.CreateService(domainProcessCoordinatorService, null, null);
            var eventData = new FakeEvent1(DateTime.Now);
            var wrappedEvent = new WrappedEvent<FakeEvent1>(Guid.NewGuid(), eventData, null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => service.Handle(wrappedEvent));
            //---------------Test Result -----------------------

            rawLogger.Received().LogErrorWithException(Arg.Any<LogLevel>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<string>(msg => msg.Contains(exceptionToThrow.Message)), exceptionToThrow);
        }

        private IDomainProcessEventHandlerService CreateService(IDomainProcessCoordinatorService domainProcessCoordinatorService = null,
                                                                IMessageBusAdvanced messageBus = null,
                                                                IList<IDomainProcess> domainProcesses = null)
        {
            var coordinatorService = domainProcessCoordinatorService ?? Substitute.For<IDomainProcessCoordinatorService>();
            var bus = messageBus ?? Substitute.For<IMessageBusAdvanced>();

            rawLogger = rawLogger ?? Substitute.For<IRawLogger>();
            loggerSource = loggerSource ?? Substitute.For<ILoggerSource>();
            loggerAppSource = loggerAppSource ?? Substitute.For<ILoggerAppSource>();

            var processes = domainProcesses ?? new List<IDomainProcess> { new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource) };

            var service = new DomainProcessEventHandlerService(coordinatorService, bus, processes, rawLogger, loggerSource, loggerAppSource);
            return service;
        }
    }
}