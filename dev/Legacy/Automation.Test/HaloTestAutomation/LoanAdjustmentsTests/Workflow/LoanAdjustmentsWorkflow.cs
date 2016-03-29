using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.RateChange;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Exceptions;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanAdjustmentsTests
{
    [RequiresSTA]
    public class LoanAdjustmentsWorkflowTests : TestBase<RateChangeTerm>
    {
        #region Private Variables

        private static readonly string DataTable = "TermChange";

        private Automation.DataModels.Account Account;
        private Automation.DataModels.Account UpdatedAccount;

        private Automation.DataModels.LoanFinancialService VariableML;
        private Automation.DataModels.LoanFinancialService UpdatedVariableML;

        #endregion Private Variables

        /// <summary>
        /// This test will create a case and setup the Term Change Request timer in order for a later test to assert that it fires correctly.
        /// </summary>
        [Test, Description("This test will create a case and setup the Term Change Request timer in order for a later test to assert that it fires correctly.")]
        public void _001_TermChangeRequestTimeout()
        {
            const string identifier = "TermChangeRequestTimeout";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            Account = base.Service<IAccountService>().GetAccountByKey(accountKey);
            CreateTermChangeCase(Account.AccountKey, newTerm, identifier, true);
            var instanceID = LoanAdjustmentsAssertions.AssertCurrentState(Account.AccountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
            //update the time of the Term Request Timeout scheduled activity
            X2Assertions.AssertScheduleActivitySetup(string.Format(@"Loan Adjustment: {0}", Account.AccountKey), ScheduledActivities.LoanAdjustments.TermRequestTimeout, 30, 0, 0);
            //fire the timer
            base.scriptEngine.ExecuteScript(WorkflowEnum.LoanAdjustments, WorkflowAutomationScripts.LoanAdjustments.TermRequestTimeout, Account.AccountKey);
        }

        /// <summary>
        /// Ensures that a Term Change request with no resultant SPV transfer can be processed by Approving Term Change
        /// </summary>
        [Test, Description("Ensures that a Term Change request with no resultant SPV transfer can be processed by Approving Term Change")]
        public void _002_ApproveNoSPVTransfer()
        {
            const string identifier = "ApproveNoSPVTransfer";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            CreateTermChangeCase(accountKey, newTerm, identifier, true);
            //assert term change case created and is at the correct state
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
            //assert case is assigned to the correct aduser at the required state
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.LoanAdjustments, accountKey, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest,
                "SPVTermChange");
            //approve the case
            ApproveOrDeclineTermChange(accountKey, identifier, true);
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, LoanAdjustmentType.TermChange, WorkflowStates.LoanAdjustmentsWF.ReviewDecision);
            MemoAssertions.AssertLatestMemoInformation(accountKey, GenericKeyTypeEnum.Account_AccountKey, MemoTable.Memo, identifier);
        }

        /// <summary>
        /// This test will login as a Credit Manager and Agree with the Term Change Request.
        /// </summary>
        [Test, Description("")]
        public void _003_ApproveNoSPVTransferAgreeDecision()
        {
            const string identifier = "ApproveNoSPVTransfer";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            int newTerm = results.Rows(0).Column("SPVMaxTermIncrease").GetValueAs<int>();
            Account = base.Service<IAccountService>().GetAccountByKey(accountKey);
            double originalInstalment = (from ml in Account.FinancialServices
                                         where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                         select ml.Payment).FirstOrDefault();
            //agree with the decision
            AgreeOrDisagreeWithTermChange(Account.AccountKey, identifier, true);
            //assert the case in now in archive and the ML remaining instalments has changed to expected new value
            LoanAdjustmentsAssertions.AssertCurrentState(Account.AccountKey, LoanAdjustmentType.TermChange,
                WorkflowStates.LoanAdjustmentsWF.ArchiveAgreeTermChange);
            UpdatedAccount = base.Service<IAccountService>().GetAccountByKey(accountKey);
            UpdatedVariableML = (from ml in UpdatedAccount.FinancialServices
                                 where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                 select ml).FirstOrDefault();
            Assert.That(UpdatedVariableML.RemainingInstalments == newTerm, "Remaining instalments were not updated with new term value.");
            Assert.That(UpdatedVariableML.Payment != originalInstalment, "Instalment was not recalculated.");
        }

        /// <summary>
        /// Ensures that a Term Change request with no resultant SPV transfer can be processed by Declining Term Change
        /// </summary>
        [Test, Description("Ensures that a Term Change request with no resultant SPV transfer can be processed by Declining Term Change")]
        public void _004_DeclineTermChange()
        {
            const string identifier = "DeclineTermChange";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            Account = base.Service<IAccountService>().GetAccountByKey(accountKey);
            VariableML = (from ml in Account.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();
            CreateTermChangeCase(accountKey, newTerm, identifier, true);
            //assert term change case created and is at the correct state
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
            //assert case is assigned to the correct aduser at the required state
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.LoanAdjustments, accountKey, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest,
                "SPVTermChange");
            //decline the case
            ApproveOrDeclineTermChange(accountKey, identifier, false);
            UpdatedAccount = base.Service<IAccountService>().GetAccountByKey(accountKey);
            UpdatedVariableML = (from ml in UpdatedAccount.FinancialServices
                                 where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                 select ml).FirstOrDefault();
            //assert the case in now in archive and the ML remaining instalments has not changed
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, LoanAdjustmentType.TermChange, WorkflowStates.LoanAdjustmentsWF.ArchiveTermChange);
            Assert.That(UpdatedVariableML.RemainingInstalments == VariableML.RemainingInstalments, "Remaining instalments were incorrectly updated.");
            MemoAssertions.AssertLatestMemoInformation(UpdatedAccount.AccountKey, GenericKeyTypeEnum.Account_AccountKey, MemoTable.Memo, identifier);
        }

        /// <summary>
        /// Ensures that a Term Change request with no resultant SPV transfer can be processed by Term Change No Longer Required
        /// </summary>
        [Test, Description("Ensures that a Term Change request with no resultant SPV transfer can be processed by Term Change No Longer Required")]
        public void _005_TermChangeNoLongerRequired()
        {
            const string identifier = "TermChangeNoLongerRequired";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            Account = base.Service<IAccountService>().GetAccountByKey(accountKey);
            VariableML = (from ml in Account.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();
            CreateTermChangeCase(accountKey, newTerm, identifier, true);
            //assert term change case created and is at the correct state
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
            //assert case is assigned to the correct aduser at the required state
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.LoanAdjustments, accountKey,
                WorkflowStates.LoanAdjustmentsWF.TermChangeRequest, "SPVTermChange");
            TermChangeNoLongerRequired(accountKey, identifier);
            UpdatedAccount = base.Service<IAccountService>().GetAccountByKey(accountKey);
            UpdatedVariableML = (from ml in UpdatedAccount.FinancialServices
                                 where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                 select ml).FirstOrDefault();
            //assert the case in now in archive and the ML remaining instalments has not changed
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, LoanAdjustmentType.TermChange,
                WorkflowStates.LoanAdjustmentsWF.ArchiveNoLongerRequired);
            Assert.That(UpdatedVariableML.RemainingInstalments == VariableML.RemainingInstalments, "Remaining instalments were incorrectly updated.");
        }

        /// <summary>
        /// Ensures that a Term Change request which results in a SPV transfer is transfered correctly
        /// </summary>
        [Test, Description("Ensures that a Term Change request which results in a SPV transfer is transfered correctly")]
        public void _006_ApproveWithSPVTransfer()
        {
            const string identifier = "ApproveWithSPVTransfer";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SAHLMaxTermIncrease").Value;
            CreateTermChangeCase(accountKey, newTerm, identifier, true);
            //assert term change case created and is at the correct state
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
            //assert case is assigned to the correct aduser at the required state
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.LoanAdjustments, accountKey,
                WorkflowStates.LoanAdjustmentsWF.TermChangeRequest, "SPVTermChange");
            //approve the case
            ApproveOrDeclineTermChange(accountKey, identifier, true);
            //assert the case in now in archive and the ML remaining instalments has changed to expected new value
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, LoanAdjustmentType.TermChange, WorkflowStates.LoanAdjustmentsWF.ReviewDecision);
        }

        /// <summary>
        /// This test will login as the Credit Manager user and agree with the Term Change Request. It will the check that the SPV has been moved, the relevant SPV
        /// movement transactions have been posted, the instalment has been recalculated and the the Term Change case has been archive correctly.
        /// </summary>
        [Ignore]
        [Test, Description(@"This test will login as the Credit Manager user and agree with the Term Change Request. It will the check that the SPV has been moved, the relevant SPV
        movement transactions have been posted, the instalment has been recalculated and the the Term Change case has been archive correctly.")]
        public void _007_ApproveWithSPVTransferAgreeWithDecision()
        {
            const string identifier = "ApproveWithSPVTransfer";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            int newTerm = results.Rows(0).Column("SAHLMaxTermIncrease").GetValueAs<int>();
            Account = base.Service<IAccountService>().GetAccountByKey(accountKey);
            VariableML = (from ml in Account.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();

            AgreeOrDisagreeWithTermChange(accountKey, identifier, true);

            UpdatedAccount = base.Service<IAccountService>().GetAccountByKey(accountKey);
            UpdatedVariableML = (from ml in UpdatedAccount.FinancialServices
                                 where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                 select ml).FirstOrDefault();

            // 1. On approval of the term change the account is transferred to the new SPV
            Assert.That(UpdatedAccount.SPVKey != Account.SPVKey, "Account did not transfer to new SPV.");
            // 2. On approval of the term change all applicable loan transactions for the SPV transfer occur
            LoanAdjustmentsAssertions.AssertSPVMovementTransactions(UpdatedAccount.AccountKey);
            // 3. On approval of the term change the account's term value is updated to the new term requested by the user who created the term change
            Assert.That(UpdatedVariableML.RemainingInstalments == newTerm, "Remaining instalments were not updated with new term value.");
            // 4. On approval of the term change the instalment due on the mortgage loan is recalculated
            Assert.That(UpdatedVariableML.Payment != VariableML.Payment, "Instalment was not recalculated.");
            //assert the case in now in archive
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, LoanAdjustmentType.TermChange,
                WorkflowStates.LoanAdjustmentsWF.ArchiveAgreeTermChange);
        }

        /// <summary>
        /// This will test that a term change request can be made when the current term on the loan is zero.
        /// This  test is being ignored for now as currently the term cannot be set to zero.
        /// </summary>
        [Ignore]
        [Test, Description("This test ensures that a term change request can be processed on a mortgage loan account with a zero term. This  test is being ignored for now as currently the term cannot be set to zero.")]
        public void _008_TermChangeZeroTerm()
        {
            const string identifier = "TermChangeZeroTerm";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);

            if (results.Rows(0).Column("OpenOffer").Value.Equals("1"))
                throw new WatiNException(String.Format("The account used for TestIdentifier {0} already have an open workflow case.", identifier));

            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            CreateTermChangeCase(accountKey, newTerm, identifier, true);
            //Assert that we have a loan adjustments workflow case.
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
        }

        /// <summary>
        /// This will assert that the TermChangeRequest timer fired in the _001_TermChangeRequestTimeout test, and that the cases remains
        /// in the same state.
        /// </summary>
        [Test, Description(@"This will assert that the TermChangeRequest timer fired in the _001_TermChangeRequestTimeout test, and that the cases remains
        in the same state.")]
        public void _009_TermChangeRequestTimeoutAssertion()
        {
            const string identifier = "TermChangeRequestTimeout";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            //check the timer fired
            LoanAdjustmentsAssertions.AssertTimeOut(accountKey);
            //check the case is in the same state
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
        }

        /// <summary>
        /// This test ensures that if a current term change case exists on an account then a new one cannot be created for the same account
        /// </summary>
        [Test, Description("This test ensures that if a current term change case exists on an account then a new one cannot be created for the same account")]
        public void _010_TermChangeCaseExists()
        {
            const string identifier = "TermChangeRequestTimeout";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            CreateTermChangeCase(accountKey, newTerm, identifier, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("An application to move this account between SPV's is already in progress");
        }

        /// <summary>
        /// This test will approve a term change request as a Credit Supervisor user and then login as a Credit Manager in order to Disagree with the
        /// supervisors decision. This will result in the term change case being archived.
        /// </summary>
        [Test, Description(@"This test will approve a term change request as a Credit Supervisor user and then login as a Credit Manager in order to Disagree with the
        supervisors decision. This will result in the term change case being archived.")]
        public void _011_DisagreeWithTermChangeApproval()
        {
            const string identifier = "TermChangeRequestDisagreeApproval";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;

            Account = base.Service<IAccountService>().GetAccountByKey(accountKey);
            VariableML = (from ml in Account.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();

            CreateTermChangeCase(accountKey, newTerm, identifier, true);
            //assert term change case created and is at the correct state
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest);
            //assert case is assigned to the correct aduser at the required state
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.LoanAdjustments, accountKey, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest,
                "SPVTermChange");
            //approve the case
            ApproveOrDeclineTermChange(accountKey, identifier, true);
            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, LoanAdjustmentType.TermChange, WorkflowStates.LoanAdjustmentsWF.ReviewDecision);
            MemoAssertions.AssertLatestMemoInformation(accountKey, GenericKeyTypeEnum.Account_AccountKey, MemoTable.Memo, identifier);
            //disagree with the decision
            AgreeOrDisagreeWithTermChange(accountKey, identifier, false);

            UpdatedAccount = base.Service<IAccountService>().GetAccountByKey(accountKey);
            UpdatedVariableML = (from ml in UpdatedAccount.FinancialServices
                                 where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                 select ml).FirstOrDefault();

            LoanAdjustmentsAssertions.AssertCurrentState(accountKey, 1, WorkflowStates.LoanAdjustmentsWF.ArchiveTermChange);
            //term shouldnt have changed
            Assert.That(UpdatedVariableML.RemainingInstalments == VariableML.RemainingInstalments, "Remaining instalments were incorrectly updated.");
        }

        /// <summary>
        /// This test case ensures that a term change request cannot be processed on a VariFix account
        /// </summary>
        [Test, Description("This test case ensures that a term change request cannot be processed on a VariFix account")]
        public void _012_TermChangeVariFix()
        {
            const string identifier = "NoTermChangeVarifix";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            CreateTermChangeCase(accountKey, newTerm, identifier, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot process a term change request on a Varifix account.");
        }

        /// <summary>
        /// This test case ensures that a term change request cannot be processed on an account when further lending is in progress
        /// </summary>
        [Test, Description("This test case ensures that a term change request cannot be processed on an account when further lending is in progress")]
        public void _013_TermChangeFurtherLendingInProgress()
        {
            const string identifier = "TermChangeFLinProgress";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").GetValueAs<string>();
            CreateTermChangeCase(accountKey, newTerm, identifier, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Further Lending is currently in progress. Term Change cannot be processed");
        }

        /// <summary>
        /// This test ensures that the user cannot enter no value or a value greater than 360 into the new term field when creating a test.
        /// </summary>
        [Test, Sequential, Description(
                @"This test ensures that the user cannot enter no value or a value greater than 360 into the new term field when creating a test.")]
        public void _014_TermChangeNewTermValidation([Values("", "361")] string newTerm)
        {
            const string identifier = "TermChangeTerm360";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            //navigate to the change term screen
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ChangeTerm();
            //provide the new remaining term value
            base.View.NewTermValue(newTerm);
            //click Calculate
            base.View.CalculateTermChange();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The remaining term must be between 1 and 360 months.");
        }

        /// <summary>
        /// This test will ensure that a term change case cannot be created on a case where the new remaining term will result
        /// in the overall life of the loan exceeding the SAHL maximum term, which is currently 360 months.
        /// </summary>
        [Test, Description(@"This test will ensure that a term change case cannot be created on a case where the new remaining term will result
        in the overall life of the loan exceeding the SAHL maximum term, which is currently 360 months.")]
        public void _015_TermChangeOverallTermOverSAHLMax()
        {
            const string identifier = "TermChangeTerm360";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            int newTerm = int.Parse(results.Rows(0).Column("SAHLMaxTermIncrease").Value) + 1;
            CreateTermChangeCase(accountKey, newTerm.ToString(), identifier, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The remaining term will exceed 360 months.");
        }

        /// <summary>
        /// This test will ensure that the user will have to provide a comment before the term change case can be created.
        /// </summary>
        [Test, Description("This test will ensure that the user will have to provide a comment before the term change case can be created.")]
        public void _016_TermChangeCommentMandatory()
        {
            const string identifier = "TermChangeTerm360";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            //navigate to the change term screen
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ChangeTerm();
            //provide the new remaining term value
            base.View.NewTermValue(newTerm);
            //click Calculate
            base.View.CalculateTermChange();
            base.View.ProcessTermChange(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Term Change Request, please add comment.");
        }

        /// <summary>
        /// This test will ensure that the user cannot perform a term change for an account that is under debt counselling.
        /// </summary>
        [Test, Description("This test will ensure that the user cannot perform a term change for an account that is under debt counselling.")]
        public void _017_TermChangeUnderDebtCounselling()
        {
            const string identifier = "UnderDebtCounselling";
            var results = Service<ICommonService>().GetTestData(identifier, DataTable);
            int accountKey = results.Rows(0).Column("Account").GetValueAs<int>();
            string newTerm = results.Rows(0).Column("SPVMaxTermIncrease").Value;
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            //navigate to the change term screen
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ChangeTerm();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("This Account is undergoing Debt Counselling.");
            base.View.AssertProcessTermChangeDisabled();
        }

        #region HelperMethods

        /// <summary>
        /// Helper method used to create a term change case
        /// </summary>
        /// <param name="accountKey">Account to use</param>
        /// <param name="newTerm">New Required Term</param>
        /// <param name="identifier">TestIdentifier</param>
        /// <param name="disposeBrowser">Whether or not to get rid of the browser after creating, used to check for domain warnings</param>
        private void CreateTermChangeCase(int accountKey, string newTerm, string identifier, bool disposeBrowser)
        {
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            //navigate to the change term screen
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ChangeTerm();
            //provide the new remaining term value
            base.View.NewTermValue(newTerm);
            //provide a comment
            base.View.AddComment(identifier);
            //click Calculate
            base.View.CalculateTermChange();
            //click Process Term Change
            if (disposeBrowser)
            {
                base.View.ProcessTermChange(true, true);
                base.Browser.Dispose();
                base.Browser = null;
            }
            else
            {
                base.View.ProcessTermChange(true, false);
            }
        }

        /// <summary>
        /// Helper method used to Approve or Decline a term change case
        /// </summary>
        /// <param name="accountKey">Account to use</param>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="Approve">TRUE = Approve, FALSE = Decline</param>
        private void ApproveOrDeclineTermChange(int accountKey, string identifier, bool Approve)
        {
            //log on as credit supervisor
            base.Browser = new TestBrowser(TestUsers.CreditSupervisor, TestUsers.Password);
            //select case off worklist
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest, accountKey);
            //approve the term change
            if (Approve)
            {
                base.Browser.ClickAction(WorkflowActivities.LoanAdjustments.ApproveTermChange);
            }
            else
            {
                base.Browser.ClickAction(WorkflowActivities.LoanAdjustments.DeclineTermChange);
            }
            base.Browser.Page<GenericMemoAdd>().AddMemoRecord(MemoStatus.Resolved, identifier);
            base.Browser.Dispose();
            base.Browser = null;
        }

        /// <summary>
        /// Helper method used to Agree or Disagree with a term change decision
        /// </summary>
        /// <param name="accountKey">Account to use</param>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="Agree">TRUE = Agree, FALSE = Disagree</param>
        private void AgreeOrDisagreeWithTermChange(int accountKey, string identifier, bool Agree)
        {
            //we need to login as the Credit Manager and either agree or disagree with the decision
            base.Browser = new TestBrowser(TestUsers.CreditManager, TestUsers.Password);
            //select case off worklist
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.LoanAdjustmentsWF.ReviewDecision, accountKey);
            //disagree or agree with the approval
            if (Agree)
            {
                base.Browser.ClickAction(WorkflowActivities.LoanAdjustments.AgreeWithDecision);
            }
            else
            {
                base.Browser.ClickAction(WorkflowActivities.LoanAdjustments.DisagreeWithDecision);
            }
            base.Browser.Page<GenericMemoAdd>().AddMemoRecord(MemoStatus.Resolved, identifier, true);
            base.Browser.Dispose();
            base.Browser = null;
        }

        /// <summary>
        /// Helper method used to login and perform the No Longer Required action
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="identifier">Test Identifier</param>
        private void TermChangeNoLongerRequired(int accountKey, string identifier)
        {
            //log on as credit manager
            base.Browser = new TestBrowser(TestUsers.CreditSupervisor, TestUsers.Password);
            //select case off worklist
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.LoanAdjustmentsWF.TermChangeRequest, accountKey);
            //no longer required
            base.Browser.ClickAction(WorkflowActivities.LoanAdjustments.NoLongerRequired);
            base.Browser.Page<GenericMemoAdd>().AddMemoRecord(MemoStatus.Resolved, identifier);
            base.Browser.Dispose();
            base.Browser = null;
        }

        #endregion HelperMethods
    }
}