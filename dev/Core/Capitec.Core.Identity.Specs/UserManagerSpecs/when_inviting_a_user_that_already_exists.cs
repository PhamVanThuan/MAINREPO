using Capitec.Core.Identity.Exceptions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_inviting_a_user_that_already_exists : WithFakes
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
        private static Exception exception;

        private Establish context = () =>
        {
            userName = "TestUser";
            emailAddress = "testuser@test.co.za";
            firstName = "test";
            lastName = "user";
            rolesToApply = new Guid[] { CombGuid.Instance.Generate(), CombGuid.Instance.Generate() };
            userId = CombGuid.Instance.Generate();
            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.DoesUsernameExist(userName)).Return(true);
            userCommunicationService = An<IUserCommunicationService>();
            passwordManager = An<IPasswordManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            userManager = new UserManager(userDataManager, userCommunicationService, passwordManager, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => { userManager.InviteUser(userName, emailAddress, firstName, lastName, rolesToApply); });
        };

        private It should_throw_a_UsernameAlreadyExists_exception = () =>
        {
            exception.ShouldBeOfExactType<UsernameAlreadyExistsException>();
        };
    }
}