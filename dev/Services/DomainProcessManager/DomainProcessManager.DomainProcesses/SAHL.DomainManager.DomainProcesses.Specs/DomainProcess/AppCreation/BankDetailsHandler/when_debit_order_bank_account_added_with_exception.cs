using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.BankDetailsHandler
{
    public class when_debit_order_bank_account_added_with_exception : WithNewPurchaseDomainProcess
    {
        private static BankAccountLinkedToClientEvent bankAccountLinkedToClientEvent;
        private static int applicationNumber;
        private static int clientKey, clientBankAccountKey;
        private static ApplicationDebitOrderModel debitOrder;
        private static Exception thrownException, runtimeException;
        private static string friendlyErrorMessage;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 5656;
            clientBankAccountKey = 54;
            runtimeException = new Exception("an error occured.");

            friendlyErrorMessage = "Application Debit Order could not be saved.";
            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            var bankAccount = applicationCreationModel.Applicants.First().BankAccounts.First();
            bankAccountLinkedToClientEvent = new BankAccountLinkedToClientEvent(new DateTime(2014, 10, 10), 123, clientKey, clientBankAccountKey, 
                        bankAccount.AccountName, bankAccount.AccountNumber, bankAccount.BranchCode, bankAccount.BranchName);

            debitOrder = applicationCreationModel.ApplicationDebitOrder;

            var clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };
            var clientBankAccountKeyDebitOrderCollection = new Dictionary<string, int>();

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientDebitOrderBankAccountCollection).Return(clientBankAccountKeyDebitOrderCollection);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllBankAccountsCaptured)).Return(true);
            applicationDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddApplicationDebitOrderCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                                                    .Throw(runtimeException);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.Handle(bankAccountLinkedToClientEvent, serviceRequestMetadata));
        };

        private It should_fire_bank_account_capture_confirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed), Param.IsAny<Guid>()));
        };

        private It should_fire_the_non_critical_error_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.NonCriticalErrorReported), Param.IsAny<Guid>()));
        };

        private It should_contain_an_error_message = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ErrorMessages().Any(y => y.Message.Contains(friendlyErrorMessage)) &&
                m.ExceptionMessages().Any(y => y.Message.Contains(runtimeException.ToString())))));
        };

        private It should_log_the_error_message = () =>
        {
            rawLogger.WasToldTo(x => x.LogError(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()
             , Param<string>.Matches(m => m.Contains(friendlyErrorMessage)), null));
        };

        private It should_not_throw_an_exception = () =>
        {
            thrownException.ShouldBeNull();
        };
    }
}