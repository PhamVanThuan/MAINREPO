using Automation.DataAccess;
using Automation.DataModels;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IADUserService
    {
        void UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum wrt, GeneralStatusEnum gs, bool UOS,
            bool ADUser, bool UOSRR);

        QueryResults GetBranchUsersForApplication(int offerKey, OfferRoleTypeEnum roleType);

        bool UpdateADUserStatus(string adUserName, GeneralStatusEnum genStatus, GeneralStatusEnum rrStatus, GeneralStatusEnum uosStatus);

        bool UpdateADUserStatusSQL(string adUserStatus, string adUsername);

        bool UpdateADUserRoundRobinStatusSQL(string adUserStatus, string adUsername);

        void DeactivateDebtCounsellingBusinessUsers();

        string GetADUserStatus(string adUserName);

        string GetRoundRobinStatus(string adUserName, int osKey);

        bool IsADUserActive(string adUserName);

        bool IsUserInSameBranchAsApp(string adUserName, string offerKey);

        QueryResults GetADUserKeyByADUserName(string adUserName, int orgStructureKey);

        QueryResults GetLegalEntityNameFromADUserName(string adUserName, int isInitials, GeneralStatusEnum gsKey);

        QueryResults SetUserInactiveAndGetTestDataSQL(string userLoginName);

        QueryResults CheckAtLeastOneUserStillActiveForOrgStructKeySQL(int orgStructKey);

        void SetAllUsersForOrgStructKeyActive(int orgStructKey);

        QueryResults TestAduserSQL(string userLoginName);

        QueryResults RoleSQLCount(string userLoginName);

        QueryResults RoleSQL(string userLoginName);

        QueryResults GetAdUserName(int udUserKey);

        QueryResults GetAdUserNameByLegalEntityKey(int legalEntityKey);

        string GetADUserPlayingWorkflowRole(string role, string currentOwner);

        string GetADUserPlayingOfferRole(OfferRoleTypeEnum role, string userToExclude);

        IEnumerable<ADUser> GetADUserKeyByADUserName(params string[] adusername);
    }
}