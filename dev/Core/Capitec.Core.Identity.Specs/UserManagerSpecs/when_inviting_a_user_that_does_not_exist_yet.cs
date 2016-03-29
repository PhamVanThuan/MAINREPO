using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;
using System.Collections.Generic;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_inviting_a_user_that_does_not_exist_yet : WithFakes
    {
        private static UserManager userManager;
        private static string userName;
        private static string emailAddress;
        private static string firstName;
        private static string lastName;
        private static Guid[] rolesToApply;
        private static IUserDataManager userDataManager;
        private static IUserCommunicationService userCommunicationService;
        private static IPasswordManager passwordManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;

        Establish context = () =>
        {
            userName = "TestUser";
            emailAddress = "testuser@test.co.za";
            firstName = "test";
            lastName = "user";
            rolesToApply = new Guid[] { CombGuid.Instance.Generate(), CombGuid.Instance.Generate() };
            userId = CombGuid.Instance.Generate();
            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.CreateInvitedUser(Param.IsAny<string>()))
                .Return(userId);

            userCommunicationService = An<IUserCommunicationService>();
            passwordManager = An<IPasswordManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            userManager = new UserManager(userDataManager, userCommunicationService, passwordManager, unitOfWorkFactory);
        };

        Because of = () =>
        {
            userManager.InviteUser(userName, emailAddress, firstName, lastName, rolesToApply);
        };

        It should_check_that_the_username_is_available = () =>
        {
            userDataManager.WasToldTo(x => x.DoesUsernameExist(userName));
        };

        It should_persist_a_user_record_that_is_inactive = () =>
        {
            userDataManager.WasToldTo(x => x.CreateInvitedUser(userName));
        };

        It should_persist_a_user_details_record = () =>
        {
            userDataManager.WasToldTo(x => x.AddUserDetails(userId, emailAddress, firstName, lastName));
        };

        It should_create_an_invitation_record = () =>
        {
            userDataManager.WasToldTo(x => x.AddUserInvitation(userId));
        };

        It should_assign_the_supplied_roles = () =>
        {
            userDataManager.WasToldTo(x => x.AssignRolesToUser(userId, rolesToApply));
        };

        It should_notify_the_user_of_the_invitation = () =>
        {
            userCommunicationService.WasToldTo(x => x.SendUserEmail(emailAddress, Param.IsAny<string>(), Param.IsAny<Dictionary<string, string>>()));
        };
    }
}