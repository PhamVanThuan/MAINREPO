﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_switch_throws_error_on_critical_path : WithDomainServiceMocks
    {
        private static SwitchApplicationDomainProcess domainProcess;
        private static SwitchApplicationCreationModel switchCreationModel;
        private static Exception thrownException;
        private static ISystemMessageCollection errorMessages;
        private static ApplicantModel applicant;
        private static SwitchApplicationAddedEvent switchApplicationAddedEvent;
        private static NaturalPersonClientAddedEvent naturalPersonClientAddedEvent;

        private static int applicationNumber, clientKey, employmentKey;

        private static Exception invalidTransitionException;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();

            domainProcess = new SwitchApplicationDomainProcess(
                                      applicationStateMachine
                                    , applicationDomainService
                                    , clientDomainService
                                    , addressDomainService
                                    , financialDomainService
                                    , bankAccountDomainService
                                    , combGuidGenerator
                                    , clientDataManager
                                    , x2WorkflowManager
                                    , linkedKeyManager
                                    , propertyDomainService
                                    , communicationManager
                                    , applicationDataManager
                                    , domainRuleManager
                                    , rawLogger
                                    , loggerSource
                                    , loggerAppSource
                               );
            switchCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan) as SwitchApplicationCreationModel;

            applicationNumber = 15006;
            clientKey = 554;
            employmentKey = 367;

            var testDate = new DateTime(2014, 12, 1);
            switchApplicationAddedEvent = DomainProcessTestHelper.GetSwitchApplicationAddedEvent(applicationNumber, switchCreationModel, testDate);

            applicant = switchCreationModel.Applicants.First();
            naturalPersonClientAddedEvent = DomainProcessTestHelper.GetNaturalPersonClientAddedEvent(clientKey, applicant, testDate);

            errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Something went wrong", SystemMessageSeverityEnum.Error));
            clientDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddNaturalPersonClientCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = null;
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            domainProcess.Start(switchCreationModel, typeof(SwitchApplicationAddedEvent).Name);

            var metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(switchApplicationAddedEvent, metadata));
        };

        private It should_add_a_new_purchase_application = () =>
        {
            applicationDomainService.WasToldTo(ads => ads.PerformCommand(
                Param<AddSwitchApplicationCommand>.Matches(m =>
                m.SwitchApplicationModel.ApplicationType == switchCreationModel.ApplicationType &&
                m.SwitchApplicationModel.EstimatedPropertyValue == switchCreationModel.EstimatedPropertyValue &&
                m.SwitchApplicationModel.ExistingLoan == switchCreationModel.ExistingLoan &&
                m.SwitchApplicationModel.CashOut == switchCreationModel.CashOut &&
                m.SwitchApplicationModel.OriginationSource == switchCreationModel.OriginationSource &&
                m.SwitchApplicationModel.Product == switchCreationModel.Product),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_go_into_critical_error_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.CriticalErrorOccured).ShouldBeTrue();
        };

        private It should_not_allow_moving_out_of_critical_error_state = () =>
        {
            invalidTransitionException = Catch.Exception(() => applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), employmentKey));
            invalidTransitionException.ShouldBeOfExactType(typeof(InvalidOperationException));
        };

        private It should_not_reverse_events_related_to_failed_application = () =>
        {
            applicationDataManager.WasToldTo(adm => adm.RollbackCriticalPathApplicationData(Param.Is<int>(applicationNumber), Param<IEnumerable<int>>.Matches(m => m.Count() == 0)));
        };

        private It should_throw_an_exception = () =>
        {
            thrownException.ShouldNotBeNull();
        };
    }
}