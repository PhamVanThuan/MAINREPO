using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_linking_domicilium_address_to_applicant : WithFakes
    {
        static ApplicantDataManager applicationDataManager;
        static OfferRoleDomiciliumDataModel offerRoleDomiciliumDataModel;
        static FakeDbFactory dbFactory;
        static int offerRoleDomiciliumKey;
        static int expectedOfferRoleDomiciliumKey;

        Establish context = () =>
        {
            offerRoleDomiciliumDataModel = new OfferRoleDomiciliumDataModel(173, 100, DateTime.Now, 155);
            expectedOfferRoleDomiciliumKey = 128;
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicantDataManager(dbFactory);
            
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<OfferRoleDomiciliumDataModel>(Param.IsAny<OfferRoleDomiciliumDataModel>()))
                .Callback<OfferRoleDomiciliumDataModel>(y => { y.OfferRoleDomiciliumKey = expectedOfferRoleDomiciliumKey; });
        };

        Because of = () =>
        {
            offerRoleDomiciliumKey = applicationDataManager.LinkDomiciliumAddressToApplicant(offerRoleDomiciliumDataModel);
        };

        It should_link_the_domicilium_address_to_the_applicant = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferRoleDomiciliumDataModel>(Arg.Is<OfferRoleDomiciliumDataModel>(
                y => y.LegalEntityDomiciliumKey == offerRoleDomiciliumDataModel.LegalEntityDomiciliumKey)));
        };

        private It should_return_a_system_generated_client_address_key = () =>
        {
            offerRoleDomiciliumKey.ShouldEqual(expectedOfferRoleDomiciliumKey);
        };

    }
}
