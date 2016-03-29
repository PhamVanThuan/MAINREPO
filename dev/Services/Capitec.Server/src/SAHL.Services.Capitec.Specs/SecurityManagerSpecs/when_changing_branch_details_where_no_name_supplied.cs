using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Capitec.Managers.Security;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.SecurityManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.SecurityManager.ChangeBrancheDetails")]
    public class when_changing_branch_details_where_no_name_supplied : WithFakes
    {
        private static ISecurityManager securityManager;
        private static IUserManager userManager;
        private static ISecurityDataManager securityDataServices;
        private static IGeoDataManager geoDataServices;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Exception exception;

        private static Guid id;
        private static string branchName;
        private static bool isActive;
        private static Guid suburbId;

        private Establish context = () =>
        {
            geoDataServices = An<IGeoDataManager>();
            securityDataServices = An<ISecurityDataManager>();

            geoDataServices.WhenToldTo(x => x.DoesSuburbIdExist(suburbId)).Return(true);
            securityDataServices.WhenToldTo(x => x.DoesBranchIdExist(id)).Return(true);

            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => securityManager.ChangeBrancheDetails(id, branchName, isActive, suburbId));
        };

        private It should_not_check_if_suburb_id_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesSuburbIdExist(suburbId));
        };

        private It should_not_check_if_branch_id_exists = () =>
        {
            securityDataServices.WasNotToldTo(x => x.DoesBranchIdExist(id));
        };

        private It should_not_change_branch_details = () =>
        {
            securityDataServices.WasNotToldTo(x => x.ChangeBranchDetails(id, branchName, isActive, suburbId));
        };
    }
}