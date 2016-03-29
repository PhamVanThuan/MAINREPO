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
    [Subject("SAHL.Services.Capitec.Services.SecurityManager.AddBranch")]
    public class when_adding_a_branch_where_suburbId_invalid : WithFakes
    {
        private static ISecurityManager securityManager;
        private static IUserManager userManager;
        private static ISecurityDataManager securityDataServices;
        private static IGeoDataManager geoDataServices;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static Exception exception;

        private static string branchName;
        private static string branchCode;
        private static bool isActive;
        private static Guid suburbId;

        private Establish context = () =>
        {
            branchName = "test";
            isActive = true;
            suburbId = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            securityDataServices = An<ISecurityDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            geoDataServices.WhenToldTo(x => x.DoesSuburbIdExist(suburbId)).Return(false);

            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => securityManager.AddBranch(branchName, isActive, suburbId, branchCode));
        };

        private It should_check_if_suburb_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSuburbIdExist(suburbId));
        };

        private It should_not_add_branch = () =>
        {
            securityDataServices.WasNotToldTo(x => x.AddBranch(branchName, isActive, suburbId, branchCode));
        };
    }
}