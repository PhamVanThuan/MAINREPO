using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.AuthenticationManagerSpecs
{
    public class when_authenticating_with_an_ivalid_token : WithFakes
    {
        private static IAuthenticationManager authManager;
        private static IUserDataManager userDataManager;
        private static IPasswordManager passwordManager;
        private static IHostContext hostContext;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid authToken;

        private Establish context = () =>
        {
            userDataManager = An<IUserDataManager>();
            passwordManager = An<IPasswordManager>();
            hostContext = An<IHostContext>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            authToken = Guid.NewGuid();

            userDataManager.WhenToldTo(x => x.GetUserFromToken(authToken))
                .Return((UserDataModel)null);

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

        private It should_not_retrieve_additional_user_info = () =>
        {
            userDataManager.WasNotToldTo(x => x.GetUserInformationFromUser(Param.IsAny<Guid>()));
        };

        private It should_not_retrieve_the_users_roles = () =>
        {
            userDataManager.WasNotToldTo(x => x.GetRolesFromUser(Param.IsAny<Guid>()));
        };

        private It should_not_set_the_authenticated_user_on_the_host_context = () =>
        {
            hostContext.WasNotToldTo(x => x.SetUser(Param.IsAny<CapitecIdentity>(), Param.IsAny<string[]>()));
        };
    }
}