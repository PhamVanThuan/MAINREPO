using System;

namespace SAHL.Services.Capitec.Managers.Security
{
    public interface ISecurityManager
    {
        void AddBranch(string branchName, bool isActive, Guid suburbId, string branchCode);

        void ChangeBrancheDetails(Guid Id, string branchName, bool isActive, Guid suburbId);

        void AddUser(string username, string emailAddress, string firstName, string lastName, Guid[] rolesToAssign, Guid branchId);

        void ChangeUserDetails(Guid id, string emailAddress, string firstName, string lastName, bool status, Guid[] rolesToAssign, Guid[] rolesToRemove, Guid branchId);
    }
}