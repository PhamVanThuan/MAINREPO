using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationEmploymentTypeHandler
{
    public class when_income_and_employment_has_been_determined : WithNewPurchaseDomainProcess
    {
        private static ApplicationEmploymentTypeSetEvent applicationEmploymentTypeSetEvent;
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationNumber = 150;
            domainProcess.ProcessState = applicationStateMachine;
            applicationEmploymentTypeSetEvent = new ApplicationEmploymentTypeSetEvent(new DateTime(2014, 01, 01), applicationNumber, (int)EmploymentType.Salaried);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.ApplicationHouseHoldIncomeDetermined)).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.ApplicationEmploymentDetermined)).Return(true);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(applicationEmploymentTypeSetEvent, serviceRequestMetadata);
        };

        private It should_fire_the_application_household_income_determined_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Param.IsAny<Guid>()));
        };

        private It should_price_the_new_business_application = () =>
        {
            financialDomainService.WasToldTo(x => x.PerformCommand(Param<PriceNewBusinessApplicationCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}