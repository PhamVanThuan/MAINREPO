using System;
using Capitec.Core.Identity;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class AutoLoginCommandHandler : IServiceCommandHandler<AutoLoginCommand>
    {
        private IAuthenticationManager authenticationManager;
        private ISecurityDataManager securityDataServices;
        private IUserDataManager userDataManager;
        private ISecurityManager securityManager;
        private IPasswordManager passwordManager;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public AutoLoginCommandHandler(IAuthenticationManager authenticationManager,
            ISecurityDataManager securityDataServices,
            IUserDataManager userDataManager,
            ISecurityManager securityManager,
            IPasswordManager passwordManager, 
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.authenticationManager = authenticationManager;
            this.securityDataServices = securityDataServices;
            this.userDataManager = userDataManager;
            this.securityManager = securityManager;
            this.passwordManager = passwordManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ISystemMessageCollection HandleCommand(AutoLoginCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            /**Before we go any further, the branch must exist **/
            var branch = securityDataServices.GetBranchByBranchCode(command.BranchCode);
            if (branch == null)
            {
                messages.AddMessage(new SystemMessage("Branch does not exist.", SystemMessageSeverityEnum.Error));
                return messages;
            }
            if (!branch.IsActive)
            {
                messages.AddMessage(new SystemMessage("Branch is not active.", SystemMessageSeverityEnum.Error));
                return messages;
            }

            ProcessAutoLogin(command, branch, messages);
            return messages;
        }

        private void ProcessAutoLogin(AutoLoginCommand command, BranchDataModel branch, ISystemMessageCollection messages)
        {
            /** Does user exist? **/
            if (!this.userDataManager.DoesUsernameExist(command.CP))
            {
                CreateUserAccount(command, branch.Id);
            }
            else
            {
                var user = userDataManager.GetUserFromUsername(command.CP);
                /** Check if user belongs to branch specified **/
                if (!securityDataServices.DoesUserBelongToBranch(user.Id, branch.Id))
                {
                    /** Check if new branch exists in DB **/
                    var newUserBranch = securityDataServices.GetBranchByBranchCode(command.BranchCode);
                    if (newUserBranch == null)
                    {
                        messages.AddMessage(new SystemMessage("Branch does not exist.", SystemMessageSeverityEnum.Error));
                    }
                    else if (!newUserBranch.IsActive)
                    {
                        messages.AddMessage(new SystemMessage("Branch is not active.", SystemMessageSeverityEnum.Error));
                    }
                    else
                    {
                        /** Update users branch if he has not been logged in before with this branch **/
                        this.securityDataServices.UpdateUserToBranchLink(user.Id, newUserBranch.Id);
                    }
                }
            }
            /** login attempt **/
            this.authenticationManager.Login(command.CP, command.Password, String.Empty);
        }

        private void CreateUserAccount(AutoLoginCommand command, Guid branchId)
        {
            string hashedPassword = this.passwordManager.HashPassword(command.Password);
            using (var uow = this.unitOfWorkFactory.Build())
            {
                var role = GetRoleForUser();
                this.securityManager.AddUser(command.CP,
                    string.Format("{0}@server.com", command.CP),
                    command.CP,
                    command.CP,
                    new Guid[] { role },
                    branchId);

                var user = userDataManager.GetUserFromUsername(command.CP);
                this.userDataManager.SetUserPasswordAndActivate(user.Id, hashedPassword);
                uow.Complete();
            }
        }

        private Guid GetRoleForUser()
        {
            RoleDataModel roleModel = securityDataServices.GetRoleByName("User");
            return roleModel.Id;
        }
    }
}
