using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationDebitOrder
{
    public class when_registering_debit_order_rules : WithCoreFakes
    {
        private static AddApplicationDebitOrderCommand command;
        private static AddApplicationDebitOrderCommandHandler handler;
        private static ApplicationDebitOrderModel applicationDebitOrderModel;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicationDebitOrderModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            applicationDebitOrderModel = new ApplicationDebitOrderModel(1234567, 15, 1234567);
            domainRuleManager = An<IDomainRuleManager<ApplicationDebitOrderModel>>();
            command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
        };

        private Because of = () =>
        {
            handler = new AddApplicationDebitOrderCommandHandler(applicationDataManager, domainRuleManager, An<IDomainQueryServiceClient>(), eventRaiser, applicantDataManager, unitOfWorkFactory);
        };

        private It should_ensure_the_client_bank_account_belongs_to_an_applicant_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ClientBankAccountMustBelongToApplicantOnApplicationRule>()));
        };

        private It should_ensure_the_debit_order_day_is_between_1_and_28_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<DebitOrderDayCannotBeAfterThe28thDayRule>()));
        };
    }
}