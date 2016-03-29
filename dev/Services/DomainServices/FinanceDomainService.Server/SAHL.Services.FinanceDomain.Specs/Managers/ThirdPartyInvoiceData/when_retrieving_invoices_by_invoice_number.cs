using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_retrieving_invoices_by_invoice_number : WithFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static ThirdPartyInvoiceDataManager dataManager;
        private static string invoiceNumber;
        private static string expectedWhereClause;
        private static IEnumerable<ThirdPartyInvoiceDataModel> invoices;
        private static IEnumerable<ThirdPartyInvoiceDataModel> results;

        private Establish context = () =>
        {
            invoices = new ThirdPartyInvoiceDataModel[] { new ThirdPartyInvoiceDataModel(1, "sahlReference", 1, 1408282, Guid.NewGuid(), invoiceNumber, DateTime.Now.AddDays(-5),
                "halouser@sahomeloans.com", 1500, 100, 1600, true, DateTime.Now.AddDays(-7), string.Empty)};
            fakeDbFactory = new FakeDbFactory();
            invoiceNumber = @"SAHL/R/1/1/22";
            expectedWhereClause = string.Format("[InvoiceNumber] = '{0}'", invoiceNumber);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectWhere<ThirdPartyInvoiceDataModel>(expectedWhereClause, null))
                .Return(invoices);
            dataManager = new ThirdPartyInvoiceDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            results = dataManager.GetThirdPartyInvoicesByInvoiceNumber(invoiceNumber);
        };

        private It should_return_the_result_of_the_query = () =>
        {
            results.First().ShouldEqual(invoices.First());
        };
    }
}