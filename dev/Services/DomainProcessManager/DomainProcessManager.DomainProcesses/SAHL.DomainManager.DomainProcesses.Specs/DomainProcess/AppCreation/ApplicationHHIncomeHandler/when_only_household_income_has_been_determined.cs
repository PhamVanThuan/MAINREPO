using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationHHIncomeHandler
{
    public class when_only_household_income_has_been_determined : WithNewPurchaseDomainProcess
    {
        private static ApplicationHouseholdIncomeDeterminedEvent applicationHHIncomeDeterminedEvent;
        private static double householdIncome = 0;
        private static int applicationNumber = 1;

        private Establish context = () =>
        {
            domainProcess.ProcessState = applicationStateMachine;
            applicationHHIncomeDeterminedEvent = new ApplicationHouseholdIncomeDeterminedEvent(new DateTime(2014, 01, 01), applicationNumber, householdIncome);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.ApplicationHouseHoldIncomeDetermined)).Return(false);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.ApplicationEmploymentDetermined)).Return(false);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(applicationHHIncomeDeterminedEvent, serviceRequestMetadata);
        };

        private It should_fire_the_application_household_income_determined_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, Param.IsAny<Guid>()));
        };

        private It should_not_price_the_new_business_application = () =>
        {
            financialDomainService.WasNotToldTo(x => x.PerformCommand(Param.IsAny<PriceNewBusinessApplicationCommand>(), Param.IsAny<ServiceRequestMetadata>()));
        };
    }
}