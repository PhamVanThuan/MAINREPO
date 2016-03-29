using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ClientPendingDomiciliumAddress
{
    public class when_handling_exception : WithNewPurchaseDomainProcess
    {
        private static Exception runtimeException;
        private static ClientAddressAsPendingDomiciliumAddedEvent clientAddressAsPendingDomiciliiumAddedEvent;
        private static DomainProcessServiceRequestMetadata metadata;
        private static Guid commandCorrelationId;
        private static int clientAddressKey;
        private static string identityNumber;

        private Establish context = () =>
        {
            commandCorrelationId = Guid.Parse("{82E96FCE-A70F-464F-8B14-6BBB136E5AB7}");
            clientAddressKey = 44;
            clientAddressAsPendingDomiciliiumAddedEvent = new ClientAddressAsPendingDomiciliumAddedEvent(new DateTime(2014, 11, 15), clientAddressKey, 56);
            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            runtimeException = new Exception("Something went wrong!");
            identityNumber = "1234567890123";
            applicationStateMachine.WhenToldTo(x => x.ClientDomicilumAddressCollection).Return(
                new Dictionary<string, int> { { identityNumber, clientAddressKey } });
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(1234);
            clientDataManager.WhenToldTo(x => x.GetClientKeyForClientAddress(clientAddressKey)).Return(13);
            domainProcess.ProcessState = applicationStateMachine;
            applicationDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<LinkDomiciliumAddressToApplicantCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Throw(runtimeException);
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(commandCorrelationId);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(clientAddressAsPendingDomiciliiumAddedEvent, metadata);
        };

        private It should_fire_the_non_critical_error_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.NonCriticalErrorReported, commandCorrelationId));
        };

        private It should_record_that_the_event_was_received = () =>
        {
            applicationStateMachine.WasToldTo(x => x.RecordCommandFailed(commandCorrelationId));
        };

        private It should_add_an_exception_message = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ErrorMessages().Any(y => y.Message.Contains(identityNumber)) &&
                m.ExceptionMessages().Any(y => y.Message.Contains(runtimeException.ToString())))));
        };
    }
}