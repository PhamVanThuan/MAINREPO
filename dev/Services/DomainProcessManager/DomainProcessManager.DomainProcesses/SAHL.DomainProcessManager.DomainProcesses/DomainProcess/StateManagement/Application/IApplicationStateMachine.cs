using SAHL.Core.Data;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using Stateless;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement
{
    public interface IApplicationStateMachine : IDataModel
    {
        ISystemMessageCollection SystemMessages { get; }

        void AggregateMessages(ISystemMessageCollection systemMessages);

        int ApplicationNumber { get; }

        bool ContainsStateInBreadCrumb(ApplicationState state);

        IDictionary<string, int> ClientCollection { get; }

        SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> InitializeMachine(ApplicationState state);

        IDictionary<string, int> ClientDomicilumAddressCollection { get; }

        IDictionary<string, int> ClientDebitOrderBankAccountCollection { get; }

        void TriggerEmploymentAdded(Guid id, int employmentKey);

        SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> Machine { get; }

        int? MailingClientAddressKey { get; set; }

        IList<int> EmploymentKeys { get;}

        bool IsInState(ApplicationState state);

        bool ClientEmploymentsFullyCaptured();

        bool HasProcessCompletedWithCriticalPathFullyCaptured();

        void RecordEventReceived(Guid guid);

        void CreateStateMachine(ApplicationCreationModel applicationData, Guid domainProcessId);

        void FireStateMachineTrigger(ApplicationStateTransitionTrigger trigger, Guid guid);

        void TriggerBasicApplicationCreated(Guid guid, int applicationNumber);

        void RecordCommandSent(Type commandType, Guid commandCorrelationGuid);

        void RecordCommandFailed(Guid guid);

        void AdjustStateExpectations(ApplicationState state);

        bool AllLeadApplicantsHaveBeenAdded();
    }
}