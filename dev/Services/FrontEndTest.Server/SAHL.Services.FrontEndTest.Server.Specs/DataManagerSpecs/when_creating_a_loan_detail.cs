using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_creating_a_loan_detail
    {
        private static TestDataManager testDataManager;
        private static DetailDataModel detailModel;
        private static int detailTypeKey;
        private static int accountKey;
        private static double amount;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
          {
              amount = 999.99;
              detailTypeKey = 12345678;
              accountKey = 1235888;
              detailModel = new DetailDataModel(detailTypeKey, accountKey, DateTime.Now, amount, "Test", 1, "VishavP", DateTime.Now);
              fakeDb = new FakeDbFactory();
              testDataManager = new TestDataManager(fakeDb);
          };

        private Because of = () =>
         {
             testDataManager.InsertLoanDetail(detailModel);
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };

        private It should_insert_the_detail_model = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Insert(Arg.Is<DetailDataModel>(y => y.DetailTypeKey == detailTypeKey
                 && y.AccountKey == accountKey)));
        };
    }
}