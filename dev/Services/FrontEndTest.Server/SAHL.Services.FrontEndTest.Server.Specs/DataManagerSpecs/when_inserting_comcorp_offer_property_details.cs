using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_inserting_comcorp_offer_property_details : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyModel;
        private static int offerKey;
        
        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            offerKey = 1711555;
            comcorpOfferPropertyModel = new ComcorpOfferPropertyDetailsDataModel(offerKey,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                                                                                 DateTime.Now, DateTime.Now);
        };

        private Because of = () =>
        {
            testDataManager.InsertComcorpOfferPropertyDetails(comcorpOfferPropertyModel);
        };

        private It should_insert_the_comcorp_offer = () =>
        {

            fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Insert<ComcorpOfferPropertyDetailsDataModel>(Arg.Is<ComcorpOfferPropertyDetailsDataModel>
                    (y => y.OfferKey == offerKey)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}