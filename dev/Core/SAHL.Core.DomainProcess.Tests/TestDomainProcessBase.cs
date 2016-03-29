using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using System;
using System.Threading.Tasks;

namespace SAHL.Core.DomainProcess.Tests
{
    [TestFixture]
    public class TestDomainProcessBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainProcess);
        }

        [Test]
        public void Start_GivenNullDataModel_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => domainProcess.Start(null, typeof(FakeEvent).Name));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("dataModel", exception.ParamName);
        }

        [Test]
        public void Start_GivenNullEventName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => domainProcess.Start(new FakeDataModel(), null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("eventToWaitFor", exception.ParamName);
        }

        [Test]
        public void Start_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var dataModel = new FakeDataModel();
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => domainProcess.Start(dataModel, typeof(FakeEvent).Name));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Start_GivenValidParameters_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var dataModel = new FakeDataModel();
            var eventName = typeof(FakeEvent).Name;
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            domainProcess.Start(dataModel, eventName);
            //---------------Test Result -----------------------
            Assert.AreSame(dataModel, domainProcess.DataModel);
            StringAssert.IsMatch(eventName, domainProcess.EventToWaitFor);
        }

        [Test]
        public async void Start_ShouldWaitForEvent()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var fakeEvent = new FakeEvent(DateTime.Now);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var startTask = domainProcess.Start(new FakeDataModel(), typeof(FakeEvent).Name);
            domainProcess.HandleEvent(fakeEvent, new ServiceRequestMetadata());
            //---------------Test Result -----------------------
            if (await Task.WhenAny(startTask, Task.Delay(2000)) != startTask)
            {
                Assert.Fail("Waited too long Domain Process Start");
            }
        }

        [Test]
        public void FireAndForgetEvent_ShouldBeUpdated_To_FiredAndForgotten()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var fakeEvent = new FakeEvent(DateTime.Now);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var startTask = domainProcess.Start(new FakeDataModel(), "FireAndForget");
            domainProcess.HandleEvent(fakeEvent, new ServiceRequestMetadata());
            //---------------Test Result -----------------------
            Assert.AreEqual("FiredAndForgotten", domainProcess.EventToWaitFor);
        }

        [Test]
        public void HandleEvent_GivenNoHandleMethodForEvent_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var unhandledEvent = new FakeUnhandledEvent(DateTime.Now);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => domainProcess.HandleEvent(unhandledEvent, new ServiceRequestMetadata()));
            //---------------Test Result -----------------------
            StringAssert.Contains("Domain Process Handler event not found", exception.Message);
        }

        [Test]
        public void HandleEvent_GivenExceptionAndNoExceptionMethod_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var processEvent = new FakeEvent2(DateTime.Now);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(() => domainProcess.HandleEvent(processEvent, new ServiceRequestMetadata()));
            //---------------Test Result -----------------------
            StringAssert.Contains("Fake Event 2 failed", exception.Message);
            StringAssert.Contains("Fake Event 2 failed", domainProcess.StatusReason);
            Assert.AreEqual(DomainProcessStatus.Error, domainProcess.Status);
        }

        [Test]
        public void HandleEvent_GivenExceptionAndExceptionMethod_ShouldNotThrowExceptionAndCallHandleExceptionMethod()
        {
            //---------------Set up test pack-------------------
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            var processEvent = new FakeEvent3(DateTime.Now);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => domainProcess.HandleEvent(processEvent, new ServiceRequestMetadata()));
            //---------------Test Result -----------------------
            Assert.IsTrue(domainProcess.ExceptionHandled);
        }

        [Test]
        public void Handle_GivenAnEventAndTheProcessComplete_ShouldRaiseCompletedEvent()
        {
            //---------------Set up test pack-------------------
            var eventCalled = false;
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();
            var domainProcess = new FakeDomainProcess(rawLogger, loggerSource, loggerAppSource);
            domainProcess.Completed += id => { eventCalled = true; };
            domainProcess.Start(new FakeDataModel(), typeof(FakeEvent).Name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            domainProcess.HandleEvent(new FakeFinalEvent(DateTime.Now), new ServiceRequestMetadata());
            //---------------Test Result -----------------------
            Assert.IsTrue(eventCalled);
        }

        private class FakeEvent : Event
        {
            public FakeEvent(DateTime date)
                : base(date)
            {
            }

            public FakeEvent(Guid id, DateTime date)
                : base(id, date)
            {
            }
        }

        private class FakeEvent2 : Event
        {
            public FakeEvent2(DateTime date)
                : base(date)
            {
            }

            public FakeEvent2(Guid id, DateTime date)
                : base(id, date)
            {
            }
        }

        private class FakeEvent3 : Event
        {
            public FakeEvent3(DateTime date)
                : base(date)
            {
            }

            public FakeEvent3(Guid id, DateTime date)
                : base(id, date)
            {
            }
        }

        private class FakeUnhandledEvent : Event
        {
            public FakeUnhandledEvent(DateTime date)
                : base(date)
            {
            }

            public FakeUnhandledEvent(Guid id, DateTime date)
                : base(id, date)
            {
            }
        }

        private class FakeFinalEvent : Event
        {
            public FakeFinalEvent(DateTime date)
                : base(date)
            {
            }

            public FakeFinalEvent(Guid id, DateTime date)
                : base(id, date)
            {
            }
        }

        private class FakeDataModel : IDataModel
        {
        }

        private class FakeDomainProcess : DomainProcessBase<FakeDataModel>
        {
            public FakeDomainProcess(IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
                : base(rawLogger, loggerSource, loggerAppSource)
            {
            }

            public new string EventToWaitFor
            {
                get { return base.EventToWaitFor; }
            }

            public bool ExceptionHandled { get; private set; }

            public void Handle(FakeEvent fakeEvent, IServiceRequestMetadata metadata)
            {
                //Fake Domain Process, empty by design
            }

            public void Handle(FakeEvent2 fakeEvent, IServiceRequestMetadata metadata)
            {
                throw new Exception("Fake Event 2 failed");
            }

            public void Handle(FakeEvent3 fakeEvent, IServiceRequestMetadata metadata)
            {
                throw new Exception("Fake Event 3 failed");
            }

            public void Handle(FakeFinalEvent fakeEvent, IServiceRequestMetadata metadata)
            {
                this.OnCompleted(this.DomainProcessId);
            }

            public void HandleException(FakeEvent3 fakeEvent, IServiceRequestMetadata metadata, Exception runtimeException)
            {
                this.ExceptionHandled = true;
            }

            public override void Initialise(IDataModel dataModel)
            {
                //Fake Domain Process, empty by design
            }

            public override void OnInternalStart()
            {
                //Fake Domain Process, empty by design
            }

            public override void RestoreState(IDataModel dataModel)
            {
                //Fake Domain Process, empty by design
            }

            public override void HandledEvent(IServiceRequestMetadata metadata)
            {
                //Fake Domain Process, empty by design
            }
        }
    }
}