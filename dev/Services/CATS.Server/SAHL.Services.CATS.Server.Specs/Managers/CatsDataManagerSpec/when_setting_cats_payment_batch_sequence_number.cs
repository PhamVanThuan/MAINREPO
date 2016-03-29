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
    public class when_setting_cats_payment_batch_sequence_number : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int catsPaymentSequenceNumber;

        private static IDbFactory dbFactory;

        private Establish context = () =>
         {
             catsPaymentSequenceNumber = 1212;
             dbFactory = new FakeDbFactory();
             dataManager = new CATSDataManager(dbFactory);
         };

        private Because of = () =>
         {
             dataManager.SetCatsPaymentBatchSequenceNumber(catsPaymentSequenceNumber);
         };

        private It should_update_the_cats_sequence_number = () =>
         {
             dbFactory.NewDb().InAppContext().WasToldTo(x => x.ExecuteNonQuery(Arg.Is<UpdateControlTableNumericValueStatement>(
                 y => y.ControlNumber == (int)CatsControlType.DisbursementInProcess)));
         };

        private It should_complete = () =>
         {
             dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
         };
    }
}