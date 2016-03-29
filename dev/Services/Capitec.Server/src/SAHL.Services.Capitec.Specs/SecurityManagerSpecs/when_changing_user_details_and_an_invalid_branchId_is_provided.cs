﻿using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityManagerSpecs
{
    public class when_changing_user_details_and_an_invalid_branchId_is_provided : WithFakes
    {
        private static System.Exception exception;
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
            branchId = Guid.NewGuid();
            securityDataServices.WhenToldTo(x => x.DoesBranchIdExist(Param.Is(branchId))).Return(false);
            securityManager = new SecurityManager(userManager, securityDataServices, geoDataServices, unitOfWorkFactory);
            rolesToAssign = new Guid[0];
            rolesToRemove = new Guid[0];
            id = Guid.NewGuid();
            branchId = Guid.NewGuid();
            emailAddress = "clintons@sahomeloans.com";
            firstName = "Clint";
            lastName = "Speed";
            status = true;
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
                securityManager.ChangeUserDetails(id, emailAddress, firstName, lastName, status, rolesToAssign, rolesToRemove, branchId));
        };

        private It should_throw_an_argument_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(ArgumentException));
        };

        private It should_throw_a_custom_exception_message = () =>
        {
            exception.ShouldContainErrorMessage("Branch id is invalid");
        };
    }
}
