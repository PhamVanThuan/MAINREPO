using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_getting_third_party_invoice : WithCoreFakes
    {
        private static IThirdPartyInvoiceManager manager;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceModel thirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemDataModel> invoiceLineItems;
        private static FakeDbFactory dbFactory;
        private static int thirdPartyInvoiceKey;
        private static ThirdPartyInvoiceDataModel thirdPartyInvoiceData;
        private static int expectedLineItemsCount;
        private static DateTime invoiceDate;
        private static string expectedWhereClause;

        private Establish context = () =>
        {
            dataManager = An<IThirdPartyInvoiceDataManager>();
            dbFactory = new FakeDbFactory();
            thirdPartyInvoiceKey = 2545;
            invoiceDate = DateTime.Now;
            thirdPartyInvoiceData = new ThirdPartyInvoiceDataModel(
                  thirdPartyInvoiceKey
                , "SAHL03-2015/03/1"
                , 1
                , 892437
                , combGuid.Generate()
                , "invoiceNumber"
                , invoiceDate
                , "attorney@partners.co.za"
                , null
                , null
                , null
                , true
                , invoiceDate
                , string.Empty
             );

            invoiceLineItems = new List<InvoiceLineItemDataModel>
            {
                  new InvoiceLineItemDataModel(121, thirdPartyInvoiceKey, 1, 100.00M, true, 14.00M, 114.00M)
                , new InvoiceLineItemDataModel(4, thirdPartyInvoiceKey, 2, 500.10M, true, 70.014M, 570.114M )
                , new InvoiceLineItemDataModel(5, thirdPartyInvoiceKey, 9, 2000.00M, true, 280.00M, 2280.00M )
            };

            expectedLineItemsCount = invoiceLineItems.Count();
            expectedWhereClause = string.Format("[ThirdPartyInvoiceKey] = {0}", thirdPartyInvoiceKey);
            dataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(Arg.Any<int>())).Return(thirdPartyInvoiceData);
            dataManager.WhenToldTo(x => x.GetInvoiceLineItems(Arg.Any<int>())).Return(invoiceLineItems);
            manager = new ThirdPartyInvoiceManager(dataManager);
        };

        private Because of = () =>
        {
            thirdPartyInvoice = manager.GetThirdPartyInvoiceModel(thirdPartyInvoiceKey);
        };

        private It should_query_using_the_key_provided = () =>
        {
            dataManager.WasToldTo(x => x.GetThirdPartyInvoiceByKey(thirdPartyInvoiceKey));
        };

        private It should_query_for_all_child_invoice_line_items = () =>
        {
            dataManager.WasToldTo(x => x.GetInvoiceLineItems(thirdPartyInvoiceKey));
        };

        private It should_get_a_valid_full_invoice = () =>
        {
            (
                thirdPartyInvoice.InvoiceNumber.Equals("invoiceNumber", StringComparison.Ordinal)
                && thirdPartyInvoice.LineItems.Count() == expectedLineItemsCount
                && thirdPartyInvoice.InvoiceDate.Equals(invoiceDate)
                && thirdPartyInvoice.AmountExcludingVAT == invoiceLineItems.Sum(li => li.Amount)
                && thirdPartyInvoice.CapitaliseInvoice == thirdPartyInvoiceData.CapitaliseInvoice
                && thirdPartyInvoice.VATAmount == invoiceLineItems.Sum(li => li.VATAmount)
            ).ShouldBeTrue();
        };
    }
}