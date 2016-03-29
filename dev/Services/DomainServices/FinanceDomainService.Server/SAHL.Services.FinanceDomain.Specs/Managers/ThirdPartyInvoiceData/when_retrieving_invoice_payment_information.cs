using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceDataSpec
{
    public class when_retrieving_invoice_payment_information : WithFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static FakeDbFactory dbFactory;
        private static int[] thirdPartyInvoiceKeys;
        private static IEnumerable<ThirdPartyInvoicePaymentModel> results, expectedResults;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            thirdPartyInvoiceDataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            thirdPartyInvoiceKeys = new int[] { 12004, 12409, 1220 };

            expectedResults = new List<ThirdPartyInvoicePaymentModel> { 
                new ThirdPartyInvoicePaymentModel("Strauss daly","strausdaly@sd.com", 1408,1114M,"Invoice Num 32", 1005) 
            };
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(y => y.Select(Param.IsAny<GetThirdPartyInvoicePaymentInformationStatement>())).Return(expectedResults);
        };

        Because of = () =>
        {
            results = thirdPartyInvoiceDataManager.GetThirdPartyInvoicePaymentInformation(thirdPartyInvoiceKeys);
        };

        It should_retrieve_the_data_from_the_db = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(y => y.Select(Param.IsAny<GetThirdPartyInvoicePaymentInformationStatement>()));
        };

        It should_return_the_invoice_payment_information = () =>
        {
            results.ShouldEqual(expectedResults);
        };
    }
}