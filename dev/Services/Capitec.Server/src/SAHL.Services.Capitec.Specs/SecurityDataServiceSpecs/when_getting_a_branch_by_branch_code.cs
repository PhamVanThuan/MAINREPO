using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityDataServiceSpecs
{
    public class when_getting_a_branch_by_branch_code : WithFakes
    {
        private static ISecurityDataManager securityDataService;
        private static FakeDbFactory dbFactory;
        private static BranchDataModel result;
        private static BranchDataModel model;
        private static string branchCode;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            securityDataService = new SecurityDataManager(dbFactory);
            branchCode = "2050";
            model = new BranchDataModel("Hillcrest", Guid.NewGuid(), true, branchCode);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetBranchByBranchCodeQuery>())).Return(model);
        };

        private Because of = () =>
        {
            result = securityDataService.GetBranchByBranchCode(branchCode);
        };

        private It should_return_a_branch_data_model = () =>
        {
            result.ShouldBeOfExactType<BranchDataModel>();
        };

        private It should_contain_the_result_of_the_sql_query = () =>
        {
            result.BranchName.ShouldEqual(model.BranchName);
        };

        private It should_use_the_params_when_constructing_the_query = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<BranchDataModel>(Arg.Is<GetBranchByBranchCodeQuery>(y => y.BranchCode == branchCode)));
        };
    }
}