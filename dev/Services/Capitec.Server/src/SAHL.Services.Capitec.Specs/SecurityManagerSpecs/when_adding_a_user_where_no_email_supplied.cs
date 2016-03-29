using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Capitec.Managers.Security;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.SecurityManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.SecurityManager.AddUser")]
    public class when_adding_a_user_where_no_email_supplied : WithFakes
    {
        private static ISecurityManager securityManager;
        private static IUserManager userManager;
        private static ISecurityDataManager securityDataServices;
        private static IGeoDataManager geoDataServices;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Exception exception;

        private static string username;
        private static string emailAddress;
        private static string firstName;
        private static string lastName;
        private static Guid[] rolesToAssign;
        private static Guid branchId;

        private static Guid userId;

        private Establish context = () =>
        {
            username = "test";
            firstName = "test";
            lastName = "test";
            rolesToAssign = new Guid[0];
            branchId = CombGuid.Instance.Generate();
            userId = CombGuid.Instance.Generate();

            userManager = An<IUserManager>();
            geoDataServices = An<IGeoDataManager>();
            securityDataServices = An<ISecurityDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();

            userManager.WhenToldTo(x => x.InviteUser(username, emailAddress, firstName, lastName, rolesToAssign)).Return(userId);

            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => securityManager.AddUser(username, emailAddress, firstName, lastName, rolesToAssign, branchId));
        };

        private It should_not_check_if_branch_id_exists = () =>
        {
            securityDataServices.WasNotToldTo(x => x.DoesBranchIdExist(branchId));
        };

        private It should_not_invite_user = () =>
        {
            userManager.WasNotToldTo(x => x.InviteUser(username, emailAddress, firstName, lastName, rolesToAssign));
        };

        private It should_not_link_user_to_branch = () =>
        {
            securityDataServices.WasNotToldTo(x => x.LinkUserToBranch(userId, branchId));
        };
    }
}