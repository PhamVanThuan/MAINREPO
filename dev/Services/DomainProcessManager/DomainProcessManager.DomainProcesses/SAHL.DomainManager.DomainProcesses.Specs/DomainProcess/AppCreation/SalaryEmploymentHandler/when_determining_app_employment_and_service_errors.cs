using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
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

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.SalaryEmploymentHandler
{
    public class when_determining_app_employment_and_service_errors : WithNewPurchaseDomainProcess
    {
        private static UnconfirmedSalariedEmploymentAddedToClientEvent salariedEmploymentAddedEvent;
        private static int applicationNumber, employmentKey;
        private static SystemMessageCollection errorMessages;
        private static Exception thrownException;

        private Establish context = () =>
        {
            applicationNumber = 150;
            employmentKey = 532;
            domainProcess.ProcessState = applicationStateMachine;
            var employer = new Services.Interfaces.ClientDomain.Models.EmployerModel(13,
                "ACME co",
                "031",
                "11224455",
                "",
                "",
                EmployerBusinessType.CloseCorporation,
                EmploymentSector.Manufacturing);

            var salariedEmploymentModel = new Services.Interfaces.ClientDomain.Models.SalariedEmploymentModel(30000,
                25,
                employer,
                new DateTime(2010, 02, 1),
                SalariedRemunerationType.Salaried,
                EmploymentStatus.Current,
                null);

            salariedEmploymentAddedEvent = new UnconfirmedSalariedEmploymentAddedToClientEvent(new DateTime(2014, 01, 01),
                13,
                salariedEmploymentModel.BasicIncome,
                salariedEmploymentModel.StartDate,
                salariedEmploymentModel.EmploymentStatus,
                salariedEmploymentModel.SalaryPaymentDay,
                salariedEmploymentModel.Employer.EmployerName,
                salariedEmploymentModel.Employer.TelephoneCode,
                salariedEmploymentModel.Employer.TelephoneNumber,
                salariedEmploymentModel.Employer.ContactPerson,
                salariedEmploymentModel.Employer.ContactEmail,
                salariedEmploymentModel.Employer.EmployerBusinessType,
                salariedEmploymentModel.Employer.EmploymentSector,
                employmentKey);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            errorMessages =
                new SystemMessageCollection(new List<ISystemMessage> { new SystemMessage("Something went wrong", SystemMessageSeverityEnum.Error) });
            applicationDomainService.WhenToldTo(
                x => x.PerformCommand(Param.IsAny<SetApplicationEmploymentTypeCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(errorMessages);
            applicationStateMachine.WhenToldTo(x => x.SystemMessages).Return(errorMessages);
            applicationStateMachine.WhenToldTo(x => x.ClientEmploymentsFullyCaptured()).Return(true);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(salariedEmploymentAddedEvent, serviceRequestMetadata));
        };

        private It should_set_the_application_employment_type = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<SetApplicationEmploymentTypeCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_transfer_into_the_critical_error_state = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.CriticalErrorReported, Param.IsAny<Guid>()));
        };

        private It should_log_the_error = () =>
        {
            rawLogger.WasToldTo(
                x => x.LogError(Param.IsAny<LogLevel>(),
                    Param.IsAny<string>(),
                    Param.IsAny<string>(),
                    Param.IsAny<string>(),
                    Param.IsAny<string>()
                    ,
                    Param<string>.Matches(m => m.Contains(errorMessages.ErrorMessages().First().Message)),
                    null));
        };

        private It should_throw_an_exception = () =>
        {
            thrownException.ShouldNotBeNull();
        };
    }
}
