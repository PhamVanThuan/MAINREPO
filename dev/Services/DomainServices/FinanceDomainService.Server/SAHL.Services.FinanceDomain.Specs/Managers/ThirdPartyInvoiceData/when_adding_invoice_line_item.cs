using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_adding_invoice_line_item : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static InvoiceLineItemModel invoiceLineItemModel;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            invoiceLineItemModel = new InvoiceLineItemModel(null, 1212, 12, 1234.34M, true);
        };

        private Because of = () =>
        {
            dataManager.AddInvoiceLineItem(invoiceLineItemModel);
        };

        private It should_add_invoice_line_item = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<InvoiceLineItemDataModel>.Matches(m =>
                m.Amount == invoiceLineItemModel.AmountExcludingVAT &&
                m.ThirdPartyInvoiceKey == invoiceLineItemModel.ThirdPartyInvoiceKey &&
                m.InvoiceLineItemDescriptionKey == invoiceLineItemModel.InvoiceLineItemDescriptionKey &&
                m.IsVATItem == invoiceLineItemModel.IsVATItem &&
                m.TotalAmountIncludingVAT == invoiceLineItemModel.TotalAmountIncludingVAT)));
        };

        private It should_complete_the_context = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}