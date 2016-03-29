using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_creating_a_valuation
    {
        private static TestDataManager testDataManager;
        private static ValuationDataModel valuationModel;
        private static int valuationKey;
        private static double valuationAmount;
        private static int propertyKey;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
        {
            valuationKey = 777;
            valuationAmount = 9999.99;
            propertyKey = 123;
            valuationModel = new ValuationDataModel(valuationKey, 1, DateTime.Now, valuationAmount, 200000, 43685608, "VishavP"
                                                   , propertyKey, 0.00, 0.00, 00.0, DateTime.Now, 1, 0.00, 123, "", 1, true, 1);
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.InsertValuation(valuationModel);
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        private It should_insert_the_detail_model_for_the_correct_property_and_valuation = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Insert(Arg.Is<ValuationDataModel>(y => y.PropertyKey == propertyKey
                 && y.ValuationKey == valuationKey && y.ValuationAmount == valuationAmount)));
        };
    }
}