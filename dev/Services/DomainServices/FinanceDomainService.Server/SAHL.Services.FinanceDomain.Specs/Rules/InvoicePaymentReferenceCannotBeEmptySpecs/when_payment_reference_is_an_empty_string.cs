using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.InvoicePaymentReferenceCannotBeEmptySpecs
{
    public class when_payment_reference_is_an_empty_string : WithFakes
    {
        static InvoicePaymentReferenceCannotBeEmptyRule rule;
        static ThirdPartyInvoiceModel ruleModel;
        static ISystemMessageCollection messages;

        Establish context = () =>
        {
            ruleModel = new ThirdPartyInvoiceModel(1, Guid.NewGuid(), "Invoice_Number", DateTime.Now, new List<InvoiceLineItemModel>(), true, string.Empty);
            rule = new InvoicePaymentReferenceCannotBeEmptyRule();
            messages = new SystemMessageCollection();
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Invoice payment reference must be captured.");
        };
    }
}
