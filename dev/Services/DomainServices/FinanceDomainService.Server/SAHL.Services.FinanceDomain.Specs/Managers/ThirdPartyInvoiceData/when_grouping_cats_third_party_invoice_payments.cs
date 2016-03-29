using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_grouping_cats_third_party_invoice_payments : WithFakes
    {
        private static IThirdPartyInvoiceManager thirdPartyInvoiceManager;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IEnumerable<ThirdPartyInvoicePaymentModel> thirdPartyInvoicePayments;
        private static IDictionary<string, IEnumerable<ThirdPartyInvoicePaymentModel>> results;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceManager = new ThirdPartyInvoiceManager(thirdPartyInvoiceDataManager);

            thirdPartyInvoicePayments = new List<ThirdPartyInvoicePaymentModel> { 
                new ThirdPartyInvoicePaymentModel("Strauss daly","strausdaly@sd.com", 1408,1114M,"Invoice Num 32", 8347 )
            };
        };

        Because of = () =>
        {
            results = thirdPartyInvoiceManager.GroupThirdPartyPaymentInvoicesByThirdParty(thirdPartyInvoicePayments);
        };

        It should_return_a_dictionary_grouping_the_payments_by_the_firm_name = () =>
        {
            results["Strauss daly"].ShouldEqual(thirdPartyInvoicePayments);
        };
    }
}
