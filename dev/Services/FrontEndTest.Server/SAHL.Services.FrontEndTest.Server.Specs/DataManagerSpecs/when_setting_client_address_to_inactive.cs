using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_setting_client_address_to_inactive : WithFakes
    {
        private static int clientAddressKey;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
        {
            clientAddressKey = 1;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.SetClientAddressToInactive(clientAddressKey);
        };

        private It should_use_the_correct_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        private It should_set_the__adress_key_for_the_correct_legal_entity = () =>
        {
            fakeDb.FakedDb.InAppContext().
              WasToldTo
               (x => x.Update<LegalEntityAddressDataModel>
                   (Arg.Is<SetClientAddressToInactiveStatement>(y => y.ClientAddressKey == clientAddressKey)));
        };
    }
}