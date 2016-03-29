using Capitec.Core.Identity.Exceptions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.AuthenticationManagerSpecs
{
    public class when_logging_in_with_valid_credentials_but_user_is_inactive : WithFakes
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
        static Exception exception;

        Establish context = () =>
        {
            userDataManager = An<IUserDataManager>();
            passwordManager = An<IPasswordManager>();
            hostContext = An<IHostContext>();

            userId = CombGuid.Instance.Generate();
            username = "someone";
            password = "blah";
            ipaddress = "192.168.1.1";

            // fake user
            user = new UserDataModel(userId, "AUSER", "APASSWORD", CombGuid.Instance.Generate(), false, false);
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

            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            authManager = new AuthenticationManager(userDataManager, passwordManager, hostContext, unitOfWorkFactory);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => authManager.Login(username, password, ipaddress));
        };

        private It should_retrieve_the_user_for_the_username = () =>
        {
            userDataManager.WasToldTo(x => x.GetUserFromUsername(username));
        };

        It should_throw_a_user_is_not_active_exception = () =>
        {
            exception.ShouldBeOfExactType<UserIsNotActiveException>();
        };

        private It should_not_retrieve_additional_user_info = () =>
        {
            userDataManager.WasNotToldTo(x => x.GetUserInformationFromUser(userId));
        };

        private It should_not_retrieve_the_users_roles = () =>
        {
            userDataManager.WasNotToldTo(x => x.GetRolesFromUser(userId));
        };

        It should_not_verify_the_password_matches_the_saved_hash = () =>
        {
            passwordManager.WasNotToldTo(x => x.VerifyHashedPassword(user.PasswordHash, password));
        };

        It should_not_create_a_token_for_the_users_session = () =>
        {
            userDataManager.WasNotToldTo(x => x.UpdateUserToken(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<string>()));
        };

        It should_not_update_the_users_last_session_activity = () =>
        {
            userDataManager.WasNotToldTo(x => x.UpdateUserLoginAndActivity(user.Id));
        };

        It should_not_send_the_authentication_token_back_to_the_user = () =>
        {
            hostContext.WasNotToldTo(x => x.IssueSecurityToken(Param.IsAny<Guid>()));
        };

        private It should_not_set_the_authenticated_user_on_the_host_context = () =>
        {
            hostContext.WasNotToldTo(x => x.SetUser(Param.IsAny<CapitecIdentity>(), Param.IsAny<string[]>()));
        };
    }
}