﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
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

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.UnemployedEmploymentHandler
{
    public class when_determining_household_income_and_service_errors : WithNewPurchaseDomainProcess
    {
        private static UnconfirmedUnemployedEmploymentAddedToClientEvent unemployedEmploymentAddedEvent;
        private static int applicationNumber, employmentKey;
        private static Exception thrownException;
        private static SystemMessageCollection errorMessages;

        private Establish context = () =>
        {
            applicationNumber = 150;
            employmentKey = 673;
            domainProcess.ProcessState = applicationStateMachine;
            var employer = new Services.Interfaces.ClientDomain.Models.EmployerModel(13, "Unemployed", "031", "11224455", null, null, EmployerBusinessType.Unknown, EmploymentSector.Manufacturing);
            var unemployedEmploymentModel = new Services.Interfaces.ClientDomain.Models.UnemployedEmploymentModel(10000, 25, employer, new DateTime(2010, 02, 1), UnemployedRemunerationType.RentalIncome, EmploymentStatus.Current);
            unemployedEmploymentAddedEvent = new UnconfirmedUnemployedEmploymentAddedToClientEvent(new DateTime(2014, 01, 01), 13
                , unemployedEmploymentModel.BasicIncome, unemployedEmploymentModel.StartDate
                , unemployedEmploymentModel.EmploymentStatus, unemployedEmploymentModel.SalaryPaymentDay
                , unemployedEmploymentModel.Employer.EmployerName, employmentKey);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            errorMessages = new SystemMessageCollection(new List<ISystemMessage> { new SystemMessage("Something went wrong", SystemMessageSeverityEnum.Error) });
            applicationDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<DetermineApplicationHouseholdIncomeCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);
            applicationStateMachine.WhenToldTo(x => x.SystemMessages).Return(errorMessages);
            applicationStateMachine.WhenToldTo(x => x.ClientEmploymentsFullyCaptured()).Return(true);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(unemployedEmploymentAddedEvent, serviceRequestMetadata));
        };

        private It should_determine_the_application_household_income = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<DetermineApplicationHouseholdIncomeCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_transfer_into_the_critical_error_state = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.CriticalErrorReported, Param.IsAny<Guid>()));
        };

        private It should_log_the_error = () =>
        {
            rawLogger.WasToldTo(x => x.LogError(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()
               , Param<string>.Matches(m => m.Contains(errorMessages.ErrorMessages().First().Message)), null));
        };

        private It should_throw_an_exception = () =>
        {
            thrownException.ShouldNotBeNull();
        };
    }
}