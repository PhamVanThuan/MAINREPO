using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Models.ThirdPartyInvoice
{
    public class when_invoice_line_item_created_with_IsVat_set_to_true : WithFakes
    {
        private static decimal amount;
        private static InvoiceLineItemModel invoiceLineItem;

        private Establish context = () =>
        {
            amount = 3000.32M;
        };

        private Because of = () =>
        {
            invoiceLineItem = new InvoiceLineItemModel(null, 1, 1, amount, true);
        };

        private It should_set_the_vat_amount_to_14_percent_of_the_amount = () =>
        {
            invoiceLineItem.VATAmount.ShouldEqual((amount * 0.14M));
        };

        private It should_set_the_total_amount_including_vat_to_sum_of_amount_and_vat_amount = () =>
        {
            invoiceLineItem.TotalAmountIncludingVAT.ShouldEqual(amount * 1.14M);
        };
    }
}