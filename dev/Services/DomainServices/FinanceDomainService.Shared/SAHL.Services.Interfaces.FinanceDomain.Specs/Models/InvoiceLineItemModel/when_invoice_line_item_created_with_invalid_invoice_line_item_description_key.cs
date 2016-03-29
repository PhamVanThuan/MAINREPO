using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Models.ThirdPartyInvoice
{
    public class when_invoice_line_item_created_with_invalid_invoice_line_item_description_key : WithFakes
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
                invoiceLineItem = new InvoiceLineItemModel(null, 5, -1, 3000.34M, true);
            });
        };

        private It should_throw_ThirdPartyInvoiceNumber_not_set_exception = () =>
        {
            ex.Message.Equals("An InvoiceLineItemDescriptionKey must be provided.");
        };
    }
}