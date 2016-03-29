using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.PersonalLoan;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.LoanServicing
{
    [RequiresSTA]
    public class CreditLifePolicyTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        private Automation.DataModels.LoanFinancialService PersonalLoanFinancialService { get; set; }

        private Automation.DataModels.Account CreditLifePolicy { get; set; }

        #region Tests

        /// <summary>
        /// This test verifies that one can create an SAHL Credit Life policy for an open personal loan account.
        /// </summary>
        [Test, Description(@"This test verifies that one can create an SAHL Credit Life policy for an open personal loan account.")]
        public void when_creating_sahl_credit_life_for_disbursed_account_sahl_policy_is_created()
        {
            // Find a Personal Loan Account with an external life policy
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(false, TestUsers.PersonalLoanClientServiceUser);

            // Create the SAHL Credit Life Policy
            base.Browser.Navigate<PersonalLoanNode>().ClickCreateCreditLifePolicy();
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            // Assert that the SAHL Credit Life Policy is now in place
            var results = Service<IAccountService>().GetOpenRelatedAccountsByProductKey(base.GenericKey, Common.Enums.ProductEnum.SAHLCreditProtectionPlan);
            CreditLifePolicy = Service<IAccountService>().GetAccountByKey(results.Rows(0).Column("AccountKey").GetValueAs<int>());
            Assert.That(CreditLifePolicy.AccountStatusKey == Common.Enums.AccountStatusEnum.Open);
            Assert.That(CreditLifePolicy.ProductKey == Common.Enums.ProductEnum.SAHLCreditProtectionPlan);
        }

        /// <summary>
        /// This test verifies that you cannot add the SAHL Credit Life policy to an open Personal Loan which already has an active SAHL Credit Life policy.
        /// </summary>
        [Test, Description(@"This test verifies that you cannot add the SAHL Credit Life policy to an open Personal Loan which already has an SAHL Credit Life policy.")]
        public void when_creating_sahl_credit_life_for_disbursed_account_with_sahl_life_assert_that_no_policy_created()
        {
            // Find a Personal Loan Account with an SAHL Credit Life policy
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(true, TestUsers.PersonalLoanClientServiceUser);

            // Attempt to create the SAHL Credit Life Policy
            base.Browser.Navigate<PersonalLoanNode>().ClickCreateCreditLifePolicy();
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            // Assert that the correct validation message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("An open SAHL Credit Protection Plan account already exists.");
        }

        /// <summary>
        /// This test verifies that a a PL Client Service user can send a life policy instatement letter for personal loans which have an SAHL Credit Life policy which was added post disbursement.
        /// </summary>
        [Test, Description(@"This test verifies that a a PL Client Service user can send a life policy instatement letter for personal loans which have an SAHL Credit Life policy which was added post disbursement.")]
        public void when_sahl_credit_life_is_created_post_disbursement_assert_that_user_can_send_life_policy_instatement_letter()
        {
            // Find a disbursed Personal Loan Account
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(true, TestUsers.PersonalLoanClientServiceUser);

            // Check that the "Credit Life Policy Instatement Letter" is available under the Correspondence node in the CBO and then send the letter
            base.Browser.Navigate<PersonalLoanNode>().ClickCorrespondence();
            base.Browser.Navigate<PersonalLoanNode>().ClickCreditLifePolicyInstatementLetter();
            base.Browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);

            // Assert that a record of the email being sent is written to the database.
            CorrespondenceAssertions.AssertImageIndex(base.GenericKey.ToString(), CorrespondenceReports.CreditLifePolicyInstatementLetter, CorrespondenceMedium.Email, base.GenericKey, 0);
        }

        #endregion Tests
    }
}