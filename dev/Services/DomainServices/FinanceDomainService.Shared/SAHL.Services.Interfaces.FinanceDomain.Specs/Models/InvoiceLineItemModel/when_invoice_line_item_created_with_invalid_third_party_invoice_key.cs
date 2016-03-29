using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Models.ThirdPartyInvoice
{
    public class when_invoice_line_item_created_with_invalid_third_party_invoice_key : WithFakes
    {
        private static Exception ex;
        private static InvoiceLineItemModel invoiceLineItem;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                invoiceLineItem = new InvoiceLineItemModel(null, -1, 1, 3000.34M, true);
            });
        };

        private It should_throw_ThirdPartyInvoiceNumber_not_set_exception = () =>
        {
            ex.Message.Equals("A ThirdPartyInvoiceKey must be provided.");
        };
    }
}