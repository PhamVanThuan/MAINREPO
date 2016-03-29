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

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Domicilium
{
    public class when_retrieving_a_client_role_key : WithFakes
    {
        static IApplicantDataManager applicantDataManager;
        static int clientKey, applicationNumber;
        static FakeDbFactory dbFactory;
        static OfferRoleDataModel expectedApplicationRoleDataModel;
        static OfferRoleDataModel applicationRoleDataModel;

        Establish context = () =>
        {
            applicationNumber = 1548;
            clientKey = 980;
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
            expectedApplicationRoleDataModel = new OfferRoleDataModel(clientKey, 34, 5, (int)GeneralStatus.Active, DateTime.Now);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<OfferRoleDataModel>(Param.IsAny<GetActiveApplicationRoleStatement>())).Return(expectedApplicationRoleDataModel);
        };

        Because of = () =>
        {
            applicationRoleDataModel = applicantDataManager.GetActiveApplicationRole(applicationNumber, clientKey);
        };

        It should_retrieve_the_client_role_key_using_the_domicilium_key = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<OfferRoleDataModel>(Arg.Is<GetActiveApplicationRoleStatement>(y => y.ApplicationNumber == applicationNumber 
                && y.ClientKey == clientKey)));
        };

        It should_return_the_client_role_key = () =>
        {
            applicationRoleDataModel.ShouldEqual(expectedApplicationRoleDataModel);
        };
    }
}
