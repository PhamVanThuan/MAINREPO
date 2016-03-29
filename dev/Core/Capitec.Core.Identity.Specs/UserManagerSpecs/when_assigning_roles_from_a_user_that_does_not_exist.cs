using Capitec.Core.Identity.Exceptions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_assigning_roles_from_a_user_that_does_not_exist : WithFakes
    {
        private static UserManager userManager;
        private static IUserDataManager userDataManager;
        private static IUserCommunicationService userCommunicationService;
        private static IPasswordManager passwordManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;
        private static Exception exception;
        private static Guid[] roleIdsToAssign;

        private Establish context = () =>
        {
            userId = CombGuid.Instance.Generate();
            roleIdsToAssign = new Guid[] { };

            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.DoesUserIdExist(userId))
                .Return(false);

            userCommunicationService = An<IUserCommunicationService>();
            passwordManager = An<IPasswordManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            userManager = new UserManager(userDataManager, userCommunicationService, passwordManager, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => { userManager.AssignUserRole(userId, roleIdsToAssign); });
        };

        private It should_throw_an_InvalidUserId_exception = () =>
        {
            exception.ShouldBeOfExactType<UserDoesNotExistException>();
        };
    }
}