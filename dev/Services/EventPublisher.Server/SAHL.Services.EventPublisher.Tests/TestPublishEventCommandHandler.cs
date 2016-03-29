using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Messaging;
using SAHL.Core.SystemMessages;
using SAHL.Services.EventPublisher.CommandHandlers;
using SAHL.Services.EventPublisher.Services;
using SAHL.Services.EventPublisher.Services.Models;
using SAHL.Services.EventPublisher.Services.Statements;
using SAHL.Services.Interfaces.EventPublisher.Commands;

namespace SAHL.Services.EventPublisher.Tests
{
    [TestFixture]
    public class TestPublishEventCommandHandler
    {
        private const string ValidEventXml = @"<Event xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>7b5da0fb-732a-4eef-aae5-a38d00da9f4e</Id>
  <Date>2014/08/02 12:23:21 PM</Date>
  <Name>TestEvent</Name>
  <Message>12345</Message>
  <SomeId>7b5da0fb-732a-4eef-aae5-a38d00da9f4e</SomeId>
  <ClassName>SAHL.Services.EventPublisher.Tests.TestEvent, SAHL.Services.EventPublisher.Tests, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null</ClassName>
</Event>";

        private const string InvalidEventXml = @"<Event xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>1</Id>
  <Date>2014/08/02 12:23:21 PM</Date>
  <Name>TestEvent</Name>
  <Message>12345</Message>
  <SomeId>7b5da0fb-732a-4eef-aae5-a38d00da9f4e</SomeId>
  <ClassName>SAHL.Services.EventPublisher.Tests.TestEvent, SAHL.Services.EventPublisher.Tests, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null</ClassName>
</Event>";

        private const string InvalidClassTypeEventXml = @"<Event xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>7b5da0fb-732a-4eef-aae5-a38d00da9f4e</Id>
  <Date>2014/08/02 12:23:21 PM</Date>
  <Name>TestEvent</Name>
  <Message>12345</Message>
  <SomeId>7b5da0fb-732a-4eef-aae5-a38d00da9f4e</SomeId>
  <ClassName>SAHL.Services.EventPublisher.Tests.TestEvent2, SAHL.Services.EventPublisher.Tests, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null</ClassName>
</Event>";

        private const string NoClassNameEventXml = @"<Event xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>7b5da0fb-732a-4eef-aae5-a38d00da9f4e</Id>
  <Date>2014/08/02 12:23:21 PM</Date>
  <Name>TestEvent</Name>
  <Message>12345</Message>
  <SomeId>7b5da0fb-732a-4eef-aae5-a38d00da9f4e</SomeId>
</Event>";

