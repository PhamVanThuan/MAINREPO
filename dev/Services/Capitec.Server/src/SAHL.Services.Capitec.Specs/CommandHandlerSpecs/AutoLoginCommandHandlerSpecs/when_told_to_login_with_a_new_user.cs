using Capitec.Core.Identity;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Services.Capitec.CommandHandlers;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Core.Data;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AutoLoginCommandHandlerSpecs
{
    public class when_told_to_login_with_a_new_user : WithAutoLoginCommandFakes
    {
        private static string userName;
        private static string password;
        private static string branchCode;
        private static Guid userId;
        private static Guid roleId;
        private static Guid branchId;
        static ServiceRequestMetadata metadata;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        Establish context = () =>
            {
                authenticationManager = An<IAuthenticationManager>();
                securityDataServices = An<ISecurityDataManager>();
                userDataManager = An<IUserDataManager>();
                securityManager = An<ISecurityManager>();
                passwordManager = An<IPasswordManager>();
                unitOfWorkFactory = An<IUnitOfWorkFactory>();
                userName = "username";
                password = "password";
                branchCode = "";
                userId = Guid.NewGuid();
                roleId = Guid.NewGuid();
                branchId = Guid.NewGuid();

                commandHandler = new AutoLoginCommandHandler(authenticationManager,
                    securityDataServices,
                    userDataManager,
                    securityManager,
                    passwordManager,
                    unitOfWorkFactory);

                securityDataServices
                    .WhenToldTo(p => p.GetBranchByBranchCode(Param<string>.Matches(m => m == branchCode)))
                    .Return(new BranchDataModel(branchId, "branchName", Guid.NewGuid(), true, branchCode));

                userDataManager
                    .WhenToldTo(p => p.DoesUsernameExist(Param<string>.Matches(m => m == userName)))
                    .Return(false);

                passwordManager
                    .WhenToldTo(p => p.HashPassword(Param<string>.Matches(m => m == password)))
                    .Return("hash");

                securityDataServices
                    .WhenToldTo(p => p.GetRoleByName(Param<string>.Matches(m => m == "User")))
                    .Return(new RoleDataModel(roleId, "User"));

                userDataManager
                    .WhenToldTo(p => p.GetUserFromUsername(Param<string>.Matches(m => m == userName)))
                    .Return(new UserDataModel(userId, userName, "hash", Guid.NewGuid(), true, false));
            };

        Because of = () =>
            {
                messages = commandHandler.HandleCommand(new AutoLoginCommand(userName, branchCode, password),metadata);
            };

        It should_get_branch_by_name = () =>
            {
                securityDataServices.WasToldTo(x => x.GetBranchByBranchCode(Param<string>.Matches(p => p == branchCode)));
                messages.AllMessages.ShouldBeEmpty();
            };

        It should_check_if_username_exists = () =>
            {
                userDataManager.WasToldTo(x => x.DoesUsernameExist(Param<string>.Matches(p => p == userName)));
            };

        It should_hash_the_password = () =>
            {
                passwordManager.WasToldTo(x => x.HashPassword(Param<string>.Matches(p => p == password)));
            };

        It should_get_role_by_name = () =>
            {
                securityDataServices.WasToldTo(x => x.GetRoleByName(Param<string>.Matches(p => p == "User")));
            };

        It should_get_user_from_username = () =>
            {
                userDataManager.WasToldTo(x => x.GetUserFromUsername(Param<string>.Matches(p => p == userName)));
            };

        It should_set_password_and_activate = () =>
            {
                userDataManager.WasToldTo(x => x.SetUserPasswordAndActivate(Param<Guid>.Matches(p => p == userId),
                    Param<string>.Matches(p => p == "hash")));
            };

        It should_add_the_user = () =>
            {
                securityManager.WasToldTo(
                    x => x.AddUser(
                        Param<string>.Matches(m => m == userName),
                        Param<string>.Matches(m => m == userName + "@server.com"),
                        Param<string>.Matches(m => m == userName),
                        Param<string>.Matches(m => m == userName),
                        Param<Guid[]>.Matches(m => m.Length == 1 && m[0] == roleId),
                        Param<Guid>.Matches(m => m == branchId)
                    )
                );
            };

        It should_log_the_user_in = () =>
            {
                authenticationManager.WasToldTo(
                    x => x.Login(
                        Param<string>.Matches(m => m == userName),
                        Param<string>.Matches(m => m == password),
                        Param<string>.Matches(m => m == "")
                    )
                );
            };
    }
}
