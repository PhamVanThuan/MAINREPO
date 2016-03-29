using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_inserting_an_address
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static AddressDataModel addressDataModel;


        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            addressDataModel = new AddressDataModel(1, "", "", "", "", "", "", 1, 1, "", "", "", "", "", "", DateTime.Now, "", "", "", "", "", "");
        };

        private Because of = () =>
        {
            testDataManager.InsertAddress(addressDataModel);
        };

        private It should_insert_the_correct_address = () =>
         {
             fakeDb.FakedDb.InAppContext().
               WasToldTo(x => x.Insert(Arg.Is<AddressDataModel>(y => y.AddressKey == addressDataModel.AddressKey)));
         };

        private It should_complete_the_db_context =()=>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x=>x.Complete());
        };

    }
}
