using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityDataServiceSpecs
{
    public class when_checking_if_user_belongs_to_any_branches_and_they_do : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static SecurityDataManager service;
        private static Guid userId;
        private static UserBranchDataModel model;
        private static bool result = false;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new SecurityDataManager(dbFactory);
            userId = Guid.NewGuid();
            model = new UserBranchDataModel(Guid.NewGuid(), userId, Guid.NewGuid());
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesUserBelongToAnyBranchesQuery>())).Return(model);
        };

        private Because of = () =>
        {
            result = service.DoesUserBelongToAnyBranches(userId);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}