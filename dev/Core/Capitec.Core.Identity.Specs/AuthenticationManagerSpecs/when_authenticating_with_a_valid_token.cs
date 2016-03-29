using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.AuthenticationManagerSpecs
{
    public class when_authenticating_with_a_valid_token : WithFakes
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

        private static Guid authToken;

        private Establish context = () =>
        {
            userDataManager = An<IUserDataManager>();
            passwordManager = An<IPasswordManager>();
            hostContext = An<IHostContext>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();

            userId = CombGuid.Instance.Generate();
            authToken = Guid.NewGuid();
            // fake user
            user = new UserDataModel(userId, "AUSER", "APASSWORD", CombGuid.Instance.Generate(), true, false);
            userDataManager.WhenToldTo(x => x.GetUserFromToken(authToken))
                .Return(user);

            // fake user info
            userInfo = new UserInformationDataModel(CombGuid.Instance.Generate(), userId, "pete@petejenkins.com", "Pete", "Jenkins");
            userDataManager.WhenToldTo(x => x.GetUserInformationFromUser(userId))
                .Return(userInfo);

            // fake user role
            role = new RoleDataModel(CombGuid.Instance.Generate(), "User");
            userDataManager.WhenToldTo(x => x.GetRolesFromUser(userId)).Return(new RoleDataModel[] { role });

            authManager = new AuthenticationManager(userDataManager, passwordManager, hostContext, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            authManager.Authenticate(authToken);
        };

        private It should_retrieve_the_user_for_the_token = () =>
        {
            userDataManager.WasToldTo(x => x.GetUserFromToken(authToken));
        };

        private It should_retrieve_additional_user_info = () =>
        {
            userDataManager.WasToldTo(x => x.GetUserInformationFromUser(userId));
        };

        private It should_retrieve_the_users_roles = () =>
        {
            userDataManager.WasToldTo(x => x.GetRolesFromUser(userId));
        };

        private It should_set_the_authenticated_user_on_the_host_context = () =>
        {
            hostContext.WasToldTo(x => x.SetUser(Param.IsAny<CapitecIdentity>(), Param.IsAny<string[]>()));
        };

        It should_send_the_authentication_token_back_to_the_user = () =>
        {
            hostContext.WasToldTo(x => x.IssueSecurityToken(Param.IsAny<Guid>()));
        };
    }
}