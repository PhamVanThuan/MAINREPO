using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_accepting_a_user_invitation_for_a_user_that_exists : WithFakes
    {
        private static UserManager userManager;
        private static IUserDataManager userDataManager;
        private static IUserCommunicationService userCommunicationService;
        private static IPasswordManager passwordManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static string password;
        private static string hashedPassword;
        private static Guid invitationToken;
        private static Guid userId;

        Establish context = () =>
        {
            userId = CombGuid.Instance.Generate();
            invitationToken = CombGuid.Instance.Generate();
            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.ValididateInvitationToken(invitationToken))
                .Return(userId);

            userCommunicationService = An<IUserCommunicationService>();
            passwordManager = An<IPasswordManager>();
            password = "";
            hashedPassword = "##";
            passwordManager.WhenToldTo(x => x.HashPassword(password))
                .Return(hashedPassword);

            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            userManager = new UserManager(userDataManager, userCommunicationService, passwordManager, unitOfWorkFactory);
        };

        Because of = () =>
        {
            userManager.AcceptInvitation(invitationToken, password);
        };

        It should_check_the_token_exists = () =>
        {
            userDataManager.WasToldTo(x => x.ValididateInvitationToken(invitationToken));
        };

        It should_accept_the_invitation = () =>
        {
            userDataManager.WasToldTo(x => x.AcceptInvitationToken(invitationToken));
        };

        It should_hash_the_users_chosen_password = () =>
        {
            passwordManager.WasToldTo(x => x.HashPassword(password));
        };

        It should_update_the_users_password = () =>
        {
            userDataManager.WasToldTo(x => x.SetUserPasswordAndActivate(userId, hashedPassword));
        };
    }
}