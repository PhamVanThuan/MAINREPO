using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace PersonalLoansTests.Rules
{
    [RequiresSTA]
    public class CreateCaseLegalEntityUnderDebtCounselling : PersonalLoansWorkflowTestBase<BasePageAssertions>
    {
        private Automation.DataModels.Account account;
        private Dictionary<int, LegalEntityTypeEnum> roles;
        private int legalEntityKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.PersonalLoanConsultant2, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //we need an account that is under DC
            account = Service<IAccountService>().GetRandomMortgageLoanAccountWithPositiveBalance(ProductEnum.NewVariableLoan, AccountStatusEnum.Open);
            roles = Service<IAccountService>().AccountRoleLegalEntityKeys(account.AccountKey);
            legalEntityKey = (from r in roles select r.Key).FirstOrDefault();
        }

        /// <summary>
        /// A user should not be able to create a personal loan application when the legal entity is under debt counselling on their SAHL mortgage loan
        /// account.
        /// </summary>
        [Test]
        public void CannotCreateCaseWhenLegalEntityIsUnderDebtCounselling()
        {
            //create a dc case
            var caseCreated = Service<IDebtCounsellingService>().CreateDebtCounsellingCase(account.AccountKey);
            if (!caseCreated)
                Assert.Fail("Debt Counselling Case Create Failed");
            //go create
            Helper.LoadLegalEntityOnCBO(base.Browser, legalEntityKey, account.AccountKey);
            //create lead
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<WorkflowYesNo>().ClickYes();
            //check for DC Warning
            base.View.AssertValidationMessagesContains("is under Debt Counselling");
        }
    }
}