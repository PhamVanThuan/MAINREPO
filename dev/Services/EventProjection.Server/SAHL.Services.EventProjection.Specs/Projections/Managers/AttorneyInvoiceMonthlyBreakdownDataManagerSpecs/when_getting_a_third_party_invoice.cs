using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    public class when_getting_a_third_party_invoice : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static ThirdPartyInvoiceDataModel invoice;
        private static int thirdPartyInvoiceKey;
        private static ThirdPartyInvoiceDataModel result;

        private Establish context = () =>
        {
            thirdPartyInvoiceKey = 1344;
            invoice = new ThirdPartyInvoiceDataModel(1, "sahl_reference", 1, 1428540, Guid.NewGuid(), "invoice_number", null, "clintons@sahomeloans.com", 0.00M, 0.00M, 0.00M, true, 
                DateTime.Now, "payment_reference");
            fakeDbFactory = new FakeDbFactory();
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetThirdPartyInvoiceStatement>())).Return(invoice);
            dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            result = dataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(thirdPartyInvoiceKey);
        };

        private It should_use_the_correct_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetThirdPartyInvoiceStatement>
                .Matches(y => y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)));
        };

        private It should_return_the_result_of_the_statement = () =>
        {
            result.ShouldEqual(invoice);
        };
    }
}
