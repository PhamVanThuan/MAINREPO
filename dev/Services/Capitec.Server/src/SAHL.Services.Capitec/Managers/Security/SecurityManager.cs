using Capitec.Core.Identity;
using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Managers.Security
{
    public class SecurityManager : ISecurityManager
    {
        private IUserManager userManager;
        private ISecurityDataManager securityDataServices;
        private IGeoDataManager geoDataServices;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public SecurityManager(IUserManager userManager, ISecurityDataManager securityDataServices, IGeoDataManager geoDataServices, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.userManager = userManager;
            this.securityDataServices = securityDataServices;
            this.geoDataServices = geoDataServices;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #region branches

        public void AddBranch(string branchName, bool isActive, Guid suburbId, string branchCode)
        {
            if (string.IsNullOrEmpty(branchName))
                throw new ArgumentException("Branch name cannot be empty");

            if (!geoDataServices.DoesSuburbIdExist(suburbId))
                throw new ArgumentException("Suburb id is invalid");

            securityDataServices.AddBranch(branchName, isActive, suburbId, branchCode);
        }

        public void ChangeBrancheDetails(Guid id, string branchName, bool isActive, Guid suburbId)
        {
            if (string.IsNullOrEmpty(branchName))
                throw new ArgumentException("Branch name cannot be empty");

            if (!geoDataServices.DoesSuburbIdExist(suburbId))
                throw new ArgumentException("Suburb id is invalid");

            if (!securityDataServices.DoesBranchIdExist(id))
                throw new ArgumentException("Branch id is invalid");

            securityDataServices.ChangeBranchDetails(id, branchName, isActive, suburbId);
        }

        #endregion branches

        #region users

        public void AddUser(string username, string emailAddress, string firstName, string lastName, Guid[] rolesToAssign, Guid branchId)
        {
            if (string.IsNullOrEmpty(emailAddress))
                throw new ArgumentException("Email Address cannot be empty");

            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("First Name cannot be empty");

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("Surname cannot be empty");

            if (!securityDataServices.DoesBranchIdExist(branchId))
                throw new ArgumentException("Branch id is invalid");
            
            using (var uow = this.unitOfWorkFactory.Build())
            {
                Guid newUserId = userManager.InviteUser(username, emailAddress, firstName, lastName, rolesToAssign);
                securityDataServices.LinkUserToBranch(newUserId, branchId);
                uow.Complete();
            }
        }

        public void ChangeUserDetails(Guid id, string emailAddress, string firstName, string lastName, bool status, Guid[] rolesToAssign, Guid[] rolesToRemove, Guid branchId)
        {
            if (string.IsNullOrEmpty(emailAddress))
                throw new ArgumentException("Email Address cannot be empty");

            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("First Name cannot be empty");

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("Surname cannot be empty");

            if (!securityDataServices.DoesBranchIdExist(branchId))
                throw new ArgumentException("Branch id is invalid");
            using (var uow = this.unitOfWorkFactory.Build())
            {
                userManager.UpdateUser(id, status);
                userManager.UpdateUserInformation(id, emailAddress, firstName, lastName);
                userManager.AssignUserRole(id,rolesToAssign);
                userManager.RemoveUserRole(id,rolesToRemove);
                if (securityDataServices.HasUsersBranchChanged(id, branchId))
                {
                    if (securityDataServices.DoesUserBelongToAnyBranches(id))
                    {
                        securityDataServices.UpdateUserToBranchLink(id, branchId);
                    }
                    else
                    {
                        securityDataServices.LinkUserToBranch(id, branchId);
                    }
                }
                uow.Complete();
            }
        }

        #endregion users
    }
}