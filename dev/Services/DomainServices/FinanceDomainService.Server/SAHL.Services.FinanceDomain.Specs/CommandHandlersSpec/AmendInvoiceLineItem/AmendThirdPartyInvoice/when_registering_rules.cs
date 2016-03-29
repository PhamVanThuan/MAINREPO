using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AmendThirdPartyInvoice
{
    public class when_registering_rules : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IThirdPartyInvoiceManager thirdPartyInvoiceDataFilter;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static AmendThirdPartyInvoiceCommandHandler handler;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceDataFilter = An<IThirdPartyInvoiceManager>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
        };

        private Because of = () =>
            {
                handler = new AmendThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, thirdPartyInvoiceDataFilter, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);
            };

        private It should_register_the_InvoiceCannotBeAmendedOnceApproved_rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<InvoiceCannotBeAmendedOnceApprovedRule>());
        };

        private It should_register_the_InvoiceCannotBeAmendedOnceAuthorisedForPayment_rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<InvoiceCannotBeAmendedOnceApprovedForPaymentRule>());
        };

        private It should_register_the_InvoiceDateCannotBeInTheFutureRule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<InvoiceDateCannotBeInTheFutureRule>());
        };

        private It should_register_the_InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule>());
        };

        private It should_register_the_InvoicePaymentReferenceCannotBeEmptyRule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<InvoicePaymentReferenceCannotBeEmptyRule>());
        };
    }
}