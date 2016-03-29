using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowAutomation.Harness;

namespace BuildingBlocks.Services
{
    public class DebtCounsellingService : _2AMDataHelper, IDebtCounsellingService
    {
        private readonly IAccountService accountService;
        private readonly ILoanTransactionService transactionService;
        private readonly IExternalRoleService externalRoleService;
        private readonly IX2WorkflowService x2Service;
        private readonly ILegalEntityAddressService legalEntityAddressService;

        public DebtCounsellingService(IAccountService accountService, ILoanTransactionService transactionService, IExternalRoleService externalRoleService, IX2WorkflowService x2Service, ILegalEntityAddressService legalEntityAddressService)
        {
            this.accountService = accountService;
            this.transactionService = transactionService;
            this.externalRoleService = externalRoleService;
            this.legalEntityAddressService = legalEntityAddressService;
            this.x2Service = x2Service;
        }

        /// <summary>
        /// Gets the accountKey for a debt CounsellingKey
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public int GetAccountKeyByDebtCounsellingKey(int debtCounsellingKey, DebtCounsellingStatusEnum status)
        {
            var results = base.GetDebtCounsellingRowByDebtCounsellingKey(debtCounsellingKey, status);
            return results.Rows(0).Column("AccountKey").GetValueAs<int>();
        }

