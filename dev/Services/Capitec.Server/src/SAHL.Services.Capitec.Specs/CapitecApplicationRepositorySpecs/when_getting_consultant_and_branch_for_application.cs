using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Capitec.Managers.CapitecApplication;
using SAHL.Services.Capitec.Managers.CapitecApplication.Models;
using SAHL.Services.Capitec.Managers.CapitecApplication.Statements;
using System;

namespace SAHL.Services.Capitec.Specs.CapitecApplicationRepositorySpecs
{
    public class when_getting_consultant_and_branch_for_application : WithCoreFakes
    {
        private static CapitecApplicationDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid applicationId;
        private static CapitecUserBranchMappingModel result;
        private static CapitecUserBranchMappingModel userBranchModel;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new CapitecApplicationDataManager(fakeDbFactory);

            applicationId = Guid.Parse("{22EC5E9B-3DC6-4569-904F-E218A78E77A9}");
            userBranchModel = new CapitecUserBranchMappingModel(Guid.Parse("{8579FA90-9108-4C00-918C-28F0D3ECEB7B}"), Guid.Parse("{3C1FCC2F-FCB9-4A0F-A674-2A89ABC6CA83}"), "Sally", "South", 9898);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetCapitecUserBranchForApplicationQuery>())).Return(userBranchModel);
        };

        private Because of = () =>
        {
            result = dataManager.GetCapitecUserBranchMappingForApplication(applicationId);
        };

        private It should_get_the_capitec_consultant_details_from_the_db = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetCapitecUserBranchForApplicationQuery>.Matches(m =>
                m.ApplicationId == applicationId)));
        };

        private It should_return_the_capitec_branch_mapping = () =>
        {
            result.ShouldMatch(m =>
                m.ApplicationNumber == userBranchModel.ApplicationNumber &&
                m.BranchName == userBranchModel.BranchName &&
                m.UserName == userBranchModel.UserName
            );
        };
    }
}