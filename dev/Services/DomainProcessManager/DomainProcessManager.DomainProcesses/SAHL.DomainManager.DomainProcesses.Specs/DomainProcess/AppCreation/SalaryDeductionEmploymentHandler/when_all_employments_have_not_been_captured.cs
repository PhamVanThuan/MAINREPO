using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.SalaryDeductionEmploymentHandler
{
    public class when_all_employments_have_not_been_captured : WithNewPurchaseDomainProcess
    {
        private static UnconfirmedSalaryDeductionEmploymentAddedToClientEvent salaryDeductionEmploymentAddedEvent;
        private static int applicationNumber, employmentKey;

        private Establish context = () =>
        {
            applicationNumber = 150;
            employmentKey = 2534;
            domainProcess.ProcessState = applicationStateMachine;
            var employer = new Services.Interfaces.ClientDomain.Models.EmployerModel(13,
                "ACME co",
                "031",
                "11224455",
                "",
                "",
                EmployerBusinessType.CloseCorporation,
                EmploymentSector.Manufacturing);

            var salaryDeductionEmploymentModel = new Services.Interfaces.ClientDomain.Models.SalaryDeductionEmploymentModel(30000,
                5000,
                25,
                employer,
                new DateTime(2010, 02, 1),
                SalaryDeductionRemunerationType.Salaried,
                EmploymentStatus.Current,
                null);

            salaryDeductionEmploymentAddedEvent = new UnconfirmedSalaryDeductionEmploymentAddedToClientEvent(new DateTime(2014, 01, 01),
                13,
                salaryDeductionEmploymentModel.HousingAllowance,
                salaryDeductionEmploymentModel.RemunerationType,
                salaryDeductionEmploymentModel.BasicIncome,
                salaryDeductionEmploymentModel.StartDate,
                salaryDeductionEmploymentModel.EmploymentStatus,
                salaryDeductionEmploymentModel.SalaryPaymentDay,
                salaryDeductionEmploymentModel.Employer.EmployerName,
                salaryDeductionEmploymentModel.Employer.TelephoneCode,
                salaryDeductionEmploymentModel.Employer.TelephoneNumber,
                salaryDeductionEmploymentModel.Employer.ContactPerson,
                salaryDeductionEmploymentModel.Employer.ContactEmail,
                salaryDeductionEmploymentModel.Employer.EmployerBusinessType,
                salaryDeductionEmploymentModel.Employer.EmploymentSector,
                employmentKey);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientEmploymentsFullyCaptured()).Return(false);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(salaryDeductionEmploymentAddedEvent, serviceRequestMetadata);
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
