using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.AttorneyInvoiceMustHaveAtLeastOneLineItemRuleSpecs
{
    internal class when_there_are_no_line_items : WithFakes
    {
        private static InvoiceMustHaveAtLeastOneLineItemRule rule;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            model = new ThirdPartyInvoiceModel(123, Guid.Empty, "invoiceNumber", DateTime.Now, Enumerable.Empty<InvoiceLineItemModel>(), true, string.Empty);
            rule = new InvoiceMustHaveAtLeastOneLineItemRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The Invoice does not contain any line items.");
        };
    }
}