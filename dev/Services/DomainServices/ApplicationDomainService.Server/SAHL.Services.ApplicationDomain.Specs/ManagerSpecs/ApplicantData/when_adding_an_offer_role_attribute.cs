using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Applicant;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicantData
{
    public class when_adding_an_offer_role_attribute : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static OfferRoleAttributeDataModel offerRoleAttribute;
        private static FakeDbFactory fakedDb;
        private static int offerRoleKey = 7;
        private static int offerRoleAttributeTypeKey = 8;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            offerRoleAttribute = new OfferRoleAttributeDataModel(offerRoleKey, offerRoleAttributeTypeKey);
            applicantDataManager = new ApplicantDataManager(fakedDb);
        };

        private Because of = () =>
        {
            applicantDataManager.AddOfferRoleAttribute(offerRoleAttribute);
        };

        private It should_insert_an_offer_attribute = () =>
        {
            fakedDb.FakedDb.InAppContext().WasToldTo(x => x.Insert(Arg.Is<OfferRoleAttributeDataModel>(m => m.OfferRoleKey == offerRoleKey 
                && m.OfferRoleAttributeTypeKey == offerRoleAttributeTypeKey)));
        };
    }
}