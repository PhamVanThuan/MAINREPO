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
    [Subject("SAHL.Services.Capitec.Services.SecurityManager.ChangeUserDetails")]
    public class when_changing_user_details : WithFakes
    {
        private static ISecurityManager securityManager;
        private static IUserManager userManager;
        private static ISecurityDataManager securityDataServices;
        private static IGeoDataManager geoDataServices;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Guid id;
        private static string emailAddress;
        private static string firstName;
        private static string lastName;
        private static bool status;
        private static Guid[] rolesToAssign;
        private static Guid[] rolesToRemove;
        private static Guid branchId;

        private Establish context = () =>
        {
            id = CombGuid.Instance.Generate();
            emailAddress = "test";
            firstName = "test";
            lastName = "test";
            status = true;
            rolesToAssign = new Guid[0];
            rolesToRemove = new Guid[0];
            branchId = CombGuid.Instance.Generate();

            userManager = An<IUserManager>();
            geoDataServices = An<IGeoDataManager>();
            securityDataServices = An<ISecurityDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();

            securityDataServices.WhenToldTo(x => x.HasUsersBranchChanged(id, branchId)).Return(true);
            securityDataServices.WhenToldTo(x => x.DoesBranchIdExist(branchId)).Return(true);
            securityDataServices.WhenToldTo(x => x.DoesUserBelongToAnyBranches(id)).Return(true);

            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            securityManager.ChangeUserDetails(id, emailAddress, firstName, lastName, status, rolesToAssign, rolesToRemove, branchId);
        };

        private It should_update_user = () =>
        {
            userManager.WasToldTo(x => x.UpdateUser(id, status));
        };

        private It should_update_status_if_changed = () =>
        {
            userManager.WasToldTo(x => x.UpdateUserInformation(id, emailAddress, firstName, lastName));
        };

        private It should_assign_role = () =>
        {
            userManager.WasToldTo(x => x.AssignUserRole(id, rolesToAssign));
        };

        private It should_remove_roles = () =>
        {
            userManager.WasToldTo(x => x.RemoveUserRole(id, rolesToRemove));
        };

        private It should_check_if_branch_changed = () =>
        {
            securityDataServices.WasToldTo(x => x.HasUsersBranchChanged(id, branchId));
        };

        private It should_update_branch_if_changed = () =>
        {
            securityDataServices.WasToldTo(x => x.UpdateUserToBranchLink(id, branchId));
        };
    }
}