using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    public class when_registering_rules : WithCoreFakes
    {
        private static IDomiciliumDataManager domiciliumDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IApplicationDataManager applicationDataManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static LinkDomiciliumAddressToApplicantCommandHandler handler;
        private static IDomainRuleManager<ApplicantDomiciliumAddressModel> domainRuleContext;
        private static IADUserManager aduserManager;

        private Establish context = () =>
        {
            applicantDataManager = An<IApplicantDataManager>();
            applicationDataManager = An<IApplicationDataManager>();
            domiciliumDataManager = An<IDomiciliumDataManager>();
            domainRuleContext = An<IDomainRuleManager<ApplicantDomiciliumAddressModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            aduserManager = An<IADUserManager>();
        };

        private Because of = () =>
        {
            handler = new LinkDomiciliumAddressToApplicantCommandHandler(
                applicantDataManager, applicationDataManager, domiciliumDataManager
                , unitOfWorkFactory, eventRaiser, domainRuleContext, aduserManager);
        };

        private It should_register_the_DoesClientHavePendingDomiciliumRule = () =>
        {
            domainRuleContext.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicantCannotHaveAnExistingPendingDomiciliumAddressRule>()));
        };

        private It should_register_the_IsDomiciliumAddressPendingDomiciliumAddressRule = () =>
        {
            domainRuleContext.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicantCanOnlyBeLinkedToAPendingDomiciliumAddressRule>()));
        };
    }
}