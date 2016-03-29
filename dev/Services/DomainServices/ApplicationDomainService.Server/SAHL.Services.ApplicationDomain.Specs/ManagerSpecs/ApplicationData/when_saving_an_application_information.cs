using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_an_application_information : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static int applicationNumber, applicationInformationKey;
        private static OfferInformationDataModel offerInformation;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            applicationNumber = 1;
            applicationInformationKey = 2;
            offerInformation = new OfferInformationDataModel(DateTime.Now, applicationNumber, (int)OfferInformationType.OriginalOffer, "System", DateTime.Now, (int)Product.NewVariableLoan);

            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<OfferInformationDataModel>(Param.IsAny<OfferInformationDataModel>()))
                .Callback<OfferInformationDataModel>(y => { y.OfferInformationKey = applicationInformationKey; });
        };

        private Because of = () =>
        {
            applicationInformationKey = applicationDataManager.SaveApplicationInformation(offerInformation);
        };

        private It should_insert_a_new_offer_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferInformationDataModel>(offerInformation));
        };

        private It should_return_the_inserted_offer_key = () =>
        {
            offerInformation.OfferInformationKey.ShouldEqual(applicationInformationKey);
        };
    }
}