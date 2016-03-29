using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_a_cats_payment_batch : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static CATSPaymentBatchDataModel model;

        private Establish context = () =>
         {
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             model = new CATSPaymentBatchDataModel(1,DateTime.Now,DateTime.Now,1,1,"");
         };

        private Because of = () =>
          {
              testDataManager.UpdateCATSPaymentBatch(model);
          };

        private It should_update_the_batch = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x=>x.Update(model));
         };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}
