using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.DebtCounselling;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class ExternalRoles : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test will navigate to the Manage Debt Counsellor node on the FLOBO and change the Debt Counsellor currently assigned to the case.
        /// </summary>
        [Test, Description("This test will navigate to the Manage Debt Counsellor node on the FLOBO and change the Debt Counsellor currently assigned to the case.")]
        public void ManageDebtCounsellor()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //navigate to the debt counsellor node
            base.Browser.Navigate<ExternalRolesNode>().ManageDebtCounsellor();
            int newDCLegalEntityKey = WorkflowHelper.ManageDebtCounsellor(base.Browser, base.TestCase.DebtCounsellingKey);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ChangeofDebtCounselor);
            ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                 ExternalRoleTypeEnum.DebtCounsellor, newDCLegalEntityKey);
        }

        /// <summary>
        /// This test will navigate to the Manage Attorney node on the FLOBO and change Attorney currently assigned to the case.
        /// </summary>
        [Test, Description("This test will navigate to the Manage Attorney node on the FLOBO and change Attorney currently assigned to the case.")]
        public void ManageAttorney()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //select an attorney
            base.Browser.Navigate<ExternalRolesNode>().ManageAttorney();
            WorkflowHelper.SelectLitigationAttorney(base.Browser, base.TestCase.DebtCounsellingKey);
        }

        /// <summary>
        /// This test will navigate to the Manage PDA node on the FLOBO and change the PDA currently assigned to the case.
        /// </summary>
        [Test, Description("This test will navigate to the Manage PDA node on the FLOBO and change the PDA currently assigned to the case.")]
        public void ManagePaymentDistributionAgent()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<ExternalRolesNode>().ManagePDA();
            int newPDALegalEntityKey = WorkflowHelper.ManagePDA(base.Browser, base.TestCase.DebtCounsellingKey);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ChangeofPaymentDistributionAgent);
            ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.PaymentDistributionAgent, newPDALegalEntityKey);
        }

        /// <summary>
        /// This test will change the PDA and the Debt Counsellor on a single account that has related accounts in its group. The test will then ensure
        /// that the stage transition is written for each account and that the external role is changed on all of the accounts.
        /// </summary>
        [Test, Description(@"This test will change the PDA and the Debt Counsellor on a single account that has related accounts in its group. The test will then ensure
		that the stage transition is written for each account and that the external role is changed on all of the accounts.")]
        public void ManageExternalRolesGroupedCases()
        {
            var testCases = WorkflowHelper.CreateCaseAndSendToState(WorkflowStates.DebtCounsellingWF.PendProposal, false, 2, false);
            //get the first one
            base.TestCase = (from t in testCases select t).FirstOrDefault();
            base.LoadCase(WorkflowStates.DebtCounsellingWF.PendProposal);
            base.Browser.Navigate<ExternalRolesNode>().ManagePDA();
            int newPDALegalEntityKey = WorkflowHelper.ManagePDA(base.Browser, base.TestCase.DebtCounsellingKey);
            base.Browser.Navigate<ExternalRolesNode>().ManageDebtCounsellor();
            var newDCLegalEntityKey = WorkflowHelper.ManageDebtCounsellor(base.Browser, base.TestCase.DebtCounsellingKey);
            foreach (var t in testCases)
            {
                //we want to run the assertions on all the accounts
                StageTransitionAssertions.AssertStageTransitionCreated(t.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ChangeofPaymentDistributionAgent);
                ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(t.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.PaymentDistributionAgent, t.DebtCounsellingKey);
                StageTransitionAssertions.AssertStageTransitionCreated(t.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ChangeofDebtCounselor);
                ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(t.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.DebtCounsellor, newDCLegalEntityKey);
            }
        }
    }
}