        private const string ValidEventData =
            @"<ServiceRequestMetadata xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">"
                + @"<item><Key>SomeKey</Key><Value>SomeValue</Value></item>"
                + @"<item><Key>username</Key><Value /></item>"
                + @"</ServiceRequestMetadata>";

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var eventPublisherDataService = Substitute.For<IEventPublisherDataService>();
            var eventSerialiser = Substitute.For<IEventSerialiser>();
            var messageBusAdvanced = Substitute.For<IMessageBusAdvanced>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var commandHandler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBusAdvanced);
            //---------------Test Result -----------------------
            Assert.IsNotNull(commandHandler);
        }

        [Test]
        public void HandleCommand_GivenNoEventData_ShouldAddErrorToMessagesCollection()
        {
            //---------------Set up test pack-------------------
            var dbFactory = Substitute.For<IDbFactory>();
            dbFactory.NewDb().InReadOnlyAppContext().SelectOne<EventDataModel>(Arg.Any<GetEventDataStatement>()).Returns(info => null);
            var command = new PublishEventCommand(0);
            var eventPublisherDataService = new EventPublisherDataService(dbFactory);
            var eventSerialiser = Substitute.For<IEventSerialiser>();
            var messageBusAdvanced = Substitute.For<IMessageBusAdvanced>();

            var commandHandler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBusAdvanced);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messages = commandHandler.HandleCommand(command, null);
            //---------------Test Result -----------------------
            Assert.IsTrue(messages.HasErrors);
            StringAssert.Contains("Could not get event data", messages.AllMessages.First().Message);
            Assert.AreEqual(SystemMessageSeverityEnum.Error, messages.AllMessages.First().Severity);
        }

        [Test]
        public void HandleCommand_GivenNoClassNameInEventData_ShouldAddErrorToMessagesCollection()
        {
            //---------------Set up test pack-------------------
            var eventDataModel = new EventDataModel
            {
                Data = NoClassNameEventXml,
            };
            var dbFactory = Substitute.For<IDbFactory>();
            dbFactory.NewDb().InReadOnlyAppContext().SelectOne(Arg.Any<GetEventDataStatement>()).Returns(eventDataModel);
            var command = new PublishEventCommand(0);

            var eventPublisherDataService = new EventPublisherDataService(dbFactory);
            var eventSerialiser = Substitute.For<IEventSerialiser>();
            var messageBusAdvanced = Substitute.For<IMessageBusAdvanced>();

            var commandHandler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBusAdvanced);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messages = commandHandler.HandleCommand(command, null);
            //---------------Test Result -----------------------
            Assert.IsTrue(messages.HasErrors);
            StringAssert.Contains("Event xml does not contain a ClassName element", messages.AllMessages.First().Message);
            Assert.AreEqual(SystemMessageSeverityEnum.Error, messages.AllMessages.First().Severity);
        }

        [Test]
        public void HandleCommand_GivenInvalidClassTypeInEventData_ShouldAddErrorToMessagesCollection()
        {
            //---------------Set up test pack-------------------
            var eventDataModel = new EventDataModel
            {
                Data = InvalidClassTypeEventXml,
            };
            var dbFactory = Substitute.For<IDbFactory>();
            dbFactory.NewDb().InReadOnlyAppContext().SelectOne(Arg.Any<GetEventDataStatement>()).Returns(eventDataModel);

            var command = new PublishEventCommand(0);

            var eventPublisherDataService = new EventPublisherDataService(dbFactory);
            var eventSerialiser = new EventSerialiser();
            var messageBusAdvanced = Substitute.For<IMessageBusAdvanced>();

            var commandHandler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBusAdvanced);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messages = commandHandler.HandleCommand(command, null);
            //---------------Test Result -----------------------
            Assert.IsTrue(messages.HasErrors);
            StringAssert.Contains("Unknown event type:", messages.AllMessages.First().Message);
            Assert.AreEqual(SystemMessageSeverityEnum.Error, messages.AllMessages.First().Severity);
        }

        [Test]
        public void HandleCommand_GivenInvalidEvent_ShouldAddErrorToMessagesCollection()
        {
            //---------------Set up test pack-------------------
            var eventDataModel = new EventDataModel
            {
                Data = InvalidEventXml,
            };
            var dbFactory = Substitute.For<IDbFactory>();
            dbFactory.NewDb().InReadOnlyAppContext().SelectOne<EventDataModel>(Arg.Any<GetEventDataStatement>()).Returns(eventDataModel);
            var command = new PublishEventCommand(0);

            var eventPublisherDataService = new EventPublisherDataService(dbFactory);
            var eventSerialiser = new EventSerialiser();
            var messageBusAdvanced = Substitute.For<IMessageBusAdvanced>();

            var commandHandler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBusAdvanced);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            var messages = commandHandler.HandleCommand(command, null);

            //---------------Test Result -----------------------
            Assert.IsTrue(messages.HasErrors);
            Assert.AreEqual(1,
                messages.ErrorMessages().Count(),
                string.Format("Expected 1 error message but recieved {0}", messages.ErrorMessages().Count()));
        }

        [Test]
        public void HandleCommand_GivenValidEvent_ShouldHaveNoErrors()
        {
            //---------------Set up test pack-------------------
            var eventDataModel = new EventDataModel
            {
                Data = ValidEventXml,
                Metadata = ValidEventData
            };
            var dbFactory = Substitute.For<IDbFactory>();
            dbFactory.NewDb().InReadOnlyAppContext().SelectOne<EventDataModel>(Arg.Any<GetEventDataStatement>()).Returns(eventDataModel);

            var command = new PublishEventCommand(0);

            var eventPublisherDataService = new EventPublisherDataService(dbFactory);
            var eventSerialiser = new EventSerialiser();
            var messageBusAdvanced = Substitute.For<IMessageBusAdvanced>();

            var commandHandler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBusAdvanced);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messages = commandHandler.HandleCommand(command, null);
            //---------------Test Result -----------------------
            Assert.IsFalse(messages.HasErrors);
        }

        [Test]
        public void HandleCommand_GivenValidEvent_ShouldPublishWrappedEvent()
        {
            //---------------Set up test pack-------------------
            var eventDataModel = new EventDataModel
            {
                Data = ValidEventXml,
                Metadata = ValidEventData
            };
            var dbFactory = Substitute.For<IDbFactory>();
            dbFactory.NewDb().InReadOnlyAppContext().SelectOne(Arg.Any<GetEventDataStatement>()).Returns(eventDataModel);

            var command = new PublishEventCommand(0);

            object publishedEvent = null;

            var eventPublisherDataService = new EventPublisherDataService(dbFactory);
            var eventSerialiser = new EventSerialiser();
            var messageBusAdvanced = Substitute.For<IMessageBusAdvanced>();

            messageBusAdvanced.WhenForAnyArgs(advanced => advanced.Publish(Arg.Any<IWrappedEvent<IEvent>>()))
                .Do(info => publishedEvent = info.Arg<IWrappedEvent<IEvent>>());

            var commandHandler = new PublishEventCommandHandler(eventPublisherDataService, eventSerialiser, messageBusAdvanced);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messages = commandHandler.HandleCommand(command, null);
            //---------------Test Result -----------------------
            Assert.IsFalse(messages.HasErrors);
            messageBusAdvanced.Received(1).Publish(Arg.Any<IWrappedEvent<IEvent>>());
            Assert.IsNotNull(publishedEvent);
            Assert.IsInstanceOf(typeof(WrappedEvent<TestEvent>), publishedEvent);
        }
    }
}
