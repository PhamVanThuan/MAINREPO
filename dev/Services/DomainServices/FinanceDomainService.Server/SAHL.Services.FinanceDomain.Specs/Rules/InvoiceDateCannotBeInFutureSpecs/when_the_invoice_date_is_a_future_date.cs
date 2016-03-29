using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.InviceDateCannotBeInFutureSpecs
{
    public class when_the_invoice_date_is_a_future_date : WithFakes
    {
        static InvoiceDateCannotBeInTheFutureRule rule;
        static ThirdPartyInvoiceModel ruleModel;
        static ISystemMessageCollection messages;

        Establish context = () =>
        {
            ruleModel = new ThirdPartyInvoiceModel(1, Guid.NewGuid(), "Invoice_Number", DateTime.Now.AddDays(1), new List<InvoiceLineItemModel>(), true, string.Empty);
            rule = new InvoiceDateCannotBeInTheFutureRule();
            messages = SystemMessageCollection.Empty();
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_return_error_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Invoice date cannot be a future date.");
        };
    }
}
