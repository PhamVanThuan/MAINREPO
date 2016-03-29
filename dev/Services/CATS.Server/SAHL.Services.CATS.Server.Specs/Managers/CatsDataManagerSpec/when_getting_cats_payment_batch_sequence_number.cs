using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_getting_cats_payment_batch_sequence_number : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int catsSequenceNumber;
        private static int expectedSequenceNumber;
        private static IDbFactory dbFactory;

        private Establish context = () =>
         {
             expectedSequenceNumber = 1212;

             dbFactory = new FakeDbFactory();
             dataManager = new CATSDataManager(dbFactory);
             dbFactory.NewDb().InAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetControlTableValueByControlNumberStatement>()))
                 .Return(expectedSequenceNumber);
         };

        private Because of = () =>
         {
             catsSequenceNumber = dataManager.GetCATSPaymentBatchSequenceNumber();
         };

        private It should_make_a_query_to_get_sequence_number = () =>
         {
             dbFactory.NewDb().InAppContext().WasToldTo(x => x.SelectOne(Param.IsAny<GetControlTableValueByControlNumberStatement>()));
         };

        private It should_get_the_cats_sequence_number_incremented_by_1 = () =>
         {
             catsSequenceNumber.ShouldEqual(expectedSequenceNumber + 1);
         };

        private It should_complete = () =>
         {
             dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
         };
    }
}