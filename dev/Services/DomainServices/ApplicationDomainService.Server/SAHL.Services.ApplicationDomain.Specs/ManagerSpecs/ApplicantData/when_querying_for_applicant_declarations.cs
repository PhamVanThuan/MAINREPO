using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicantData
{
    public class when_querying_for_applicant_declarations : WithFakes
    {
        private static ApplicantDataManager applicantDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static IEnumerable<OfferDeclarationDataModel> offerDeclarations;
        private static int applicationNumber;
        private static int clientKey;
        private static IEnumerable<OfferDeclarationDataModel> results;

        private Establish context = () =>
        {
            applicationNumber = 111115;
            clientKey = 2222;
            offerDeclarations = new OfferDeclarationDataModel[] { new OfferDeclarationDataModel(1, 2, 3, null) };
            fakeDbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.Select(Param.IsAny<GetApplicantDeclarationsStatement>())).Return(offerDeclarations);
        };

        private Because of = () =>
        {
            results = applicantDataManager.GetApplicantDeclarations(applicationNumber, clientKey);
        };

        private It should_return_the_collection_from_the_database = () =>
        {
            results.ShouldEqual(offerDeclarations);
        };

        private It should_use_the_applicationNumber_and_clientKey_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select(Arg.Is<GetApplicantDeclarationsStatement>(
                y => y.ApplicationNumber == applicationNumber)));
        };
    }
}