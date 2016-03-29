using System;
using SAHL.Core.Data.Models.Capitec;

namespace SAHL.Services.Capitec.Managers.Security
{
    public interface ISecurityDataManager
    {
        bool DoesBranchIdExist(Guid id);

        void AddBranch(string branchName, bool isActive, Guid suburbId, string branchCode);

        void ChangeBranchDetails(Guid id, string branchName, bool isActive, Guid suburbId);

        void LinkUserToBranch(Guid userId, Guid branchId);

        bool HasUsersBranchChanged(Guid userId, Guid branchId);

        void UpdateUserToBranchLink(Guid userId, Guid branchId);

        bool DoesUserBelongToAnyBranches(Guid id);

        bool DoesUserBelongToBranch(Guid userId, Guid branchId);

        RoleDataModel GetRoleByName(string roleName);

        BranchDataModel GetBranchByBranchCode(string branchCode);

        BranchDataModel GetBranchForUser(Guid userId);
    }
}