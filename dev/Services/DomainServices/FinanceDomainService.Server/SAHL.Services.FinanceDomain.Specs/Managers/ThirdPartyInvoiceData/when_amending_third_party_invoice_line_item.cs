using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_amending_third_party_invoice_line_item : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static InvoiceLineItemModel InvoiceLineItemModel;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            InvoiceLineItemModel = new InvoiceLineItemModel(133, 1212, 12, 1234.34M, true);
        };

        private Because of = () =>
        {
            dataManager.AmendInvoiceLineItem(InvoiceLineItemModel);
        };

        private It should_add_attorney_invoice_line_item = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Update<InvoiceLineItemDataModel>(Param<AmendInvoiceLineItemStatement>.Matches(m =>
                m.Amount == InvoiceLineItemModel.AmountExcludingVAT &&
                m.TotalAmountIncludingVAT == InvoiceLineItemModel.TotalAmountIncludingVAT)));
        };

        private It should_complete_the_context = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}