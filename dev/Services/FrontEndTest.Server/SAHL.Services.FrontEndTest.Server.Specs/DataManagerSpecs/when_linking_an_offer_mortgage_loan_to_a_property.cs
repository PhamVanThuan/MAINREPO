using Machine.Specifications;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Core.Testing.Fakes;
using Machine.Fakes;
using NSubstitute;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_linking_an_offer_mortgage_loan_to_a_property
    {
        private static int offerKey;
        private static int propertyKey;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
        {
            propertyKey = 12091992;
            offerKey = 299129021;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.LinkOfferMortgageLoanProperty(offerKey,propertyKey);
        };

        private It should_link_the_property_to_the_offer = () =>
        {
            fakeDb.FakedDb.InAppContext().
                   WasToldTo(x => x.ExecuteNonQuery(Arg.Is(Param.IsAny<LinkOfferMortgageLoanPropertyStatement>())));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}
