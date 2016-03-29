using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.Statements;

namespace SAHL.Services.FinanceDomain.Specs.Managers.FinanceDataManagerSpec
{
    public class when_updating_an_invoice_status : WithFakes
    {
        static IFinanceDataManager financeDataManager;
        static FakeDbFactory dbFactory;
        static int thirdPartyInvoiceKey;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financeDataManager = new FinanceDataManager(dbFactory);
            thirdPartyInvoiceKey = 2005;
        };

        Because of = () =>
        {
            financeDataManager.UpdateInvoiceStatus(thirdPartyInvoiceKey, InvoiceStatus.Paid);
        };

        It should_update_the_invoice_status = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Update(Param<UpdateInvoiceStatusStatement>.Matches(y =>
                y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey
                && y.ThirdPartyInvoiceStatus == (int)InvoiceStatus.Paid)
               ));
        };

        It should_complete_the_activity = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
        };
    }
}
