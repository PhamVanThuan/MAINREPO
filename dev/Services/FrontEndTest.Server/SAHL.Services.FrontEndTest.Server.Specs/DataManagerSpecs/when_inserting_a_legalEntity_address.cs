using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class when_inserting_a_legalEntity_address
    {
        private static TestDataManager testDataManager;
        private static LegalEntityAddressDataModel legalEntityAddressDataModel;
        private static FakeDbFactory fakeDb;
        private static int LegalEntityAddressKey;

        private Establish context = () =>
        {
            LegalEntityAddressKey = 123;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            legalEntityAddressDataModel = new LegalEntityAddressDataModel(LegalEntityAddressKey, 1, 1, DateTime.Now, (int)GeneralStatus.Pending);
        };

        private Because of = () =>
        {
            testDataManager.InsertLegalEntityAddress(legalEntityAddressDataModel);
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        private It should_insert_the_correct_legalEntityAddress = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Insert(Arg.Is<LegalEntityAddressDataModel>(y => y.AddressKey == legalEntityAddressDataModel.AddressKey)));
        };
    }
}
