using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_inserting_a_cats_payment_batch : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static TestDataManager testDataManager;
        private static CATSPaymentBatchDataModel model;
        private static int paymentBatchKey;

        private Establish context = () =>
         {
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             paymentBatchKey = 123;
             model = new CATSPaymentBatchDataModel(paymentBatchKey, DateTime.Now, DateTime.Now, 1, 1, "");
         };

        private Because of = () =>
         {
             testDataManager.InsertCatsPaymentBatch(model);
         };

        private It should_insert_the_paymentBatch = () =>
         {
             fakeDb.FakedDb.InAppContext().
                WasToldTo(x=>x.Insert(model));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };
    }
}
