using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicantData
{
    public class when_querying_for_offerrole_attributes : WithFakes
    {
        private static ApplicantDataManager applicantDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static IEnumerable<OfferRoleAttributeDataModel> offerRoleAttributes;
        private static int applicantRoleKey;
        private static IEnumerable<OfferRoleAttributeDataModel> results;

        private Establish context = () =>
        {
            applicantRoleKey = 111115;
            offerRoleAttributes = new OfferRoleAttributeDataModel[] { new OfferRoleAttributeDataModel(12345, applicantRoleKey, (int)OfferRoleAttributeType.IncomeContributor) };
            fakeDbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x=>x.Select(Param.IsAny<GetApplicantAttributesStatement>())).Return(offerRoleAttributes);
        };

        private Because of = () =>
        {
            results = applicantDataManager.GetApplicantAttributes(applicantRoleKey);
        };

        private It should_return_the_collection_from_the_database = () =>
        {
            results.ShouldEqual(offerRoleAttributes);
        };

        private It should_use_the_applicantRoleKey_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select(Arg.Is<GetApplicantAttributesStatement>(
                y => y.ApplicationRoleKey == applicantRoleKey)));
        };

    }
}