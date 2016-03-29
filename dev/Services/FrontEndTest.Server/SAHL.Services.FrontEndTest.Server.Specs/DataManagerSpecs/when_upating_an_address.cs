using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_upating_an_address
    {
        private static TestDataManager testDataManager;
        private static AddressDataModel addressModel;
        private static int addressFormatKey;
        private static FakeDbFactory fakeDb;

        private Establish context =()=>
        {
            addressFormatKey = 999;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            addressModel = new AddressDataModel(addressFormatKey, "", "", "", "", "", "", 1, 1, "", "", "", "", "", "", DateTime.Now, "", "", "", "", "", "");
        };

        private Because of = () =>
        {
            testDataManager.UpdateAddress(addressModel);
        };

        private It should_update_the_correct_address = () =>
         {
             fakeDb.FakedDb.InAppContext().
               WasToldTo(x => x.Update(Arg.Is<AddressDataModel>(y => y == addressModel)));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };

    }
}
