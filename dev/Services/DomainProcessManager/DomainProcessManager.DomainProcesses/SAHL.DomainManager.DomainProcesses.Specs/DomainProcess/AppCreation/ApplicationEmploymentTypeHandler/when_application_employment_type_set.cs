using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationEmploymentTypeHandler
{
    public class when_application_employment_type_set : WithNewPurchaseDomainProcess
    {
        private static ApplicationEmploymentTypeSetEvent applicationEmploymentTypeSetEvent;
        private static int applicationNumber = 1;

        private Establish context = () =>
        {
            applicationNumber = 1234567;
            domainProcess.ProcessState = applicationStateMachine;
            applicationEmploymentTypeSetEvent = new ApplicationEmploymentTypeSetEvent(DateTime.Now, applicationNumber, (int)EmploymentType.Salaried);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientEmploymentsFullyCaptured()).Return(true);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(applicationEmploymentTypeSetEvent, serviceRequestMetadata);
        };

        private It should_fire_the_applicationemploymentdeterminedconfirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, applicationEmploymentTypeSetEvent.Id));
        };

        private It should_not_price_the_new_business_application = () =>
        {
            financialDomainService.WasNotToldTo(x => x.PerformCommand(Param.IsAny<PriceNewBusinessApplicationCommand>(), Param.IsAny<ServiceRequestMetadata>()));
        };
    }
}