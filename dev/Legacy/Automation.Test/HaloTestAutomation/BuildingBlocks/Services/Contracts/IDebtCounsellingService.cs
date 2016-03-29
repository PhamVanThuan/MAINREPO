using Automation.DataAccess;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IDebtCounsellingService
    {
        Automation.DataModels.SnapshotAccount GetSnapShotAccountByDebtCounsellingKey(int debtCounsellingkey);

        Automation.DataModels.SnapshotAccount GetSnapShotAccountByAccountKey(int accountKey);

        int GetRandomDebtCounsellingAccount(DebtCounsellingStatusEnum status);

        QueryResults RemoveDebtCounsellingCase(int debtCounsellingKey, int debtCounsellingGroupKey);

        int GetAccountKeyByDebtCounsellingKey(int debtCounsellingKey, DebtCounsellingStatusEnum status);

        Automation.DataModels.DebtCounselling GetDebtCounsellingAccount(int debtCounsellingKey);

        List<int> AddAccountUnderDebtCounselling(int accountKey);

        int GetDebtCounsellingKey(int accountKey);

        List<int> GetRelatedDebtCounsellingAccounts(int debtCounsellingKey);

        int GetDebtCounsellingCaseWithMoreThanOneLegalEntity(string[] users, params string[] workflowStates);

        IEnumerable<Automation.DataModels.DebtCounselling> GetDebtCounsellingCases(int debtcounsellingkey = 0, int accountkey = 0,
            string[] debtcounsellingWorkflowStates = null, ExternalRoleTypeEnum extRoleType = ExternalRoleTypeEnum.None,
            DebtCounsellingStatusEnum debtcounsellingstatus = DebtCounsellingStatusEnum.None, bool isArchivedCases = false, bool isAcceptedProposal = false, bool includeDebtReviewApproved = false);

        Automation.DataModels.Account GetDebtCounsellingEworkCaseWithNoDebtCounsellingCase(string LossControlSubMap = "None");

        bool DebtCounsellingCaseExists(int accountKey);

        int GetDebtCounsellingRemainingTerm(int debtCounsellingKey, DateTime proposalEndDate);

        bool DebtCounsellingCaseExistsByStatus(int accountKey, DebtCounsellingStatusEnum debtCounsellingStatus);

        void UpdateDebtCounsellingLegalEntity(string testIdentifier, bool underDebtCounselling);

        void UpdateCurrentCaseOwner(string adUserName, string testIdentifier);

        int GetDebtCounsellorLegalEntityKey(string ncrdcNumber);

        string GetNCRNumber();

        QueryResults GetLegalEntityForDebtCounsellingCreate(bool bHasEWorkLossControlCase, int countOfCases, bool hasArrearBalance, ProductEnum product,
           bool isInterestOnly);

        string GetLegalEntityIDNumberForDCCreate(bool hasEWorkLossControlCase, int countOfCases, bool hasArrearBalance, ProductEnum product = ProductEnum.NewVariableLoan, bool isInterestOnly = false);

        List<string> GetAccountWithoutDebtCounsellingCaseByMainApplicantCount(int roleCount, int recordCount, out int key);

        List<int> GetAutomationDebtCounsellingTestCase(ProductEnum product, bool isInterestOnly);

        void DebtCounsellingArrears();

        QueryResults GetAccountSnapShot(int debtCounsellingKey);

        QueryResults GetDebtCounsellingRowByDebtCounsellingKey(int debtCounsellingKey, DebtCounsellingStatusEnum status);

        IEnumerable<Automation.DataModels.DebtCounselling> GetDebtCounsellingCaseByStateAndWorkflowRoleType(string workflowState, string aduserName);

        int[,] CreateDCKeyLEKeyArray(string testIdentifier, int underdebtcounselling);

        int MinDebtCounselling(string testIdentifier);

        void SaveDebtCounsellingkeys(string testIdentifier);

        QueryResults GetAccountsForDCTestIdentifier(string testIdentifier);

        QueryResults GetLegalEntitiesForDebtCounsellingCaseCreate(string testIdentifier);

        string GetIDNumber(string testIdentifier);

        string GetPassportNumber(string testIdentifier);

        string GetDebtCounsellorName(string testIdentifier);

        string GetCaseOwnerName(string testIdentifier);

        List<int> GetAccountListByDCTestIdentifier(string testIdentifier);

        List<int> GetDCKeyListByDCTestIdentifier(string testIdentifier);

        string GetFirstAccountKey(string testIdentifier);

        int GetDebtCounsellingKey(string testIdentifier, string accountKey);

        bool GetDebtCounsellingCaseByState(string stateName, out int debtCounsellingKey, out int accountKey, out string aduserName,
            ProductEnum product = ProductEnum.NewVariableLoan);

        int GetDCTestCaseDebtCounsellorCorrespondenceDetails(string testIdentifier, int accountKey);

        void MakeDebtCounsellingBusinessUsersInactive();

        QueryResults GetDCAccountsByIDNumber(string idNumber);

        List<int> GetDebtCounsellingAccountListByLegalEntityIDNumber(string idNumber);

        int GetPersonalLoanDebtCounsellingAccountByLegalEntityIDNumber(string idNumber);

        string GetNCRNumber(string testIdentifier = "", int legalEntityKey = 0);

        List<string> DebtCounsellingTestIdentifiers(string testGroup);

        QueryResults GetDebtCounsellingByAccountKeyAndStatus(int accountKey, DebtCounsellingStatusEnum status = DebtCounsellingStatusEnum.Open);

        QueryResults GetReferenceByDebtCounsellingKey(int debtCounsellingKey);

        QueryResults GetDebtCounsellingAccountSnapShot(int debtCounsellingKey);

        DateTime GetDebtCounsellingDiaryDate(int debtcounsellingkey);

        List<int> AssignDebtCounsellingCasesToAttorney(int legalEntityKey);

        bool CreateDebtCounsellingCase(int accountKey);

        QueryResults GetDebtCounsellingCreateCases();

        QueryResults GetDebtCounsellingSearchCases();

        int GetPersonalLoanAccountUnderDebtCounselling();

        void SetupClientsWithSharedDomiciliumOnDebtCounsellingCase(int debtCounsellingKey);

        void SetupClientsWithDifferentDomiciliumsOnDebtCounsellingCase(int debtCounsellingKey);

        void ChangeDebtCounsellor(int debtCounsellorKey, int accountKey);
    }
}