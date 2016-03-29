using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_an_sa_home_loans_originated_application : WithDomainServiceMocks
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

            domainProcess = new SwitchApplicationDomainProcess(
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
            switchCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan) as SwitchApplicationCreationModel;
            switchCreationModel.OriginationSource = OriginationSource.SAHomeLoans;
            domainRuleManager.WhenToldTo(x => x.ExecuteRulesForContext(Param.IsAny<ISystemMessageCollection>(), switchCreationModel, switchCreationModel.OriginationSource))
                .Callback<ISystemMessageCollection>(y => { SystemMessageCollection.Empty(); });
        };

        private Because of = () =>
        {
            domainProcess.Start(switchCreationModel, typeof(SwitchApplicationAddedEvent).Name);
        };

        private It should_add_an_application = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<AddSwitchApplicationCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_go_into_the_critical_error_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.CriticalErrorOccured).ShouldBeFalse();
        };

        private It should_not_check_for_a_vendor = () =>
        {
            domainRuleManager.WasNotToldTo(x => x.RegisterRuleForContext(Param.IsAny<VendorMustExistForVendorCodeRule>(), OriginationSource.SAHomeLoans));
        };

        private It should_not_check_for_a_duplicate_comcorp_application = () =>
        {
            domainRuleManager.WasNotToldTo(x => x.RegisterRuleForContext(Param.IsAny<OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule>(), OriginationSource.SAHomeLoans));
        };
    }
}