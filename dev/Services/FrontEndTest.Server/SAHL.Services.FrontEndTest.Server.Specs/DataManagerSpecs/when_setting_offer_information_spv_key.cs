using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_setting_offer_information_spv_key : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int applicationInformationKey, spvkey;

        private Establish context = () =>
        {
            applicationInformationKey = 1;
            spvkey = 1;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.SetOfferInformationSPV(applicationInformationKey, spvkey);
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        private It should_use_the_correct_spv_key_and_application_info_key = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Update<OfferInformationVariableLoanDataModel>(
                 Arg.Is<SetOfferInformationSPVStatement>(y => y.SPVKey == spvkey && y.ApplicationInformationKey == applicationInformationKey)));
        };
    }
}