using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_all_bank_accounts_captured : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            permittedTriggers = new List<ApplicationStateTransitionTrigger> {
                ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed,
                ApplicationStateTransitionTrigger.ApplicationPricingConfirmed,
                ApplicationStateTransitionTrigger.CriticalErrorReported
            };
            applicationStateMachine = new ApplicationStateMachine();
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            applicationCreationModel.Applicants.FirstOrDefault().BankAccounts = new List<BankAccountModel>
            {
                new BankAccountModel("051001", "STANDARD BANK SOUTH AFRICA", "302879056", ACBType.Current, "Account Name", "System", true),
                new BankAccountModel("051002", "STANDARD BANK SOUTH AFRICA", "302879057", ACBType.Current, "Account Name", "System", true)
            };
            applicationStateMachine = Common.getApplicationStateMachineWithCriticalPathCaptured(1232, applicationStateMachine, applicationCreationModel);
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, Guid.NewGuid());
        };

        private It should_transition_state_to_all_bank_accounts_captured_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.AllBankAccountsCaptured).ShouldBeTrue();
        };
    }
}