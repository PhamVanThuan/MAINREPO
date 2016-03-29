using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Events;
using SAHL.Core.Messaging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.EventPublisher.Services;
using SAHL.Services.EventPublisher.Services.Models;
using SAHL.Services.EventPublisher.Specs.EventTypeProviderServiceSpecs;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;

namespace SAHL.Services.EventPublisher.Specs
{
    public abstract class WithEventPublisherFakes : WithFakes
    {
        protected static ISystemMessageCollection messages;
        protected static IServiceRequestMetadata metadata;
        protected static IEventPublisherDataService eventPublisherDataService;
        protected static IEventSerialiser eventSerialiser;
        protected static IMessageBusAdvanced messageBus;
        protected static IEvent deserialisedEvent;
        protected static EventDataModel eventDataModel;
        protected static string eventXml;
        protected static Guid eventGuid;
        protected static IEvent eventModel;

        private Establish context = () =>
        {
            eventGuid = Guid.Parse("7b5da0fb-732a-4eef-aae5-a38d00da9f4e");
            eventModel = new FakeEvent(eventGuid, DateTime.Now, "StringValue", 1, FakeEnum.abc);
            eventPublisherDataService = An<IEventPublisherDataService>();
            eventSerialiser = An<IEventSerialiser>();
            messageBus = An<IMessageBusAdvanced>();
            messages = An<ISystemMessageCollection>();
            metadata = An<IServiceRequestMetadata>();
            eventXml = string.Format(@"<Event xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                  <ApplicationKey>1618078</ApplicationKey>
                  <ApplicationInformationKey>1701230</ApplicationInformationKey>
                  <AssignedADUser>SAHL\BeverlyJ</AssignedADUser>
                  <CommissionableADUser>SAHL\BeverlyJ</CommissionableADUser>
                  <AdUserKey>1618</AdUserKey>
                  <AdUserName>X2</AdUserName>
                  <Date>2014/08/02 12:23:21 PM</Date>
                  <GenericKey>1618078</GenericKey>
                  <GenericKeyTypeKey>2</GenericKeyTypeKey>
                  <Id>{0}</Id>
                  <Name>AppProgressInApplicationCapture</Name>
                  <ClassName>{1}</ClassName>
                </Event>", eventGuid, typeof(AppProgressInApplicationCaptureLegacyEvent).AssemblyQualifiedName);
            eventDataModel = new EventDataModel();
            eventDataModel.Data = eventXml;
            eventDataModel.Metadata = @"<ServiceRequestMetadata xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                  <item>
                    <Key>DomainProcessId</Key>
                    <Value>057039b6-fd16-4dc2-86e2-990927ed350b</Value>
                  </item>
                  <item>
                    <Key>CommandCorrelationId</Key>
                    <Value>6708b981-cc35-491e-b321-a46e00b4606c</Value>
                  </item>
                  <item>
                    <Key>username</Key>
                    <Value>SAHL\AppServices</Value>
                  </item>
                </ServiceRequestMetadata>";
            eventPublisherDataService.WhenToldTo(x => x.GetEventDataModelByEventKey(Arg.Any<int>())).Return(eventDataModel);
        };
    }
}
