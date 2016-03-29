using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Rules.InviceDateCannotBeInFutureSpecs
{
    public class when_invoice_date_is_current_day : WithFakes
    {
        static InvoiceDateCannotBeInTheFutureRule rule;
        static ThirdPartyInvoiceModel ruleModel;
        static ISystemMessageCollection messages;

        Establish context = () =>
        {
            ruleModel = new ThirdPartyInvoiceModel(1, Guid.NewGuid(), "Invoice_Number", DateTime.Now, new List<InvoiceLineItemModel>(), true, string.Empty);
            rule = new InvoiceDateCannotBeInTheFutureRule();
            messages = SystemMessageCollection.Empty();
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
