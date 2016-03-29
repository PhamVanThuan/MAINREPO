using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_all_bank_acc_substates_events_have_been_received : WithFakes
    {
        private static Guid guid;
        private static int applicationNumber;
        private static ApplicationStateMachine applicationStateMachine;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static IRawLogger rawLogger;
        private static ILoggerAppSource loggerAppSource;
        private static ILoggerSource loggerSource;

        private Establish context = () =>
        {
            applicationNumber = 1324;
            guid = Guid.NewGuid();
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            var firstApplicant = applicationCreationModel.Applicants.First();
            var secondApplicant = ApplicationCreationTestHelper.PopulateApplicantModel(new List<AddressModel>());
            secondApplicant.BankAccounts = new List<BankAccountModel> { ApplicationCreationTestHelper.PopulateBankAccountModel() };
            applicationCreationModel.Applicants = new List<ApplicantModel> { firstApplicant, secondApplicant };

            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();
            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine, applicationCreationModel);
        };

        private Because of = () =>
        {
            applicationStateMachine.Machine.Fire(applicationStateMachine.BankAccountCapturedTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.BankAccountCapturedTrigger, Guid.NewGuid());
        };

        private It should_auto_transition_to_parent = () =>
        {
            applicationStateMachine.Machine.State.ShouldEqual(ApplicationState.AllBankAccountsCaptured);
        };

        private It should_be_able_to_fire_debit_order = () =>
        {
            applicationStateMachine.Machine.CanFire(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed).ShouldBeTrue();
        };
    }
}