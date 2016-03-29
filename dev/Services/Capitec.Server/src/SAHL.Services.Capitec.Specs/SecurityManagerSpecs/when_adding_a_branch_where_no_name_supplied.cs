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
    [Subject("SAHL.Services.Capitec.Services.SecurityManager.AddBranch")]
    public class when_adding_a_branch_where_no_name_supplied : WithFakes
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
            branchName = string.Empty;
            geoDataServices = An<IGeoDataManager>();
            securityDataServices = An<ISecurityDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => securityManager.AddBranch(branchName, isActive, suburbId, branchCode));
        };

        private It should_not_check_if_suburb_id_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesSuburbIdExist(suburbId));
        };

        private It should_not_add_branch = () =>
        {
            securityDataServices.WasNotToldTo(x => x.AddBranch(branchName, isActive, suburbId, branchCode));
        };
    }
}