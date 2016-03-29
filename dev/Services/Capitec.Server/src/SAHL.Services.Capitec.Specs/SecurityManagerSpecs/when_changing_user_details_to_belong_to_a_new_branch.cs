using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityManagerSpecs
{
    public class when_changing_user_details_to_belong_to_a_new_branch : WithFakes
    {
        private static SecurityManager securityManager;
        private static IUserManager userManager;
        private static ISecurityDataManager securityDataServices;
        private static IGeoDataManager geoDataServices;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Guid id, branchId;
        private static Guid[] rolesToAssign, rolesToRemove;
        private static string emailAddress, firstName, lastName;
        private static bool status;

        private Establish context = () =>
        {
            userManager = An<IUserManager>();
            securityDataServices = An<ISecurityDataManager>();
            geoDataServices = An<IGeoDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
            securityDataServices.WhenToldTo(x => x.DoesBranchIdExist(Param.IsAny<Guid>())).Return(true);
            securityDataServices.WhenToldTo(x => x.HasUsersBranchChanged(Param.IsAny<Guid>(), Param.IsAny<Guid>())).Return(true);
            securityDataServices.WhenToldTo(x => x.DoesUserBelongToAnyBranches(Param.IsAny<Guid>())).Return(false);
            rolesToAssign = new Guid[0];
            rolesToRemove = new Guid[0];
            id = Guid.NewGuid();
            emailAddress = "clintons@sahomeloans.com";
            firstName = "Clint";
            lastName = "Speed";
            status = true;
        };

        private Because of = () =>
        {
            securityManager.ChangeUserDetails(id, emailAddress, firstName, lastName, status, rolesToAssign, rolesToRemove, branchId);
        };

        private It should_link_the_user_to_the_new_branch = () =>
        {
            securityDataServices.WasToldTo(x => x.LinkUserToBranch(Param.Is(id), Param.Is(branchId)));
        };

        private It should_not_update_any_branch_links_for_the_user = () =>
        {
            securityDataServices.WasNotToldTo(x => x.UpdateUserToBranchLink(Param.Is(id), Param.Is(branchId)));
        };
    }
}