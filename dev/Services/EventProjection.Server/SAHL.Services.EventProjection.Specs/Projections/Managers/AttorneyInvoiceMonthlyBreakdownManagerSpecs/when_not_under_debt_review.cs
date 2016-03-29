using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownManagerSpecs
{
    public class when_not_under_debt_review : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakdownDataManager;
        private static int accountKey;
        private static DebtCounsellingDataModel model;
        private static bool result;

        private Establish that = () =>
        {
            result = false;
            accountKey = 1234567;
            model = null;
            attorneyInvoiceMonthlyBreakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            attorneyInvoiceMonthlyBreakdownDataManager.WhenToldTo(x => x.GetOpenDebtCounsellingByAccountKey(accountKey)).Return(model);
            attorneyInvoiceMonthlyBreakdownManager = new AttorneyInvoiceMonthlyBreakdownManager(attorneyInvoiceMonthlyBreakdownDataManager);
        };

        private Because of = () =>
        {
            result = attorneyInvoiceMonthlyBreakdownManager.IsAccountUnderDebtCounselling(accountKey);
        };

        private It should_use_the_correct_params = () =>
        {
            attorneyInvoiceMonthlyBreakdownDataManager.GetOpenDebtCounsellingByAccountKey(accountKey);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
