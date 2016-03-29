using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_amending_third_party_invoice : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static ThirdPartyInvoiceModel thirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemModel> attorneyInvoiceLineItems;
        private static Guid thirdPartyId;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            thirdPartyId = Guid.NewGuid();
            attorneyInvoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null, 1212, 1, 21212.34M, true) };
            thirdPartyInvoice = new ThirdPartyInvoiceModel(1212, thirdPartyId, "DD0011", DateTime.Now, attorneyInvoiceLineItems, true, "Payment Reference");
        };

        private Because of = () =>
        {
            dataManager.AmendThirdPartyInvoiceHeader(thirdPartyInvoice);
        };

        private It should_add_the_third_party_invoice = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<ThirdPartyInvoiceDataModel>(Param<UpdateThirdPartyInvoiceHeaderStatement>.Matches(
                y => y.ThirdPartyInvoiceKey == thirdPartyInvoice.ThirdPartyInvoiceKey
                && y.ThirdPartyId == thirdPartyInvoice.ThirdPartyId
                && y.InvoiceDate == thirdPartyInvoice.InvoiceDate
                && y.InvoiceNumber == thirdPartyInvoice.InvoiceNumber
                && y.PaymentReference == thirdPartyInvoice.PaymentReference
                )));
        };

        private It should_complete_the_context = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}