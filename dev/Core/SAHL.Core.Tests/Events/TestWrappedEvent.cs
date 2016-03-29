using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Events;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Testing.Specs
{
    [TestFixture]
    public class TestWrappedEvent
    {
        [Test]
        public void Constructor()
        {
           
            //---------------Set up test pack-------------------
            var testEvent = new TestEvent(DateTime.Now);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wrappedEvent = new WrappedEvent<TestEvent>(Guid.NewGuid(), testEvent, null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(wrappedEvent);
        }

        [Test]
        public void Constructor_GivenEmptyEventId_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new WrappedEvent<TestEvent>(Guid.Empty, new TestEvent(DateTime.Now), null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("eventId", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullEvent_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new WrappedEvent<TestEvent>(Guid.NewGuid(), null, null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("internalEvent", exception.ParamName);
        }

        [Test]
        public void Properties_GivenValues_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var eventId = Guid.NewGuid();
            var metadata = Substitute.For<IServiceRequestMetadata>();
            var testEvent = new TestEvent(Guid.NewGuid(), DateTime.Now);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wrappedEvent = new WrappedEvent<TestEvent>(eventId, testEvent, metadata);
            //---------------Test Result -----------------------
            Assert.AreEqual(eventId, wrappedEvent.Id);
            Assert.AreSame(testEvent, wrappedEvent.InternalEvent);
            Assert.AreSame(metadata, wrappedEvent.ServiceRequestMetadata);
            StringAssert.IsMatch(testEvent.Name, wrappedEvent.Name);
        }

        private class TestEvent : Event
        {
            public TestEvent(DateTime date)
                : base(date)
            {
            }

            public TestEvent(Guid id, DateTime date)
                : base(id, date)
            {
            }
        }
    }
}