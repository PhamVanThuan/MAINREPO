using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
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
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.UnemployedEmploymentHandler
{
    public class when_all_employents_have_been_captured : WithNewPurchaseDomainProcess
    {
        private static UnconfirmedUnemployedEmploymentAddedToClientEvent unemployedEmploymentAddedEvent;
        private static int applicationNumber, employmentKey;

        private Establish context = () =>
        {
            applicationNumber = 150;
            employmentKey = 2673;
            domainProcess.ProcessState = applicationStateMachine;
            var employer = new Services.Interfaces.ClientDomain.Models.EmployerModel(13, "Unemployed", "031", "11224455", null, null, EmployerBusinessType.Unknown, EmploymentSector.Manufacturing);
            var unemployedEmploymentModel = new Services.Interfaces.ClientDomain.Models.UnemployedEmploymentModel(10000, 25, employer, new DateTime(2010, 02, 1), UnemployedRemunerationType.RentalIncome, EmploymentStatus.Current);
            unemployedEmploymentAddedEvent = new UnconfirmedUnemployedEmploymentAddedToClientEvent(new DateTime(2014, 01, 01), 13
                , unemployedEmploymentModel.BasicIncome, unemployedEmploymentModel.StartDate
                , unemployedEmploymentModel.EmploymentStatus, unemployedEmploymentModel.SalaryPaymentDay
                , unemployedEmploymentModel.Employer.EmployerName, employmentKey);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientEmploymentsFullyCaptured()).Return(true);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(unemployedEmploymentAddedEvent, serviceRequestMetadata);
        };

        private It should_determine_the_application_household_income = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<DetermineApplicationHouseholdIncomeCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_set_the_employment_type = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<SetApplicationEmploymentTypeCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}