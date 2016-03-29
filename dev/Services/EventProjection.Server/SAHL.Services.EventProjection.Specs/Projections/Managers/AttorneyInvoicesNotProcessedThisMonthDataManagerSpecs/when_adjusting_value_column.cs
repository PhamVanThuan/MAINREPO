using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesNotProcessedThisMonthDataManagerSpecs
{
    public class when_adjusting_value_column : WithFakes
    {
        private static AttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static decimal adjustment;

        private Establish context = () =>
        {
            adjustment = 525M;
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesNotProcessedThisMonthDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.AdjustMonthlyValue(adjustment);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<AdjustMonthlyValueStatement>
                .Matches(y => y.Adjustment == adjustment)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}