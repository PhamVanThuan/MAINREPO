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
    public class when_adding_a_new_purchase_and_validate_fails : WithDomainServiceMocks
    {
        private static NewPurchaseApplicationDomainProcess domainProcess;
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;

        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();

            domainProcess = new NewPurchaseApplicationDomainProcess(
                                      applicationStateMachine
                                    , applicationDomainService
                                    , clientDomainService
                                    , addressDomainService
                                    , financialDomainService
                                    , bankAccountDomainService
                                    , combGuidGenerator
                                    , clientDataManager
                                    , x2WorkflowManager
                                    , linkedKeyManager
                                    , propertyDomainService
                                    , communicationManager
                                    , applicationDataManager
                                    , domainRuleManager
                                    , rawLogger
                                    , loggerSource
                                    , loggerAppSource
                               );

            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            domainRuleManager.WhenToldTo(x => x.ExecuteRulesForContext(Param.IsAny<ISystemMessageCollection>(), newPurchaseCreationModel, newPurchaseCreationModel.OriginationSource))
                 .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("Rule failure message.", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);
        };

        private It should_not_add_a_new_purchase_application = () =>
        {
            applicationDomainService.WasNotToldTo(x => x.PerformCommand(Param.IsAny<AddNewPurchaseApplicationCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_go_into_critical_error_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.CriticalErrorOccured).ShouldBeTrue();
        };

        private It should_have_an_error_message = () =>
        {
            applicationStateMachine.SystemMessages.AllMessages.Any(x => x.Message == "Rule failure message." && x.Severity == SystemMessageSeverityEnum.Error).ShouldBeTrue();
        };
    }
}