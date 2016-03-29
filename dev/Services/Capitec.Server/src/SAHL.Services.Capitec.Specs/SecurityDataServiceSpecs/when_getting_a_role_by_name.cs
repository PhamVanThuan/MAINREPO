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
    public class when_getting_a_role_by_name : WithFakes
    {
        private static ISecurityDataManager securityDataService;
        private static FakeDbFactory dbFactory;
        private static string roleName;
        private static RoleDataModel model;
        private static RoleDataModel result;

        private Establish context = () =>
            {
                dbFactory = new FakeDbFactory();
                securityDataService = new SecurityDataManager(dbFactory);
                roleName = "Admin";
                model = new RoleDataModel(Guid.NewGuid(), roleName);
                dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetRoleByName>())).Return(model);
            };

        private Because of = () =>
            {
                result = securityDataService.GetRoleByName(roleName);
            };

        private It should_return_a_role_data_model = () =>
            {
                result.ShouldBeOfExactType<RoleDataModel>();
            };

        private It should_contain_the_result_of_the_sql_query = () =>
            {
                result.Name.ShouldEqual(roleName);
            };

        private It should_use_the_params_when_constructing_the_query = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<RoleDataModel>(Arg.Is<GetRoleByName>(y => y.UserRole == roleName)));
        };
    }
}