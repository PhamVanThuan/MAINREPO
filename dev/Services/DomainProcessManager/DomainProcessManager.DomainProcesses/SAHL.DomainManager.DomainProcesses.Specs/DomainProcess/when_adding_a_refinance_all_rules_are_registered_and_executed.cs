using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_refinance_all_rules_are_registered_and_executed : WithDomainServiceMocks
    {
        private static RefinanceApplicationDomainProcess domainProcess;
        private static RefinanceApplicationCreationModel RefinanceCreationModel;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();

            domainProcess = new RefinanceApplicationDomainProcess(
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
            RefinanceCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.RefinanceLoan) as RefinanceApplicationCreationModel;
        };

        private Because of = () =>
        {
            domainProcess.Start(RefinanceCreationModel, typeof(RefinanceApplicationAddedEvent).Name);
        };

        private It should_register_the_comcorp_property_rule_in_the_comcorp_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule>(), OriginationSource.Comcorp));
        };

        private It should_register_the_does_vendor_exist_rule_in_the_comcorp_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<VendorMustExistForVendorCodeRule>(), OriginationSource.Comcorp));
        };

        private It should_register_the_lead_main_applicant_rule_in_the_comcorp_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<ApplicationMustHaveAtLeastOneMainApplicantRule>(), OriginationSource.Comcorp));
        };

        private It should_register_the_lead_main_applicant_rule_in_the_SAHL_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<ApplicationMustHaveAtLeastOneMainApplicantRule>(), OriginationSource.SAHomeLoans));
        };

        private It should_register_the_open_application_exists_for_property_and_applicant_rule_in_the_SAHL_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<ApplicantsMustHaveUniqueIdentityNumbersRule>(), OriginationSource.SAHomeLoans));
        };

        private It should_register_the_open_application_exists_for_property_and_applicant_rule_in_the_comcorp_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<ApplicantsMustHaveUniqueIdentityNumbersRule>(), OriginationSource.Comcorp));
        };

        private It should_register_the_applicants_must_have_unique_identity_numbers_rule_in_the_comcorp_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<ApplicantsMustHaveUniqueIdentityNumbersRule>(), OriginationSource.Comcorp));
        };

        private It should_register_the_applicants_must_have_unique_identity_numbers_rule_in_the_sahl_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<ApplicantsMustHaveUniqueIdentityNumbersRule>(), OriginationSource.SAHomeLoans));
        };

        private It should_register_eight_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<IDomainRule<ApplicationCreationModel>>(), Param.IsAny<OriginationSource>())).Times(8);
        };
    }
}