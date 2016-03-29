using System;
using BuildingBlocks;
using WatiN.Core;
using WatiN.Core.Logging;
using NUnit.Framework;
using CommonData.Enums;
using CommonData.Constants;
using SQLQuerying;
using Meyn.TestLink;
using Common = BuildingBlocks.Common;
using Navigation = BuildingBlocks.Navigation;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests
{
	[TestFixture, RequiresSTA]
#if !DEBUG
	[TestLinkFixture(
		Url = "http://sahls31:8181/testlink/lib/api/xmlrpc.php",
		ProjectName = "HALO Automation",
		TestPlan = "Regression Tests",
		TestSuite = "Loan Servicing Tests",
		UserId = "admin",
		DevKey = "896f902c0397d7c1849290a44d0f6fb5")]
#endif
	public sealed class ManualDebitOrder
    {
        #region Tests
        [Test]
        public void AddManualDebitOrder()
        {
            var browser = new TestBrowser(TestUsers.DebtCounsellingConsultant, TestUsers.Password);
            var debtcounsellingRecord = Common.DebtCounselling.GetRandomDebtCounsellingRecord
                    (
                        ExternalRoleTypeEnum.DebtCounsellor,
                        DebtCounsellingStatus.Open,
                        false,
                        WorkflowStates.DebtCounsellingWF.PendPayment,
                        WorkflowStates.DebtCounsellingWF.TermReview,
                        WorkflowStates.DebtCounsellingWF.Termination,
                        WorkflowStates.DebtCounsellingWF.ReviewNotification,
                        WorkflowStates.DebtCounsellingWF.DecisiononProposal
                    );

            Navigation.Helper.Task(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, debtcounsellingRecord.AccountKey.ToString(), WorkflowStates.DebtCounsellingWF.ManageProposal);

            browser.Navigate<Navigation.LoanDetailsNode>().ClickLoanDetailsNode();
            browser.Navigate<Navigation.LoanDetailsNode>().ClickFutureDatedTransactionsNode();
            browser.Navigate<Navigation.LoanDetailsNode>().ClickAddManualDebitOrderNode();

            var manualDebitOrderDetail = new BuildingBlocks.Models.DebitOrder();
            browser.Page<Views.ManualDebitOrderAddRecurringView>().PopulateDebitOrderDetail(manualDebitOrderDetail);

        }
        [Test]
        public void UpdateManualDebitOrder()
        {
            var browser = new TestBrowser(TestUsers.DebtCounsellingConsultant, TestUsers.Password);
            var debtcounsellingRecord = Common.DebtCounselling.GetRandomDebtCounsellingRecord
                (
                    ExternalRoleTypeEnum.DebtCounsellor,
                    DebtCounsellingStatus.Open,
                    false,
                    WorkflowStates.DebtCounsellingWF.PendPayment,
                    WorkflowStates.DebtCounsellingWF.TermReview,
                    WorkflowStates.DebtCounsellingWF.Termination,
                    WorkflowStates.DebtCounsellingWF.ReviewNotification,
                    WorkflowStates.DebtCounsellingWF.DecisiononProposal
                );

            Navigation.Helper.Task(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, debtcounsellingRecord.AccountKey.ToString(), WorkflowStates.DebtCounsellingWF.ManageProposal);

            browser.Navigate<Navigation.LoanDetailsNode>().ClickLoanDetailsNode();
            browser.Navigate<Navigation.LoanDetailsNode>().ClickFutureDatedTransactionsNode();
            browser.Navigate<Navigation.LoanDetailsNode>().ClickAddManualDebitOrderNode();

            var manualDebitOrderDetail = new BuildingBlocks.Models.DebitOrder();
            browser.Page<Views.ManualDebitOrderUpdateView>().PopulateDebitOrderDetail(manualDebitOrderDetail);


        }
        [Test]
        public void DeleteManualDebitOrder()
        {
            var browser = new TestBrowser(TestUsers.DebtCounsellingConsultant, TestUsers.Password);
            var debtcounsellingRecord = Common.DebtCounselling.GetRandomDebtCounsellingRecord
                    (
                        ExternalRoleTypeEnum.DebtCounsellor,
                        DebtCounsellingStatus.Open,
                        false,
                        WorkflowStates.DebtCounsellingWF.PendPayment,
                        WorkflowStates.DebtCounsellingWF.TermReview,
                        WorkflowStates.DebtCounsellingWF.Termination,
                        WorkflowStates.DebtCounsellingWF.ReviewNotification,
                        WorkflowStates.DebtCounsellingWF.DecisiononProposal
                    );

            Navigation.Helper.Task(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, debtcounsellingRecord.AccountKey.ToString(), WorkflowStates.DebtCounsellingWF.ManageProposal);

            browser.Navigate<Navigation.LoanDetailsNode>().ClickLoanDetailsNode();
            browser.Navigate<Navigation.LoanDetailsNode>().ClickFutureDatedTransactionsNode();
            browser.Navigate<Navigation.LoanDetailsNode>().ClickAddManualDebitOrderNode();

            var manualDebitOrderDetail = new BuildingBlocks.Models.DebitOrder();
            browser.Page<Views.ManualDebitOrderDeleteView>().SelectDebitOrderInGrid("");
        }
        #endregion Tests

        #region Helper

        #endregion Helper
    }
}
