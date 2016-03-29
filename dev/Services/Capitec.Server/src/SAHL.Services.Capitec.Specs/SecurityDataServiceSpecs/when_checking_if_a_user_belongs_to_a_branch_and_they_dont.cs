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
    public class when_checking_if_a_user_belongs_to_a_branch_and_they_dont : WithFakes
    {
        private static SecurityDataManager service;
        private static FakeDbFactory dbFactory;
        private static UserBranchDataModel model;
        private static bool result;
        private static Guid userId;
        private static Guid branchId;

        private Establish context = () =>
        {
            userId = Guid.NewGuid();
            branchId = Guid.NewGuid();
            result = true;
            model = null;
            dbFactory = new FakeDbFactory();
            service = new SecurityDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesUserBelongToBranchQuery>())).Return(model);
        };

        private Because of = () =>
        {
            result = service.DoesUserBelongToBranch(userId, branchId);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}