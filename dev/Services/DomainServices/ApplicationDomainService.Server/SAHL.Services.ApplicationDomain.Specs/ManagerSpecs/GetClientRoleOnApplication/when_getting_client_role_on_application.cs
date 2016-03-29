using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.GetActiveClientRoleOnApplication
{
    public class when_getting_client_role_on_application : WithFakes
    {
        private static ApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<OfferRoleDataModel> expectedResult;
        private static IEnumerable<OfferRoleDataModel> actualResult;
        private static int clientKey;
        private static int applicationNumber;

        private Establish context = () =>
        {
            clientKey = 1234567;
            applicationNumber = 7654321;
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
            expectedResult = new OfferRoleDataModel[] {new OfferRoleDataModel(1,2,(int)OfferRoleType.MainApplicant, (int)GeneralStatus.Active, DateTime.Now),
            new OfferRoleDataModel(1,2,(int)OfferRoleType.MainApplicant,(int)GeneralStatus.Active,DateTime.Now)};

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<OfferRoleDataModel>(Param.IsAny<GetActiveClientRoleOnApplicationStatement>())).Return(() => { return expectedResult; });
        };

        private Because of = () =>
        {
            actualResult = applicantDataManager.GetActiveClientRoleOnApplication(applicationNumber, clientKey);
        };

        private It should_select_offer_role_date_for_a_given_applicant_on_an_application = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<OfferRoleDataModel>(Arg.Is<GetActiveClientRoleOnApplicationStatement>(y => y.ApplicationNumber == applicationNumber 
                && y.ClientKey == clientKey)));
        };

        private It should_return_offer_role_data = () =>
        {
            actualResult.ShouldBeTheSameAs(expectedResult);
        };
    }
}
