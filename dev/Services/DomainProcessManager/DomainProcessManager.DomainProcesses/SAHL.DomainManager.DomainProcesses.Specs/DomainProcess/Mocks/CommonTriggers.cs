using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks
{
    public static class Common
    {
        public static IApplicationStateMachine getApplicationStateMachineWithCriticalPathCaptured(int applicationNumber, IApplicationStateMachine applicationStateMachine, ApplicationCreationModel applicationCreationModel = null)
        {
            if (applicationCreationModel == null)
            {
                applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            }

            var totalApplicants = applicationCreationModel.Applicants.Count();
            var totalEmployments = applicationCreationModel.Applicants.Sum(x => x.Employments.Count());

            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
            for (int i = 0; i < totalApplicants; i++)
            {
                applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            }
            for (int i = 0; i < totalEmployments; i++)
            {
                applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), i);
            }
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationLinkingToExternalVendorConfirmed, Guid.NewGuid());
            return applicationStateMachine;
        }

        public static List<ApplicationStateTransitionTrigger> NonCriticalPathTriggers = new List<ApplicationStateTransitionTrigger> {
            ApplicationStateTransitionTrigger.ApplicationLinkingToExternalVendorConfirmed,
            ApplicationStateTransitionTrigger.ComcorpPropertyCaptureConfirmed,
            ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed,
            ApplicationStateTransitionTrigger.DeclarationsCaptureConfirmed,
            ApplicationStateTransitionTrigger.X2CaseCreationConfirmed,
            ApplicationStateTransitionTrigger.NonCriticalErrorReported,
            ApplicationStateTransitionTrigger.CompletionConfirmed
        };

        public static List<ApplicationStateTransitionTrigger> BankAccountPermittedTriggers = new List<ApplicationStateTransitionTrigger> {
           ApplicationStateTransitionTrigger.AllAddressesCaptured,
           ApplicationStateTransitionTrigger.AddressCaptureConfirmed,
           ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, 
           ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, 
           ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, 
           ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, 
        };

        public static List<ApplicationStateTransitionTrigger> AddressPermittedTriggers = new List<ApplicationStateTransitionTrigger> {
           ApplicationStateTransitionTrigger.AllBankAccountsCaptured,
           ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, 
           ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, 
        };


    }
}
