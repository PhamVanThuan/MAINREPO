﻿using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.SalaryEmploymentHandler
{
    public class when_all_employments_have_not_been_captured : WithNewPurchaseDomainProcess
    {
        private static UnconfirmedSalariedEmploymentAddedToClientEvent salariedEmploymentAddedEvent;
        private static int applicationNumber, employmentKey;

        private Establish context = () =>
        {
            applicationNumber = 150;
            employmentKey = 989;
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
            applicationStateMachine.WhenToldTo(x => x.ClientEmploymentsFullyCaptured()).Return(false);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(salariedEmploymentAddedEvent, serviceRequestMetadata);
        };

        private It should_not_determine_the_application_household_income = () =>
        {
            applicationDomainService.WasNotToldTo(x => x.PerformCommand(
                Param.IsAny<DetermineApplicationHouseholdIncomeCommand>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_set_the_employment_type = () =>
        {
            applicationDomainService.WasNotToldTo(x => x.PerformCommand(
                Param.IsAny<SetApplicationEmploymentTypeCommand>(),
                Param.IsAny<ServiceRequestMetadata>()));
        };
    }
}
