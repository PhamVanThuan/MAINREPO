using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_amending_third_party_invoice_totals : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static int thirdPartyInvoiceKey;
        private static  List<InvoiceLineItemDataModel> lineItems;
        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            thirdPartyInvoiceKey = 48379;

            lineItems = new List<InvoiceLineItemDataModel>()
            {
                new InvoiceLineItemDataModel(1, 1, 1, 100.00M, false, 0.00M, 100.00M),
                new InvoiceLineItemDataModel(2, 1, 2, 100.00M, true, 14.00M, 114.00M)
            };
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(y => y.SelectWhere<InvoiceLineItemDataModel>(Arg.Any<string>(), null))
                .Return(lineItems);

        };

        private Because of = () =>
        {
            dataManager.AmendInvoiceTotals(thirdPartyInvoiceKey);
        };

        private It should_adjust_attorney_invoice_totals = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Update<ThirdPartyInvoiceDataModel>(Param<UpdateThirdPartyInvoiceTotalsStatement>
                .Matches(m => m.TotalAmountExcludingVAT == lineItems.Sum(y=>y.Amount)
                           && m.VATAmount == lineItems.Sum(y => y.VATAmount)
                           && m.TotalAmountIncludingVAT == lineItems.Sum(y => y.Amount + y.VATAmount)
                )
            ));
        };

        private It should_complete_the_context = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}