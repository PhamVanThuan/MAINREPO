using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Views
{
    [RequiresSTA]
    public sealed class ManageDCLegalEntity : DebtCounsellingTests.TestBase<DebtCounsellingMaintainLegalEntitiesView>
    {
        private string[] stateList { get; set; }

        private string[] userList { get; set; }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
            stateList = new string[] { WorkflowStates.DebtCounsellingWF.ManageProposal,
                WorkflowStates.DebtCounsellingWF.PendProposal,
                WorkflowStates.DebtCounsellingWF.AcceptedProposal,
                WorkflowStates.DebtCounsellingWF.Termination,
                WorkflowStates.DebtCounsellingWF.IntenttoTerminate,
                WorkflowStates.DebtCounsellingWF.TerminationLetterSent,
                WorkflowStates.DebtCounsellingWF.PendPayment,
                WorkflowStates.DebtCounsellingWF._45DayReminder,
                WorkflowStates.DebtCounsellingWF.DebtReviewApproved,
                WorkflowStates.DebtCounsellingWF.PendCancellation
            };
            userList = new string[] {
                TestUsers.DebtCounsellingConsultant
            };
        }

        /// <summary>
        /// Add a legal entity to a debt counselling case
        /// </summary>
        [Test, Description(@"Test ensures a user is able to add and remove a legal entity onto a debt counselling case")]
        public void AddAndRemoveLegalEntity()
        {
            int accountKey = Service<IDebtCounsellingService>().GetDebtCounsellingCaseWithMoreThanOneLegalEntity(userList, stateList);
            base.TestCase = Service<IDebtCounsellingService>().GetDebtCounsellingCases(accountkey: accountKey).FirstOrDefault();
            base.LoadCase(stateList);
            //select Maintain Legal Entities action - this will click the view LE first then select maintain
            base.Browser.Navigate<LegalEntityNode>().ClickLegalEntities(NodeTypeEnum.Maintain);
            int legalEntitySelected = base.Browser.Page<DebtCounsellingMaintainLegalEntitiesView>().SelectFirstActiveLegalEntity(base.TestCase.DebtCounsellingKey);
            base.Browser.Page<DebtCounsellingMaintainLegalEntitiesView>().ClickRemoveLegalEntity();
            ExternalRoleAssertions.AssertActiveExternalRoleDoesNotExistForLegalEntity(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.Client, legalEntitySelected);
            //active legal entity is selected once current entity is deactivated - inactive one must be reselected
            base.Browser.Page<DebtCounsellingMaintainLegalEntitiesView>().SelectLegalEntity(legalEntitySelected);
            base.Browser.Page<DebtCounsellingMaintainLegalEntitiesView>().ClickAddLegalEntity();
            ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.Client, legalEntitySelected);
        }

        /// <summary>
        /// Remove the only legal entity on a debt counselling case
        /// </summary>
        /// TODO: REWORK THIS TEST TO REMOVE ALL LEGAL ENTITIES ON THE DEBT COUNSELLING CASE
        [Test, Description(@"Test ensures that user cannot remove the only entity on a dc case")]
        public void RemoveOnlyLegalEntityOnCase()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //get the externalRoles
            var clientRoles = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            base.Browser.Navigate<LegalEntityNode>().ClickLegalEntities(NodeTypeEnum.Maintain);
            string formattedName = string.Empty;
            int legalEntityKey = 0;
            foreach (var client in clientRoles)
            {
                base.View.SelectLegalEntity(client.LegalEntityKey);
                base.View.ClickRemoveLegalEntity();
                formattedName = Service<ILegalEntityService>().GetLegalEntityLegalName(client.LegalEntityKey);
                legalEntityKey = client.LegalEntityKey;
            }
            string message = string.Format("{0} is the only legal entity linked to this debt counselling case and cannot be removed.", formattedName);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(message);
            ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.Client, legalEntityKey);
        }

        /// <summary>
        /// This test will check that the correct warning is displayed on the screen when removing a legal entity that is active on another debt counselling case.
        /// </summary>
        [Test, Description(@"This test will check that the correct warning is displayed on the screen when removing a legal entity that is active on another
        debt counselling case.")]
        public void RemoveLegalEntityWithGroupedAccounts()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, countOfAccounts: 2, searchForCase: false, eWorkCase: false);
            //navigate to Maintain Legal Entities screen
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().ClickLegalEntities(NodeTypeEnum.Maintain);
            int selectedLegalEntityKey = base.Browser.Page<DebtCounsellingMaintainLegalEntitiesView>().SelectFirstActiveLegalEntity(base.TestCase.DebtCounsellingKey);
            //fetch the actual warning message
            string actualWarningMSG = base.Browser.Page<DebtCounsellingMaintainLegalEntitiesView>().GetGroupedAccountsWarningLabel();
            //generate the expected warning message
            string formattedName = Service<ILegalEntityService>().GetLegalEntityLegalName(selectedLegalEntityKey);
            var groupedAccounts = Service<IDebtCounsellingService>().GetRelatedDebtCounsellingAccounts(base.TestCase.DebtCounsellingKey);
            int relatedAccountKey = groupedAccounts[0];
            string expectedWarningMSG = string.Format("{0} is a client on the following debt counselling account(s): {1} ", formattedName, relatedAccountKey);
            //check that the warning message is displayed
            StringAssert.AreEqualIgnoringCase(expectedWarningMSG, actualWarningMSG);
        }
    }
}