        /// <summary>
        ///  inserts a data into the debtcounselling and external role tables in order for a fake debt counselling case to be created.
        /// </summary>
        /// <param name="accountKey">account to use</param>
        /// <returns>DebtCounsellingKey, DebtCounsellingGroupKey</returns>
        public List<int> AddAccountUnderDebtCounselling(int accountKey)
        {
            List<int> insertedKeys = new List<int>();
            var debtCounsellingKeys = base.AddAccountUnderDebtCounselling(accountKey);
            insertedKeys.Add(debtCounsellingKeys.Rows(0).Column(0).GetValueAs<int>());
            insertedKeys.Add(debtCounsellingKeys.Rows(0).Column(1).GetValueAs<int>());
            return insertedKeys;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public int GetDebtCounsellingKey(int accountKey)
        {
            var results = base.GetDebtCounsellingByAccountKeyAndStatus(accountKey);
            return results.Rows(0).Column("DebtCounsellingKey").GetValueAs<int>();
        }

        /// <summary>
        /// Fetches a list of accounts within the debt counselling process that are linked to the specified debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey">DebtCounsellingKey</param>
        /// <returns></returns>
        public List<int> GetRelatedDebtCounsellingAccounts(int debtCounsellingKey)
        {
            List<int> accounts = new List<int>();
            var results = base.GetGroupedAccounts(debtCounsellingKey);
            foreach (var item in results)
            {
                accounts.Add(item.Column("accountKey").GetValueAs<int>());
            }
            return accounts;
        }

        /// <summary>
        /// Get a single debtcounselling record from the database based on the parameters
        /// </summary>
        public IEnumerable<Automation.DataModels.DebtCounselling> GetDebtCounsellingCases
            (
                int debtcounsellingkey = 0, int accountKey = 0, string[] debtcounsellingWorkflowStates = null, ExternalRoleTypeEnum extRoleType = ExternalRoleTypeEnum.None, DebtCounsellingStatusEnum debtcounsellingstatus = DebtCounsellingStatusEnum.None,
                bool isArchivedCases = false, bool isAcceptedProposal = false, bool includeDebtReviewApproved = false
            )
        {
            var debtcounsellingcases = base.GetDebtCounsellingRecords(isArchivedCases, includeDebtReviewApproved, accountKey, debtcounsellingkey);
            foreach (var item in debtcounsellingcases)
            {
                if (accountKey > 0)
                {
                    item.AccountKey = accountKey;
                }
                else if (item.Account != null)
                {
                    item.AccountKey = item.Account.AccountKey;
                }
            }

            if (extRoleType != ExternalRoleTypeEnum.None)
                debtcounsellingcases = from debtcounsellingcase in debtcounsellingcases
                                       where debtcounsellingcase.ExternalRoleTypeKey == extRoleType
                                       select debtcounsellingcase;

            if (debtcounsellingstatus != DebtCounsellingStatusEnum.None)
                debtcounsellingcases = from debtcounsellingcase in debtcounsellingcases
                                       where debtcounsellingcase.DebtCounsellingStatusKey == (int)debtcounsellingstatus
                                       select debtcounsellingcase;

            if (debtcounsellingWorkflowStates != null)
            {
                var debtcounsellingCaseList = new List<Automation.DataModels.DebtCounselling>();
                foreach (string state in debtcounsellingWorkflowStates)
                {
                    var list = (from debtCaseRec in debtcounsellingcases
                                where debtCaseRec.StageName == state
                                select debtCaseRec).ToArray();
                    debtcounsellingCaseList.AddRange(list);
                }
                debtcounsellingcases = debtcounsellingCaseList;
            }
            if (isAcceptedProposal)
            {
                debtcounsellingcases = from debtcounsellingcase in debtcounsellingcases
                                       where
                                       debtcounsellingcase.Proposal != null
                                       && debtcounsellingcase.Proposal.ProposalTypeKey == ProposalTypeEnum.Proposal
                                       && debtcounsellingcase.Proposal.ProposalStatusKey == ProposalStatusEnum.Active
                                       && debtcounsellingcase.Proposal.Accepted
                                       && debtcounsellingcase.SnapshotAccount != null
                                       select debtcounsellingcase;
            }
            return debtcounsellingcases;
        }

        public int GetDebtCounsellingRemainingTerm(int debtCounsellingKey, DateTime proposalEndDate)
        {
            var r = base.GetDebtCounsellingRemainingTerm(debtCounsellingKey, proposalEndDate);
            return r.Rows(0).Column("RemainingTerm").GetValueAs<int>();
        }

        /// <summary>
        /// Checks whether a debt counselling case exists or has previously existed for the specified account.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        public bool DebtCounsellingCaseExists(int accountKey)
        {
            var r = base.GetDebtCounsellingByAccountKeyAndStatus(accountKey, DebtCounsellingStatusEnum.None);
            return r.HasResults;
        }

        /// <summary>
        /// Checks whether an open debt counselling case exists.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        public bool OpenDebtCounsellingCaseExists(int accountKey)
        {
            var r = base.GetDebtCounsellingByAccountKeyAndStatus(accountKey, DebtCounsellingStatusEnum.Open);
            return r.HasResults;
        }

        /// <summary>
        /// Fetches an account for which an e-work case exists at a debt counselling stage with no corresponding open debt counselling case.
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Account GetDebtCounsellingEworkCaseWithNoDebtCounsellingCase(string LossControlSubMap = "None")
        {
            int accountKey = 0;
            bool exists = false;
            var r = base.GetDebtCounsellingEworkCaseWithNoDebtCounsellingCase();
            if (r.HasResults)
            {
                foreach (var row in r.RowList)
                {
                    accountKey = row.Column("accountKey").GetValueAs<int>();
                    exists = DebtCounsellingCaseExists(accountKey);
                    if (!exists)
                        return accountService.GetAccountByKey(accountKey);
                }
            }
            return default(Automation.DataModels.Account);
        }

        /// <summary>
        /// Tells you whether a debt counselling cases exists against an account with specified status.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <param name="debtCounsellingStatus">debtCounsellingStatus</param>
        /// <returns>TRUE=Case Exists, FALSE=No Case Exists</returns>
        public bool DebtCounsellingCaseExistsByStatus(int accountKey, DebtCounsellingStatusEnum debtCounsellingStatus)
        {
            var r = base.GetDebtCounsellingByAccountKeyAndStatus(accountKey, debtCounsellingStatus);
            return r.HasResults;
        }

        /// <summary>
        /// Gets an ID Number to use for debt counselling case creation
        /// </summary>
        /// <param name="hasEWorkLossControlCase">0 = LE without a loss control case, 1 = LE with a loss control case</param>
        /// <param name="countOfCases">Number of accounts linked to a LE</param>
        /// <param name="hasArrearBalance">true = account has an arrear balance</param>
        /// <param name="product">product type</param>
        /// <param name="isInterestOnly"></param>
        /// <returns></returns>
        public string GetLegalEntityIDNumberForDCCreate(bool hasEWorkLossControlCase, int countOfCases, bool hasArrearBalance, ProductEnum product = ProductEnum.NewVariableLoan, bool isInterestOnly = false)
        {
            var r = base.GetLegalEntityForDebtCounsellingCreate(hasEWorkLossControlCase, countOfCases, hasArrearBalance, product, isInterestOnly);
            return r.Rows(0).Column("idnumber").Value;
        }

        /// <summary>
        /// Returns an account with the number of roles provided that does not have an open debt counselling case
        /// </summary>
        /// <param name="roleCount">No. of Roles</param>
        /// <param name="recordCount">No. of Records to return</param>
        /// <param name="key">accountkey</param>
        public List<string> GetAccountWithoutDebtCounsellingCaseByMainApplicantCount(int roleCount, int recordCount, out int key)
        {
            var results = accountService.GetRandomVariableLoanAccountByMainApplicantCount(2, 25000, AccountStatusEnum.Open);
            //get first account that doesnt have a debt counselling case.
            var accountKey = (from r in results
                              where DebtCounsellingCaseExistsByStatus(r.Column("AccountKey").GetValueAs<int>(), DebtCounsellingStatusEnum.Open) == false
                              select r.Column("AccountKey").GetValueAs<int>()).FirstOrDefault();
            //find all their id numbers
            var idNumberList = (from r in results
                                where r.Column("AccountKey").GetValueAs<int>() == accountKey
                                select r.Column("idnumber").GetValueAs<string>()).ToList();
            key = accountKey;
            return idNumberList;
        }

        /// <summary>
        /// Gets Automation Debt Counselling Test Cases that has not been used to create a debt counselling case before.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="isInterestOnly"></param>
        /// <returns></returns>
        public List<int> GetAutomationDebtCounsellingTestCase(ProductEnum product, bool isInterestOnly)
        {
            var results = base.GetAccountsForDebtCounsellingByProduct(product, isInterestOnly);
            if (results.HasResults)
            {
                return results.GetColumnValueList<int>("accountKey");
            }
            else
            {
                return default(List<int>);
            }
        }

        /// <summary>
        /// Get the snapshot that was taken from the account, financialservices and rateoverrides
        /// before the proposal was accepted.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public QueryResults GetAccountSnapShot(int debtCounsellingKey)
        {
            int accountkey = GetAccountKeyByDebtCounsellingKey(debtCounsellingKey, DebtCounsellingStatusEnum.Open);
            return accountService.GetAccountFinancialServiceFinancialAdjustments(accountkey);
        }

        /// <summary>
        /// Creates a 2D array of DCKeys and LEKeys from the configuration that allows us to determine
        /// which LE's we expect to be under debt counselling by virtue of their Client Roles being added
        /// to our debt counselling case
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <param name="underdebtcounselling">1=roles exist, 0=roles dont exist</param>
        /// <returns></returns>
        public int[,] CreateDCKeyLEKeyArray(string testIdentifier, int underdebtcounselling)
        {
            QueryResults r = base.GetLegalEntitiesForCaseCreationAssertion(testIdentifier, underdebtcounselling);
            int count = r.RowList.Count;
            int[,] numbers = new int[count, 2];
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j != 2; j++)
                {
                    int val = r.Rows(i).Column(j).GetValueAs<int>();
                    numbers[i, j] = val;
                }
            }
            return numbers;
        }

