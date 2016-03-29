using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityDataServiceSpecs
{
    public class when_linking_a_user_to_a_branch : WithFakes
    {
        private static SecurityDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid userId, branchId;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new SecurityDataManager(dbFactory);
            userId = Guid.NewGuid();
            branchId = Guid.NewGuid();
        };

        private Because of = () =>
        {
            service.LinkUserToBranch(userId, branchId);
        };

        private It should_create_a_user_branch_data_model_with_the_details_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<UserBranchDataModel>(y => y.BranchId == branchId && y.UserId == userId)));
        };
    }
}