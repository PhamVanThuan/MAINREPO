using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationMailingAddress
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddApplicationMailingAddressCommandHandler handler;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicationMailingAddressModel> domainRuleManager;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicationMailingAddressModel>>();
        };

        private Because of = () =>
        {
            handler = new AddApplicationMailingAddressCommandHandler(domainQueryService, applicationDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser, applicantDataManager);
        };

        private It should_register_the_free_text_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicationMailingAddressCannotBeAFreeTextAddressRule>()));
        };

        private It should_register_email_correspondence_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<CheckIfClientForEmailCorrespondenceIsRequiredRule>()));
        };

        private It should_register_online_state_format_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<CheckIfOnlineFormatStatementIsRequiredRule>()));
        };

        private It should_register_the_applicant_check_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ClientAddressMustBelongToAnApplicantRule>()));
        };
    }
}
