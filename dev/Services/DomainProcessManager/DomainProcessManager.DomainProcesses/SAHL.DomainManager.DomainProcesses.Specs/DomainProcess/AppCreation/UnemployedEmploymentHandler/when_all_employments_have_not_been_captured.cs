using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.PropertyDomain;
using SAHL.Services.Interfaces.X2;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.UnemployedEmploymentHandler
{
    public class when_all_employments_have_not_been_captured : WithNewPurchaseDomainProcess
    {
        private static UnconfirmedUnemployedEmploymentAddedToClientEvent unemployedEmploymentAddedEvent;
        private static int applicationNumber, employmentKey;

        private Establish context = () =>
        {
            applicationNumber = 150;
            employmentKey = 989;
            domainProcess.ProcessState = applicationStateMachine;
            var employer = new Services.Interfaces.ClientDomain.Models.EmployerModel(13, "Unemployed", "031", "11224455", null, null, EmployerBusinessType.Unknown, EmploymentSector.Manufacturing);
            var unemployedEmploymentModel = new Services.Interfaces.ClientDomain.Models.UnemployedEmploymentModel(10000, 25, employer, new DateTime(2010, 02, 1), UnemployedRemunerationType.RentalIncome, EmploymentStatus.Current);
            unemployedEmploymentAddedEvent = new UnconfirmedUnemployedEmploymentAddedToClientEvent(new DateTime(2014, 01, 01), 13
                , unemployedEmploymentModel.BasicIncome, unemployedEmploymentModel.StartDate
                , unemployedEmploymentModel.EmploymentStatus, unemployedEmploymentModel.SalaryPaymentDay
                , unemployedEmploymentModel.Employer.EmployerName, employmentKey);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientEmploymentsFullyCaptured()).Return(false);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(unemployedEmploymentAddedEvent, serviceRequestMetadata);
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