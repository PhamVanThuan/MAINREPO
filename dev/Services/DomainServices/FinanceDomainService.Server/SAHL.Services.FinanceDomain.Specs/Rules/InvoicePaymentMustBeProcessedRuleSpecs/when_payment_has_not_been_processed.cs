using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;

namespace SAHL.Services.FinanceDomain.Specs.Rules.InvoicePaymentMustBeProcessedRuleSpecs
{
    public class when_payment_has_not_been_processed : WithFakes
    {
        private static InvoicePaymentShouldBeBeingProcessedRule rule;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static RuleModel ruleModel;
        private static ISystemMessageCollection messages;

        private static ThirdPartyInvoiceDataModel invoice;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            invoice = new ThirdPartyInvoiceDataModel(1, "reference", (int)InvoiceStatus.Received, 1428540, Guid.NewGuid(), "invoiceNumber", DateTime.Now, "clintons@sahomeloans.com",
                100, 14, 114, true, DateTime.Now, "paymentReference");
            ruleModel = new RuleModel(12345);
            dataManager = An<IThirdPartyInvoiceDataManager>();
            dataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(Arg.Any<int>())).Return(invoice);
            rule = new InvoicePaymentShouldBeBeingProcessedRule(dataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Invoice payment is not currently being processed.");
        };
    }
}