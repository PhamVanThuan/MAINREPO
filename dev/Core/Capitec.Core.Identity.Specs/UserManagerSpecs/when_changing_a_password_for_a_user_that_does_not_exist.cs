using Capitec.Core.Identity.Exceptions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_changing_a_password_for_a_user_that_does_not_exist : WithFakes
    {
        private static UserManager userManager;
        private static IUserDataManager userDataManager;
        private static IUserCommunicationService userCommunicationService;
        private static IPasswordManager passwordManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;
        private static string password;
        private static Exception exception;

        private Establish context = () =>
        {
            userId = CombGuid.Instance.Generate();
            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.DoesUserIdExist(userId))
                .Return(false);

            userCommunicationService = An<IUserCommunicationService>();
            passwordManager = An<IPasswordManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            userManager = new UserManager(userDataManager, userCommunicationService, passwordManager, unitOfWorkFactory);
            password = "";
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => { userManager.ChangePassword(userId, password); });
        };

        private It should_throw_an_InvalidUserId_exception = () =>
        {
            exception.ShouldBeOfExactType<UserDoesNotExistException>();
        };
    }
}