using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_a_catsPaymentbatch : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static CATSPaymentBatchDataModel model;
        private static int catsPaymentBatchKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            catsPaymentBatchKey = 1;
            model = new CATSPaymentBatchDataModel(catsPaymentBatchKey, DateTime.Now, DateTime.Now, 1, 1, "");
           
        };

        private Because of = () =>
        {
            testDataManager.RemoveCATSPaymentBatch(catsPaymentBatchKey);
        };

        private It should_remove_the_batch = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.DeleteByKey<CATSPaymentBatchDataModel, int>(catsPaymentBatchKey));
        };
    }
}
