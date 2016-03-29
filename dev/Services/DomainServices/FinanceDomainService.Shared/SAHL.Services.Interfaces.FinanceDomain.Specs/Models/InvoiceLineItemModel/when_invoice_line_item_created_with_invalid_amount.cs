using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Models.ThirdPartyInvoice
{
    public class when_invoice_line_item_created_with_invalid_amount : WithFakes
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
                invoiceLineItem = new InvoiceLineItemModel(null, 1, 1, 0.00M, true);
            });
        };

        private It should_throw_ThirdPartyInvoiceNumber_not_set_exception = () =>
        {
            ex.Message.Equals("The line item amount should be greater than R0.00.");
        };
    }
}