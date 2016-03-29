using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityDataServiceSpecs
{
    public class when_getting_a_branch_for_a_user : WithFakes
    {
        private static ISecurityDataManager securityDataService;
        private static FakeDbFactory dbFactory;
        private static BranchDataModel result;
        private static BranchDataModel model;
        private static Guid userId;
        private static Guid branchId;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            securityDataService = new SecurityDataManager(dbFactory);
            userId = Guid.NewGuid();
            branchId = Guid.NewGuid();
            model = new BranchDataModel(branchId, "Hillcrest", Guid.NewGuid(), true, "2017");
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetBranchForUserQuery>())).Return(model);
        };

        private Because of = () =>
        {
            result = securityDataService.GetBranchForUser(userId);
        };

        private It should_return_a_branch_data_model = () =>
        {
            result.ShouldBeOfExactType<BranchDataModel>();
        };

        private It should_contain_the_result_of_the_sql_query = () =>
        {
            result.Id.ShouldEqual(branchId);
        };

        private It should_use_the_params_when_constructing_the_query = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne(Arg.Is<GetBranchForUserQuery>(y => y.UserId == userId)));
        };
    }
}