using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_changing_a_password_for_a_user_that_exists : WithFakes
    {
        private static UserManager userManager;
        private static IUserDataManager userDataManager;
        private static IUserCommunicationService userCommunicationService;
        private static IPasswordManager passwordManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;
        private static string password;
        private static string hashedPassword;

        private Establish context = () =>
        {
            userId = CombGuid.Instance.Generate();
            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.DoesUserIdExist(userId))
                .Return(true);

            userCommunicationService = An<IUserCommunicationService>();

            password = "12345";
            hashedPassword = "54321";
            passwordManager = An<IPasswordManager>();
            passwordManager.WhenToldTo(x => x.HashPassword(password))
                .Return(hashedPassword);
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            userManager = new UserManager(userDataManager, userCommunicationService, passwordManager, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            userManager.ChangePassword(userId, password);
        };

        private It should_change_the_users_password = () =>
        {
            userDataManager.WasToldTo(x => x.ChangeUserPassword(userId, hashedPassword));
        };
    }
}