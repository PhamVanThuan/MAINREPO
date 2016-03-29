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
    public class when_invoice_date_is_midnight_tomorrow : WithFakes
    {
        static InvoiceDateCannotBeInTheFutureRule rule;
        static ThirdPartyInvoiceModel ruleModel;
        static ISystemMessageCollection messages;

        Establish context = () =>
        {
            var midnightTomorrow = DateTime.Today.AddDays(1);
            ruleModel = new ThirdPartyInvoiceModel(1, Guid.NewGuid(), "Invoice_Number", midnightTomorrow, new List<InvoiceLineItemModel>(), true, string.Empty);
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
