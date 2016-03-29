using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_assigning_roles_from_a_user_that_does_exist : WithFakes
    {
        private static UserManager userManager;
        private static IUserDataManager userDataManager;
        private static IUserCommunicationService userCommunicationService;
        private static IPasswordManager passwordManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;
        private static Guid[] roleIdsToAssign;

        private Establish context = () =>
        {
            userId = CombGuid.Instance.Generate();
            roleIdsToAssign = new Guid[] { };
            userDataManager = An<IUserDataManager>();
            userDataManager.WhenToldTo(x => x.DoesUserIdExist(userId))
                .Return(true);

            userCommunicationService = An<IUserCommunicationService>();
            passwordManager = An<IPasswordManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            userManager = new UserManager(userDataManager, userCommunicationService, passwordManager, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            userManager.AssignUserRole(userId, roleIdsToAssign);
        };

        private It should_deactivate_the_user = () =>
        {
            userDataManager.WasToldTo(x => x.AssignRolesToUser(userId, roleIdsToAssign));
        };
    }
}