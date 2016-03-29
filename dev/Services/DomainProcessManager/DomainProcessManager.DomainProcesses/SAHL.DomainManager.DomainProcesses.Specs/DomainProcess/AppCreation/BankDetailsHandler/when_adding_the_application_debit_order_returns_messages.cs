﻿using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.BankDetailsHandler
{
    public class when_adding_the_application_debit_order_returns_messages : WithNewPurchaseDomainProcess
    {
        private static BankAccountLinkedToClientEvent @event;
        private static int applicationNumber;
        private static int clientKey, clientBankAccountKey;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 5656;
            clientBankAccountKey = 54;

            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            var bankAccount = applicationCreationModel.Applicants.First().BankAccounts.First();
            @event = new BankAccountLinkedToClientEvent(new DateTime(2014, 10, 10), 123, clientKey, clientBankAccountKey, bankAccount.AccountName,
                                                        bankAccount.AccountNumber, bankAccount.BranchCode, bankAccount.BranchName);

            var clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };
            var clientBankAccountKeyDebitOrderCollection = new Dictionary<string, int>();

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientDebitOrderBankAccountCollection).Return(clientBankAccountKeyDebitOrderCollection);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllBankAccountsCaptured)).Return(true);

            var serviceMessages = SystemMessageCollection.Empty();
            serviceMessages.AddMessage(new SystemMessage("something bad happened!", SystemMessageSeverityEnum.Error));
            applicationDomainService
                .WhenToldTo(x => x.PerformCommand(Param.IsAny<AddApplicationDebitOrderCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(serviceMessages);
            applicationStateMachine
                .WhenToldTo(x => x.Machine)
                .Return(new SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>(ApplicationState.Processing));
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(@event, serviceRequestMetadata);
        };

        private It should_add_application_debit_order_details = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<AddApplicationDebitOrderCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_fire_bank_account_capture_confirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed), Param.IsAny<Guid>()));
        };

        private It should_fire_the_non_critical_error_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.NonCriticalErrorReported), Param.IsAny<Guid>()));
        };
    }
}