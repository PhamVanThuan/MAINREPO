using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using System;

namespace Capitec.Core.Identity.Specs.UserManagerSpecs
{
    public class when_removing_roles_from_a_user_that_does_exist : WithFakes
    {
        private static UserManager userManager;
        private static IUserDataManager userDataManager;
        private static IUserCommunicationService userCommunicationService;
        private static IPasswordManager passwordManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid userId;
        private static Guid[] roleIdsToRemove;

        private Establish context = () =>
        {
            userId = CombGuid.Instance.Generate();
            roleIdsToRemove = new Guid[] { };
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
            userManager.RemoveUserRole(userId, roleIdsToRemove);
        };

        private It should_deactivate_the_user = () =>
        {
            userDataManager.WasToldTo(x => x.RemoveRolesFromUser(userId, roleIdsToRemove));
        };
    }
}