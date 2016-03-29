using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Rules.AttorneyInvoiceMustHaveAtLeastOneLineItemRuleSpecs
{
    public class when_there_are_multiple_line_items : WithFakes
    {
        private static InvoiceMustHaveAtLeastOneLineItemRule rule;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            model = new ThirdPartyInvoiceModel(123, Guid.Empty, "invoiceNumber", DateTime.Now,
                new List<InvoiceLineItemModel>()
                {
                    new InvoiceLineItemModel(1, 2, 1, 100.00M, false),
                    new InvoiceLineItemModel(2, 2, 1, 100.00M, true)
                }, true, string.Empty);
            rule = new InvoiceMustHaveAtLeastOneLineItemRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}