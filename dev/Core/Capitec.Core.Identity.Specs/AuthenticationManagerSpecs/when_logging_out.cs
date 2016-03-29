using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.AuthenticationManagerSpecs
{
    public class when_logging_out : WithFakes
    {
        private static IAuthenticationManager authManager;
        private static IUserDataManager userDataManager;
        private static IPasswordManager passwordManager;
        private static IHostContext hostContext;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;

        private Establish context = () =>
        {
            userId = Guid.NewGuid();
            userDataManager = An<IUserDataManager>();
            passwordManager = An<IPasswordManager>();
            hostContext = An<IHostContext>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();

            authManager = new AuthenticationManager(userDataManager, passwordManager, hostContext, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            authManager.Logout(userId);
        };

        private It should_revoke_the_current_security_token = () =>
        {
            hostContext.WasToldTo(x => x.RevokeSecurityToken());
        };

        private It should_remove_the_users_auth_token = () =>
        {
            userDataManager.WasToldTo(x => x.RemoveUserToken(userId));
        };
    }
}