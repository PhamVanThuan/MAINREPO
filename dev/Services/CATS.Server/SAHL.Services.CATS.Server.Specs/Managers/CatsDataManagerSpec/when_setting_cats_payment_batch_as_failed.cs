using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;
using SAHL.Services.CATS.Utils;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_setting_cats_payment_batch_as_failed : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int catsPaymentBatchKey;

        private static IDbFactory dbFactory;

        private Establish context = () =>
         {
             catsPaymentBatchKey = 3434;
             dbFactory = new FakeDbFactory();
             dataManager = new CATSDataManager(dbFactory);
         };

        private Because of = () =>
         {
             dataManager.SetCATSPaymentBatchAsFailed(catsPaymentBatchKey);
         };

        private It should_update_the_cats_payment_batch_status_to_failed = () =>
         {
             dbFactory.NewDb().InAppContext().WasToldTo(x => x.ExecuteNonQuery(Arg.Is<SetCATSPaymentBatchAsFailedStatement>(
                 y => y.CATSPaymentBatchKey == catsPaymentBatchKey)));
         };

        private It should_complete = () =>
         {
             dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
         };
    }
}