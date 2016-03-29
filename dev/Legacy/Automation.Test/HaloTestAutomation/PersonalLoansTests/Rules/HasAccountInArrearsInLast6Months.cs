using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.Rules
{
    [RequiresSTA]
    public class HasAccountInArrearsInLast6Months : PersonalLoansWorkflowTestBase<BasePageAssertions>
    {
        /// <summary>
        /// Test will ensure that a warning message is displayed when the associated mortgage loan account has been in arrears in the last 6 months
        /// </summary>
        [Test, Description("Test will ensure that a warning message is displayed when the associated mortgage loan account has been in arrears in the last 6 months")]
        public void AccountHasBeenInArrearsInLast6Months()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            var mortgageLoanAccountKey = base.Service<ILegalEntityService>().GetRelatedMortgageLoanAccountKeyByOfferKey(base.GenericKey);
            UpdateArrearBalance(mortgageLoanAccountKey);
            //select the personal loan node again
            base.Browser.ClickAction(string.Format("Personal Loan : {0} (Personal Loans)", base.GenericKey));
            //ensure warning message is displayed
            base.View.AssertValidationMessageExists(
                string.Format(@"The Mortgage Loan Account [{0}] is currently in arrears or has been in arrears in the last 6 months", mortgageLoanAccountKey));
        }

        /// <summary>
        /// If the latest decision on the application is an approval then the warning message should not be displayed to the user.
        /// </summary>
        [Test, Description("If the latest decision on the application is an approval then the warning message should not be displayed to the user.")]
        public void ArrearsWarningIsRemovedAfterCreditApproval()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD);
            var mortgageLoanAccountKey = base.Service<ILegalEntityService>().GetRelatedMortgageLoanAccountKeyByOfferKey(base.GenericKey);
            UpdateArrearBalance(mortgageLoanAccountKey);
            //select the personal loan node again
            base.Browser.ClickAction(string.Format("Personal Loan : {0} (Personal Loans)", base.GenericKey));
            string msg = string.Format(@"The Mortgage Loan Account [{0}] is currently in arrears or has been in arrears in the last 6 months",
                mortgageLoanAccountKey);
            //ensure warning message is displayed
            base.View.AssertValidationMessageExists(msg);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.Approve);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //load the case as the PL Consultant
            base.ReloadCase(WorkflowStates.PersonalLoansWF.LegalAgreements, WorkflowRoleTypeEnum.PLConsultantD);
            //warning should be removed
            base.View.AssertValidationMessageDoesNotExist(msg);
        }

        /// <summary>
        /// If the latest decision on the application is a Decline then the warning message should still be displayed to the user.
        /// </summary>
        [Test, Description("If the latest decision on the application is a Decline then the warning message should still be displayed to the user.")]
        public void ArrearsWarningRemainsAfterCreditDecline()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD);
            var mortgageLoanAccountKey = base.Service<ILegalEntityService>().GetRelatedMortgageLoanAccountKeyByOfferKey(base.GenericKey);
            UpdateArrearBalance(mortgageLoanAccountKey);
            //select the personal loan node again
            base.Browser.ClickAction(string.Format("Personal Loan : {0} (Personal Loans)", base.GenericKey));
            string msg = string.Format(@"The Mortgage Loan Account [{0}] is currently in arrears or has been in arrears in the last 6 months",
                mortgageLoanAccountKey);
            //ensure warning message is displayed
            base.View.AssertValidationMessageExists(msg);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.Decline);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.PersonalLoanDecline);
            //load the case as the PL Consultant
            base.ReloadCase(WorkflowStates.PersonalLoansWF.DeclinedbyCredit, WorkflowRoleTypeEnum.PLConsultantD);
            //warning should be removed
            base.View.AssertValidationMessageExists(msg);
        }

        private void UpdateArrearBalance(int mortgageLoanAccountKey)
        {
            var arrearTransactionKey = base.Service<ILoanTransactionService>().GetLatestArrearTransactionKey(mortgageLoanAccountKey);
            //we need to update the balance of this record to be greater than R200
            double amount = 200.01;
            base.Service<ILoanTransactionService>().UpdateArrearTransactionBalance(arrearTransactionKey, amount);
        }
    }
}