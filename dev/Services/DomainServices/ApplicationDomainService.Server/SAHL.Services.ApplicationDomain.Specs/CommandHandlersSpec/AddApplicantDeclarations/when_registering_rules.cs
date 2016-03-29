using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicantDeclarations
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddApplicantDeclarationsCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IDomainRuleManager<ApplicantDeclarationsModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicantDataManager applicantDataManager;

        private Establish context = () =>
        {
            //create mock objects
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicantDeclarationsModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
        };

        private Because of = () =>
        {
            handler = new AddApplicantDeclarationsCommandHandler(
                serviceCommandRouter, applicationDataManager, domainQueryService, domainRuleManager, eventRaiser, applicantDataManager, unitOfWorkFactory);
        };

        private It should_register_CurrentlyUnderDebtReviewRule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<DebtArrangementAnsweredIsRequiredWhenUnderDebtReviewRule>()));
        };

        private It should_register_DeclaredInsolventRule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<RehabilitationDateIsRequiredWhenClientHasBeenDeclaredInsolventRule>()));
        };

        private It should_register_UnderAdministrationRule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<RescindedDateRequiredWhenClientHasBeenUnderAdminOrderRule>()));
        };

        private It should_register_ClientShouldBeANaturalPersonRule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ClientShouldBeANaturalPersonRule>()));
        };

        private It should_register_ClientIsAnApplicantOnApplicationRule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ClientIsAnApplicantOnApplicationRule>()));
        };
    }
}