        /// <summary>
        /// Returns the ID Number for a test case when provided with the test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>DebtCounsellingTestCases.IDNumber</returns>
        public string GetIDNumber(string testIdentifier)
        {
            return (from d in base.GetDebtCounsellingTestCases(testIdentifier)
                    select d.Column("IDNumber").GetValueAs<string>()).FirstOrDefault();
        }

        /// <summary>
        /// Returns the Passport Number for a test case when provided with the test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>DebtCounsellingTestCases.PassportNumber</returns>
        public string GetPassportNumber(string testIdentifier)
        {
            return (from d in base.GetDebtCounsellingTestCases(testIdentifier)
                    select d.Column("PassportNumber").GetValueAs<string>()).FirstOrDefault();
        }

        /// <summary>
        /// Returns the DebtCounsellorName for a test case when provided with the test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>DebtCounsellingTestCases.DebtCounsellorName</returns>
        public string GetDebtCounsellorName(string testIdentifier)
        {
            return (from d in base.GetDebtCounsellingTestCases(testIdentifier)
                    select d.Column("DebtCounsellorName").GetValueAs<string>()).FirstOrDefault();
        }

        /// <summary>
        /// Returns the CreatorADUserName for a test case when provided with the test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>DebtCounsellingTestCases.CreatorADUserName</returns>
        public string GetCaseOwnerName(string testIdentifier)
        {
            QueryResults r = base.GetDebtCounsellingTestCases(testIdentifier);
            return r.Rows(0).Column("CurrentCaseOwner").Value;
        }

        /// <summary>
        /// Generates an Account List from the db for a given test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>List of int's</returns>
        public List<int> GetAccountListByDCTestIdentifier(string testIdentifier)
        {
            return (from results in GetAccountsForDCTestIdentifier(testIdentifier)
                    select results.Column("AccountKey").GetValueAs<int>()).ToList();
        }

