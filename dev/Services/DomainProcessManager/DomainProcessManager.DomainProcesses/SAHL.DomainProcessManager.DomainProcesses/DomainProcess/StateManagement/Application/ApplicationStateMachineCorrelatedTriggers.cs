using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement
{
    public partial class ApplicationStateMachine
    {
        public SerialisationFriendlyStateMachine<ApplicationState,ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid, int> BasicApplicationCreatedTrigger;

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicantAddedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid, int> EmploymentAddedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> SetApplicationEmploymentTypeTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicationEmploymentDeterminedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicationHouseHoldIncomeDeterminedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicationPricedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicationFundedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicationLinkedToExternalVendorTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> CriticalErrorOccuredTrigger;

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> BankAccountCapturedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicationDebitOrderCapturedTrigger;

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> AddressCapturedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ApplicationMailingAddressCapturedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ClientPendingDomiciliumCaptureTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> DomiciliumAddressCapturedTrigger;

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> ComcorpPropertyCapturedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> AffordabilityDetailsCapturedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> DeclarationsCapturedTrigger;
        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> AssetLiabilityCapturedTrigger;

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> NonCriticalErrorOccuredTrigger;

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> X2CaseCreationTrigger { get; set; }

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid> CompletedTrigger;

        private Dictionary<ApplicationStateTransitionTrigger, SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid>> paramTrigger 
            = new Dictionary<ApplicationStateTransitionTrigger, SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>.TriggerWithParameters<Guid>>();

        public void TriggerBasicApplicationCreated(Guid guid, int applicationNumber)
        {
            if (!statesBreadCrumb.ContainsKey(guid))
            {
                Machine.Fire(BasicApplicationCreatedTrigger, guid, applicationNumber);
            }
        }

        public void TriggerEmploymentAdded(Guid guid, int employmentKey)
        {
            if (!statesBreadCrumb.ContainsKey(guid))
            {
                Machine.Fire(EmploymentAddedTrigger, guid, employmentKey);
            }
        }

        public void FireStateMachineTrigger(ApplicationStateTransitionTrigger trigger, Guid guid)
        {
            if (!statesBreadCrumb.ContainsKey(guid))
            {
                Machine.Fire(paramTrigger[trigger], guid);
            }
        }

        private void SetupCommandCorrelatedTriggers(SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine)
        {
            BasicApplicationCreatedTrigger = machine.SetTriggerParameters<Guid, int>(ApplicationStateTransitionTrigger.BasicApplicationCreationConfirmed);

            ApplicantAddedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, ApplicantAddedTrigger);

            EmploymentAddedTrigger = machine.SetTriggerParameters<Guid, int>(ApplicationStateTransitionTrigger.EmploymentAdditionConfirmed);

            ApplicationEmploymentDeterminedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, ApplicationEmploymentDeterminedTrigger);

            ApplicationHouseHoldIncomeDeterminedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, ApplicationHouseHoldIncomeDeterminedTrigger);

            ApplicationPricedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, ApplicationPricedTrigger);

            ApplicationFundedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed, ApplicationFundedTrigger);

            ApplicationLinkedToExternalVendorTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicationLinkingToExternalVendorConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicationLinkingToExternalVendorConfirmed, ApplicationLinkedToExternalVendorTrigger);

            CriticalErrorOccuredTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.CriticalErrorReported);
            paramTrigger.Add(ApplicationStateTransitionTrigger.CriticalErrorReported, CriticalErrorOccuredTrigger);

            AddressCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.AddressCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, AddressCapturedTrigger);

            BankAccountCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, BankAccountCapturedTrigger);

            ApplicationDebitOrderCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, ApplicationDebitOrderCapturedTrigger);

            ApplicationMailingAddressCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationMailingAddressCapturedTrigger);

            AssetLiabilityCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, AssetLiabilityCapturedTrigger);

            ComcorpPropertyCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ComcorpPropertyCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ComcorpPropertyCaptureConfirmed, ComcorpPropertyCapturedTrigger);

            AffordabilityDetailsCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed, AffordabilityDetailsCapturedTrigger);

            DeclarationsCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.DeclarationsCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.DeclarationsCaptureConfirmed, DeclarationsCapturedTrigger);

            ClientPendingDomiciliumCaptureTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ClientPendingDomiciliumCaptureTrigger);

            DomiciliumAddressCapturedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, DomiciliumAddressCapturedTrigger);

            NonCriticalErrorOccuredTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.NonCriticalErrorReported);
            paramTrigger.Add(ApplicationStateTransitionTrigger.NonCriticalErrorReported, NonCriticalErrorOccuredTrigger);

            X2CaseCreationTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, X2CaseCreationTrigger);

            CompletedTrigger = machine.SetTriggerParameters<Guid>(ApplicationStateTransitionTrigger.CompletionConfirmed);
            paramTrigger.Add(ApplicationStateTransitionTrigger.CompletionConfirmed, CompletedTrigger);
        }
    }
}