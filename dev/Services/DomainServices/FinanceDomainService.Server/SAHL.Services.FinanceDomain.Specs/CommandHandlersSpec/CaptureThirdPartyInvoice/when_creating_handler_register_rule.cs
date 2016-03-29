using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CaptureThirdPartyInvoice
{
    public class when_creating_handler_register_rule : WithFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static CaptureThirdPartyInvoiceCommandHandler handler;
        private static IEventRaiser eventRaiser;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IUnitOfWorkFactory uowFactory;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            eventRaiser = An<IEventRaiser>();
            serviceCommandRouter = An<IServiceCommandRouter>();
            uowFactory = An<IUnitOfWorkFactory>();

            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
        };

        private Because of = () =>
        {
            handler = new CaptureThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, eventRaiser, serviceCommandRouter, uowFactory, domainRuleManager);
        };

        private It should_register_the_InvoiceShouldBeAmendedOnceInitiallyCapturedRule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<InvoiceShouldBeAmendedOnceInitiallyCapturedRule>());
        };

        private It should_register_the_ThirdPartyInvoiceStatusMustBeReceivedRule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<InvoiceCannotBeAmendedOnceApprovedRule>());
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