        /// <summary>
        /// Gets a list of DebtCounsellingKeys for a give test identifier from the configuration tables
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>List of int's</returns>
        public List<int> GetDCKeyListByDCTestIdentifier(string testIdentifier)
        {
            return (from results in GetAccountsForDCTestIdentifier(testIdentifier)
                    select results.Column("DebtCounsellingKey").GetValueAs<int>()).ToList();
        }

        /// <summary>
        /// returns the first account linked to a test identifier. this can only be used for tests that have a single mortgage loan account.
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <returns></returns>
        public string GetFirstAccountKey(string testIdentifier)
        {
            QueryResults r = GetAccountsForDCTestIdentifier(testIdentifier);
            return r.Rows(0).Column("AccountKey").Value;
        }

        /// <summary>
        /// Gets the debt counselling key for case
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <param name="accountKey">account number</param>
        /// <returns></returns>
        public int GetDebtCounsellingKey(string testIdentifier, string accountKey)
        {
            var results = GetAccountsForDCTestIdentifier(testIdentifier);
            var key = (from r in results
                       where r.Column("AccountKey").Value == accountKey
                       select r.Column("DebtCounsellingKey").GetValueAs<int>()).FirstOrDefault();
            return key;
        }

        /// <summary>
        /// Gets the correspondence details captured against a debt counselling test case when
        /// provided with the test identifier and the accountKey
        /// </summary>
        /// <param name="testIdentifier">TestIdentifier</param>
        /// <param name="accountKey">AccountKey</param>
        public int GetDCTestCaseDebtCounsellorCorrespondenceDetails(string testIdentifier, int accountKey)
        {
            int debtCounsellingKey = 0;
            int legalEntityKey = 0;
            if (string.IsNullOrEmpty(testIdentifier))
            {
                debtCounsellingKey = GetDebtCounsellingKey(accountKey);
            }
            else
            {
                debtCounsellingKey = GetDebtCounsellingKey(testIdentifier, accountKey.ToString());
            }
            var results = externalRoleService.GetExternalRoleCorrespondenceDetails(debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            if (results.HasResults)
            {
                legalEntityKey = results.Rows(0).Column(0).GetValueAs<int>();
            }
            else
            {
                var externalRole = externalRoleService.GetFirstActiveExternalRole(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
                legalEntityKey = externalRole.LegalEntityKey;
            }
            return legalEntityKey;
        }

        /// <summary>
        /// Gets a debt counselling case at a given state
        /// </summary>
        /// <param name="stateName">State</param>
        /// <param name="debtCounsellingKey">out DebtCounsellingKey</param>
        /// <param name="accountKey">out AccountKey</param>
        /// <param name="aduserName">out User Assigned the case</param>
        /// <param name="product">Product</param>
        public bool GetDebtCounsellingCaseByState(string stateName, out int debtCounsellingKey, out int accountKey, out string aduserName,
            ProductEnum product = ProductEnum.NewVariableLoan)
        {
            debtCounsellingKey = -1;
            accountKey = -1;
            aduserName = string.Empty;
            //get a list of cases
            var results = x2Service.GetX2DataByWorkflowAndState(Workflows.DebtCounselling, stateName);
            if (!results.HasResults)
                return false;
            foreach (var row in results.RowList)
            {
                debtCounsellingKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
                var dc = base.GetDebtCounsellingRowByDebtCounsellingKey(debtCounsellingKey, DebtCounsellingStatusEnum.Open);
                accountKey = Convert.ToInt32(dc.Rows(0).Column("AccountKey").Value);
                var acc = base.GetAccountByKeySQL(accountKey);
                if ((int)acc.ProductKey == (int)product)
                {
                    int instanceID = results.Rows(0).Column(1).GetValueAs<int>();
                    aduserName = base.GetWorkflowRoleAssignmentByInstance(instanceID).Rows(0).Column("adusername").Value;
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets a list of the debt counselling test identifiers when provided with the Test Group
        /// </summary>
        /// <param name="testGroup"></param>
        /// <returns></returns>
        public List<string> DebtCounsellingTestIdentifiers(string testGroup)
        {
            return (from r in base.GetDebtCounsellingTestIdentifiers(testGroup)
                    select r.Column("TestIdentifier").GetValueAs<string>()).ToList();
        }

        /// <summary>
        /// Returns the NCR Number for a test case when provided with the test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <param name="legalEntityKey"></param>
        /// <returns>DebtCounsellingTestCases.NCRNumber</returns>
        public string GetNCRNumber(string testIdentifier = "", int legalEntityKey = 0)
        {
            QueryResults results;
            if (!string.IsNullOrEmpty(testIdentifier))
            {
                results = base.GetDebtCounsellingTestCases(testIdentifier);
                return results.Rows(0).Column("NCRDCRegNumber").GetValueAs<string>();
            }
            else
            {
                results = base.GetDebtCounsellorDetails(legalEntityKey);
                return results.Rows(0).Column("NCRDCRegistrationNumber").GetValueAs<string>();
            }
        }

        /// <summary>
        /// Builds a list of accountKeys that a legal entity is currently under debt counselling for when provided with an idnumber
        /// </summary>
        /// <param name="idNumber">LE ID Number</param>
        /// <returns></returns>
        public List<int> GetDebtCounsellingAccountListByLegalEntityIDNumber(string idNumber)
        {
            var results = base.GetDCAccountsByIDNumber(idNumber);
            List<int> accountList = (from r in results select r.Column("AccountKey").GetValueAs<int>()).ToList();
            return accountList;
        }

        /// <summary>
        /// Build up a single account, based upon the idnumber parameter, for which a legal entity is currently under personal loan account debt counselling.
        /// </summary>
        /// <param name="idNumber">LE ID Number</param>
        /// <returns></returns>
        public int GetPersonalLoanDebtCounsellingAccountByLegalEntityIDNumber(string idNumber)
        {
            var results = base.GetDCAccountsByIDNumber(idNumber);
            int accountKey = (
                from r in results
                where r.Column("ProductKey").GetValueAs<int>() == (int)ProductEnum.PersonalLoan
                select r.Column("AccountKey").GetValueAs<int>()).FirstOrDefault();
            return accountKey;
        }

        public bool CreateDebtCounsellingCase(int accountKey)
        {
            //now we need to create a debt counselling case
            var testCase = new Automation.DataModels.TestCase
            {
                AccountKey = accountKey,
                DataTable = "x2.x2data.debt_counselling",
                ExpectedEndState = WorkflowStates.DebtCounsellingWF.ReviewNotification,
                KeyType = "DebtCounsellingKey",
                ScriptFile = "DebtCounselling.xaml",
                ScriptToRun = "CaseCreate",
                TestCaseResults = new Dictionary<int, WorkflowReturnData>()
            };
            //we need to do the case creation
            IDataCreationHarness dataCreation = new DataCreationHarness();
            return dataCreation.CreateSingleTestCase(Common.Enums.WorkflowEnum.DebtCounselling, testCase);
        }

        public void SetupClientsWithSharedDomiciliumOnDebtCounsellingCase(int debtCounsellingKey)
        {
            var clientList = externalRoleService.GetActiveExternalRoleList(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            var legalEntityAddress = default(Automation.DataModels.LegalEntityAddress);
            foreach (var client in clientList)
            {
                legalEntityAddressService.CleanupLegalEntityAddresses(client.LegalEntityKey, false, GeneralStatusEnum.Inactive);
                legalEntityAddressService.DeleteLegalEntityDomiciliumAddress(client.LegalEntityKey);
                if (legalEntityAddress == null)
                {
                    legalEntityAddress = legalEntityAddressService.InsertLegalEntityAddressByAddressType(client.LegalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
                }
                else
                {
                    legalEntityAddress.LegalEntityKey = client.LegalEntityKey;
                    legalEntityAddress.LegalEntityAddressKey = 0;
                    legalEntityAddress.LegalEntity.LegalEntityKey = client.LegalEntityKey;
                    legalEntityAddress = legalEntityAddressService.InsertLegalEntityAddress(legalEntityAddress);
                }
                legalEntityAddressService.InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, client.LegalEntityKey, GeneralStatusEnum.Active);
            }
        }

        public void SetupClientsWithDifferentDomiciliumsOnDebtCounsellingCase(int debtCounsellingKey)
        {
            var clientList = externalRoleService.GetActiveExternalRoleList(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            foreach (var client in clientList)
            {
                legalEntityAddressService.CleanupLegalEntityAddresses(client.LegalEntityKey, false, GeneralStatusEnum.Inactive);
                legalEntityAddressService.DeleteLegalEntityDomiciliumAddress(client.LegalEntityKey);
                var legalEntityAddress = legalEntityAddressService.InsertLegalEntityAddressByAddressType(client.LegalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
                legalEntityAddressService.InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, client.LegalEntityKey, GeneralStatusEnum.Active);
            }
        }

        public void ChangeDebtCounsellor(int debtCounsellorLegalEntitKey, int accountKey)
        {
            base.UpdateDebtCounsellingCompany(debtCounsellorLegalEntitKey, accountKey);
        }
    }
}