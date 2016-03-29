using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using System;

namespace Capitec.Core.Identity.Specs.AuthenticationManagerSpecs
{
    public class when_logging_in_with_valid_credentials : WithFakes
    {
        private static IAuthenticationManager authManager;
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
        private static string ipaddress;

        Establish context = () =>
        {
            SAHL.Core.Testing.Ioc.TestingIoc.Initialise();

            userDataManager = An<IUserDataManager>();
            passwordManager = An<IPasswordManager>();
            hostContext = An<IHostContext>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();

            userId = CombGuid.Instance.Generate();
            username = "someone";
            password = "blah";
            ipaddress = "192.168.1.1";

            // fake user
            user = new UserDataModel(userId, "AUSER", "APASSWORD", CombGuid.Instance.Generate(), true, false);
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

            authManager = new AuthenticationManager(userDataManager, passwordManager, hostContext, unitOfWorkFactory);
        };

        Because of = () =>
        {
            authManager.Login(username, password, ipaddress);
        };

        private It should_retrieve_the_user_for_the_username = () =>
        {
            userDataManager.WasToldTo(x => x.GetUserFromUsername(username));
        };

        private It should_retrieve_additional_user_info = () =>
        {
            userDataManager.WasToldTo(x => x.GetUserInformationFromUser(userId));
        };

        private It should_retrieve_the_users_roles = () =>
        {
            userDataManager.WasToldTo(x => x.GetRolesFromUser(userId));
        };

        It should_verify_the_password_matches_the_saved_hash = () =>
        {
            passwordManager.WasToldTo(x => x.VerifyHashedPassword(user.PasswordHash, password));
        };

        It should_create_a_token_for_the_users_session = () =>
        {
            userDataManager.WasToldTo(x => x.UpdateUserToken(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<string>()));
        };

        It should_update_the_users_last_session_activity = () =>
        {
            userDataManager.WasToldTo(x => x.UpdateUserLoginAndActivity(user.Id));
        };

        It should_send_the_authentication_token_back_to_the_user = () =>
        {
            hostContext.WasToldTo(x => x.IssueSecurityToken(Param.IsAny<Guid>()));
        };

        private It should_set_the_authenticated_user_on_the_host_context = () =>
        {
            hostContext.WasToldTo(x => x.SetUser(Param.IsAny<CapitecIdentity>(), Param.IsAny<string[]>()));
        };
    }
}