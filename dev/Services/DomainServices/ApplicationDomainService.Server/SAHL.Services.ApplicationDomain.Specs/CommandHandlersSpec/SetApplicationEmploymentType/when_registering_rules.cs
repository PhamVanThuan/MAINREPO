using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.SetApplicationEmploymentType
{
    public class when_registering_rules : WithCoreFakes
    {
        private static SetApplicationEmploymentTypeCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicationManager applicationManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<OfferInformationDataModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = An<IApplicationManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = An<IDomainRuleManager<OfferInformationDataModel>>();
        };

        private Because of = () =>
        {
            handler = new SetApplicationEmploymentTypeCommandHandler(unitOfWorkFactory, applicationDataManager, applicationManager, applicantDataManager, eventRaiser, domainRuleManager);
        };

        private It should_register_the_ApplicationMayNotBeAcceptedWhenSettingApplicationEmploymentTypeRule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicationMayNotBeAcceptedWhenSettingApplicationEmploymentTypeRule>()));
        };
    }
}