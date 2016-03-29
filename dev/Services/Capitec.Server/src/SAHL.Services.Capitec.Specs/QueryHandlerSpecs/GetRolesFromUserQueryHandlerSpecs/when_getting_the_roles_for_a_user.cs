using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Web.Services;
using SAHL.Services.Capitec.QueryHandlers;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.GetRolesFromUserQueryHandlerSpecs
{
    public class when_getting_the_roles_for_a_user : WithFakes
    {
        private static IUserDataManager userDataManager;
        private static IEnumerable<RoleDataModel> roles;
        private static GetRolesFromUserQueryHandler handler;
        private static GetRolesFromUserQuery query;
        private static Guid userId;

        Establish context = () =>
        {
            userId = Guid.NewGuid();
            roles = new List<RoleDataModel>(){ new RoleDataModel("TestRole1"), new RoleDataModel("TestRole2") };
            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.GetRolesFromUser(Param.IsAny<Guid>())).Return(roles);
            handler = new GetRolesFromUserQueryHandler(userDataManager);
            query = new GetRolesFromUserQuery(userId);
        };

        Because of = () =>
        {
            handler.HandleQuery(query);
        };

        It should_return_the_roles = () =>
        {
            query.Result.Results.First().Name.ShouldEqual(roles.First().Name);
            query.Result.Results.Last().Name.ShouldEqual(roles.Last().Name);
        };

        It should_use_the_UserId_provided = () =>
        {
            userDataManager.WasToldTo(x => x.GetRolesFromUser(userId));
        };
    }
}
