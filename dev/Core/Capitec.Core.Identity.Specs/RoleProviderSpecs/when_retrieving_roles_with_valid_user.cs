using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using SAHL.Core.Roles;
using SAHL.Core.Testing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capitec.Core.Identity.Specs.RoleProviderSpecs
{
    public class when_retrieving_roles_with_valid_user : WithFakes
    {
        private static IUserDataManager userDataManager;
        private static IPasswordManager passwordManager;
        private static IHostContext hostContext;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;
        private static UserDataModel user;
        private static UserInformationDataModel userInfo;
        private static RoleDataModel role;
        private static string username;
        private static string password;
        private static IRoleProvider roleProvider;
        private static List<string> roles;

        private Establish context = () =>
        {
            SAHL.Core.Testing.Ioc.TestingIoc.Initialise();

            userDataManager = An<IUserDataManager>();
            passwordManager = An<IPasswordManager>();
            hostContext = An<IHostContext>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();

            userId = CombGuid.Instance.Generate();
            username = "someone";
            password = "blah";

            // fake user
            user = new UserDataModel(userId, username, password, CombGuid.Instance.Generate(), true, false);
            userDataManager.WhenToldTo(x => x.GetUserFromUsername(username))
                .Return(user);

            // fake user info
            userInfo = new UserInformationDataModel(CombGuid.Instance.Generate(), userId, "pete@petejenkins.com", "Pete", "Jenkins");
            userDataManager.WhenToldTo(x => x.GetUserInformationFromUser(userId))
                .Return(userInfo);

            // fake user role
            role = new RoleDataModel(CombGuid.Instance.Generate(), "User");
            userDataManager.WhenToldTo(x => x.GetRolesFromUser(userId)).Return(new RoleDataModel[] { role });

            // fake the password match
            passwordManager.WhenToldTo(x => x.VerifyHashedPassword(user.PasswordHash, password))
                .Return(true);

            roleProvider = new RoleProvider(userDataManager);
        };

        private Because of = () =>
        {
            roles = roleProvider.GetRoles(username).ToList(); //must call ToList as GetRoles is an iterator
        };

        private It should_retrieve_the_user_for_the_username = () =>
        {
            userDataManager.WasToldTo(x => x.GetUserFromUsername(username));
        };

        private It should_retrieve_the_users_roles = () =>
        {
            userDataManager.WasToldTo(x => x.GetRolesFromUser(userId));
        };

        private It should_have_a_single_role = () =>
        {
            roles.Count.ShouldBeLike(1);
        };

        private It should_have_the_user_role = () =>
        {
            roles.Contains("User").ShouldBeTrue();
        };
    }
}