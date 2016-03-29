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
    public class when_checking_if_a_user_has_changed_branch_and_they_have_not : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static SecurityDataManager service;
        private static Guid userId, branchId;
        private static UserBranchDataModel model;
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            dbFactory = new FakeDbFactory();
            service = new SecurityDataManager(dbFactory);
            userId = Guid.NewGuid();
            branchId = Guid.NewGuid();
            model = new UserBranchDataModel(Guid.NewGuid(), userId, branchId);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<HasUsersBranchChangedQuery>())).Return(model);
        };

        private Because of = () =>
        {
            result = service.HasUsersBranchChanged(userId, branchId);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}