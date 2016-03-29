using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_switch_application_and_validate_fails : WithDomainServiceMocks
    {
        private static SwitchApplicationDomainProcess domainProcess;
        private static SwitchApplicationCreationModel switchCreationModel;

        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();

            domainProcess = new SwitchApplicationDomainProcess(applicationStateMachine, applicationDomainService, clientDomainService, addressDomainService, financialDomainService
                                    , bankAccountDomainService, combGuidGenerator, clientDataManager, x2WorkflowManager, linkedKeyManager, propertyDomainService
                                    , communicationManager, applicationDataManager, domainRuleManager, rawLogger, loggerSource, loggerAppSource);
            switchCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan) as SwitchApplicationCreationModel;
            domainRuleManager.WhenToldTo(x => x.ExecuteRulesForContext(Param.IsAny<ISystemMessageCollection>(), switchCreationModel, switchCreationModel.OriginationSource))
                .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("Rule failure message.", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            domainProcess.Start(switchCreationModel, typeof(SwitchApplicationAddedEvent).Name);
        };

        private It should_not_add_the_application = () =>
        {
            applicationDomainService.WasNotToldTo(x => x.PerformCommand(Param.IsAny<AddSwitchApplicationCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_go_into_critical_error_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.CriticalErrorOccured).ShouldBeTrue();
        };

        private It should_have_an_error_message = () =>
        {
            applicationStateMachine.SystemMessages.ErrorMessages().First().Message.ShouldEqual("Rule failure message.");
        };
    }
}