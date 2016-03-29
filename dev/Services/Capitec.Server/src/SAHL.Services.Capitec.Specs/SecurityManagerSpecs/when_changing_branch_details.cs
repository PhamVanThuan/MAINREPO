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
    [Subject("SAHL.Services.Capitec.Services.SecurityManager.ChangeBrancheDetails")]
    public class when_changing_branch_details : WithFakes
    {
        private static ISecurityManager securityManager;
        private static IUserManager userManager;
        private static ISecurityDataManager securityDataServices;
        private static IGeoDataManager geoDataServices;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Guid id;
        private static string branchName;
        private static bool isActive;
        private static Guid suburbId;

        private Establish context = () =>
        {
            id = CombGuid.Instance.Generate();
            branchName = "test";
            isActive = true;
            suburbId = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            securityDataServices = An<ISecurityDataManager>();

            geoDataServices.WhenToldTo(x => x.DoesSuburbIdExist(suburbId)).Return(true);
            securityDataServices.WhenToldTo(x => x.DoesBranchIdExist(id)).Return(true);

            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            securityManager.ChangeBrancheDetails(id, branchName, isActive, suburbId);
        };

        private It should_check_if_suburb_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSuburbIdExist(suburbId));
        };

        private It should_check_if_branch_id_exists = () =>
        {
            securityDataServices.WasToldTo(x => x.DoesBranchIdExist(id));
        };

        private It should_change_branch_details = () =>
        {
            securityDataServices.WasToldTo(x => x.ChangeBranchDetails(id, branchName, isActive, suburbId));
        };
    }
}