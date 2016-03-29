using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.PriceFundingEventHandler
{
    public class when_pricing_new_business_application_successfully : WithNewPurchaseDomainProcess
    {
        private static NewBusinessApplicationPricedEvent newBusinessApplicationPricedEvent;
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationNumber = 150;
            domainProcess.ProcessState = applicationStateMachine;
            newBusinessApplicationPricedEvent = new NewBusinessApplicationPricedEvent(new DateTime(2014, 01, 01).Date, applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.IsInState(ApplicationState.ApplicationPriced)).Return(true);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(newBusinessApplicationPricedEvent, serviceRequestMetadata);
        };

        private It should_fund_new_business_application = () =>
        {
            financialDomainService.WasToldTo(x => x.PerformCommand(
                Param<FundNewBusinessApplicationCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_be_in_application_priced_state = () =>
        {
            applicationStateMachine.WasToldTo(x => x.IsInState(ApplicationState.ApplicationPriced));
        };

        private It should_fire_the_application_pricing_confirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, Param.IsAny<Guid>()));
        };
    }
}