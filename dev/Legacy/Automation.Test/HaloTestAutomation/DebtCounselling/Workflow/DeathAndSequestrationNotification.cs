using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    /// <summary>
    ///
    /// </summary>
    [RequiresSTA]
    public class DeathAndSequestrationNotificationTests : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test will perform the Notified of Death action on a debt counselling case ensuring that the case moves to the Estates and Sequestration state.
        /// </summary>
        [Test, Description(@"This test will perform the Notified of Death action on a debt counselling case ensuring that the case moves to the Estates and
            Sequestration state.")]
        public void NotifiedOfDeathSingleLegalEntity()
        {
            base.StartTestWithMultipleClientsUnderDebtCounselling(new string[] { TestUsers.DebtCounsellingConsultant }, WorkflowStates.DebtCounsellingWF.ManageProposal);
            var clientRoles = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            foreach (var client in clientRoles)
            {
                Service<IReasonService>().RemoveReasonsAgainstGenericKeyByReasonType(client.LegalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, ReasonTypeEnum.DebtCounsellingNotification);
            }
            //search for the case
            NotificationOfDeath();
        }

        /// <summary>
        ///  This test will perform the Notified of Sequestration action on a debt counselling case ensuring that the case moves to the Estates and Sequestration state.
        /// </summary>
        [Test, Description(@"This test will perform the Notified of Sequestration action on a debt counselling case ensuring that the case moves to the Estates
            and Sequestration state.")]
        public void NotifiedOfSequestrationSingleLegalEntity()
        {
            base.StartTestWithMultipleClientsUnderDebtCounselling(new string[] { TestUsers.DebtCounsellingConsultant }, WorkflowStates.DebtCounsellingWF.ManageProposal);
            var clientRoles = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            foreach (var client in clientRoles)
            {
                Service<IReasonService>().RemoveReasonsAgainstGenericKeyByReasonType(client.LegalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, ReasonTypeEnum.DebtCounsellingNotification);
            }
            //we need all of the LE's on our account
            NotificationOfSequestration();
        }

        /// <summary>
        /// Ensures that all of the legal entities are displayed on the screen for selection.
        /// </summary>
        [Test, Description("Ensures that all of the legal entities are displayed on the screen for selection.")]
        public void NotifiedOfDeathCheckLegalEntities()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotifiedofDeath);
            //we need all of the LE's on our account
            var leKeys = base.Service<IAccountService>().AccountRoleLegalEntityKeys(base.TestCase.AccountKey);
            base.Browser.Page<LegalEntitySequestrationDeathNotify>().AssertLegalEntityCheckBoxes(leKeys, NotificationTypeEnum.Death);
        }

        /// <summary>
        /// Ensures that all of the legal entities are displayed on the screen for selection.
        /// </summary>
        [Test, Description("Ensures that all of the legal entities are displayed on the screen for selection.")]
        public void NotifiedOfSequestrationCheckLegalEntities()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotifiedofSequestration);
            //we need all of the LE's on our account
            var leKeys = base.Service<IAccountService>().AccountRoleLegalEntityKeys(base.TestCase.AccountKey);
            base.Browser.Page<LegalEntitySequestrationDeathNotify>().AssertLegalEntityCheckBoxes(leKeys, NotificationTypeEnum.Sequestration);
        }

        /// <summary>
        /// Marks all of the Legal Entities on the account as being notified of their death
        /// </summary>
        [Test, Description("Marks all of the Legal Entities on the account as being notified of their death")]
        public void NotifiedOfDeathAllLegalEntitiesOnAccount()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, searchForCase: false);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotifiedofDeath);
            //we need all of the LE's on our account
            var leKeys = base.Service<IAccountService>().AccountRoleLegalEntityKeys(base.TestCase.AccountKey);
            WorkflowHelper.NotificationOfDeathOrSequestrationMultipleLegalEntities(base.Browser, leKeys, base.TestCase.InstanceID, base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, NotificationTypeEnum.Death,
                ReasonDescription.NotificationofDeath, CorrespondenceTemplateEnum.DeceasedNotificationNoLiving, base.TestCase.AccountKey);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_NotifiedofDeath);
        }

        /// <summary>
        /// Marks all of the Legal Entities on the account as being under sequestration"
        /// </summary>
        [Test, Description("Marks all of the Legal Entities on the account as being under sequestration")]
        public void NotifiedOfSequestrationAllLegalEntitiesOnAccount()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, searchForCase: false);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotifiedofSequestration);
            //we need all of the LE's on our account
            var leKeys = base.Service<IAccountService>().AccountRoleLegalEntityKeys(base.TestCase.AccountKey);
            WorkflowHelper.NotificationOfDeathOrSequestrationMultipleLegalEntities(base.Browser, leKeys, base.TestCase.InstanceID, base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, NotificationTypeEnum.Sequestration,
                ReasonDescription.NotificationofSequestration, CorrespondenceTemplateEnum.SequestrationNotificationNoOthers, base.TestCase.AccountKey);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_NotifiedofSequestration);
        }

        /// <summary>
        /// Workflow Helper method that will put one legal entity on the debt counselling case under sequestration
        /// </summary>
        private void NotificationOfSequestration()
        {
            string emailAddress = Service<IExternalRoleService>().GetActiveExternalRoleEmailAddress(base.TestCase.DebtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            DateTime dt = DateTime.Now;
            var leKeys = Service<IAccountService>().AccountRoleLegalEntityKeys(base.TestCase.AccountKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotifiedofSequestration);
            var legalEntityKey = (from l in leKeys
                                  where l.Value == LegalEntityTypeEnum.NaturalPerson
                                  select l.Key).FirstOrDefault();
            base.Browser.Page<LegalEntitySequestrationDeathNotify>().SelectLegalEntityCheckBox(legalEntityKey, NotificationTypeEnum.Sequestration);
            base.Browser.Page<LegalEntitySequestrationDeathNotify>().ClickUpdateButton();
            ReasonAssertions.AssertReason(ReasonDescription.NotificationofSequestration, ReasonType.DebtCounsellingNotification, legalEntityKey,
                    GenericKeyTypeEnum.LegalEntity_LegalEntityKey);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.EstatesandSequestration);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                false, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_NotifiedofSequestration);
            CorrespondenceAssertions.AssertClientEmailByCorrespondenceTemplate(base.TestCase.DebtCounsellingKey, Common.Enums.CorrespondenceTemplateEnum.SequestrationNotificationOthersExist,
                    dt, emailAddress, base.TestCase.AccountKey);
        }

        private void NotificationOfDeath()
        {
            string emailAddress = Service<IExternalRoleService>().GetActiveExternalRoleEmailAddress(base.TestCase.DebtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            DateTime dt = DateTime.Now;
            //we need all of the LE's on our account
            var leKeys = Service<IAccountService>().AccountRoleLegalEntityKeys(base.TestCase.AccountKey);
            var legalEntityKey = (from l in leKeys
                                  where l.Value == LegalEntityTypeEnum.NaturalPerson
                                  select l.Key).FirstOrDefault();
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotifiedofDeath);
            base.Browser.Page<LegalEntitySequestrationDeathNotify>().SelectLegalEntityCheckBox(legalEntityKey, NotificationTypeEnum.Death);
            base.Browser.Page<LegalEntitySequestrationDeathNotify>().ClickUpdateButton();
            ReasonAssertions.AssertReason(ReasonDescription.NotificationofDeath, ReasonType.DebtCounsellingNotification, legalEntityKey,
                GenericKeyTypeEnum.LegalEntity_LegalEntityKey);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.EstatesandSequestration);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_NotifiedofDeath);
            CorrespondenceAssertions.AssertClientEmailByCorrespondenceTemplate(base.TestCase.DebtCounsellingKey, Common.Enums.CorrespondenceTemplateEnum.DeceasedNotificationLivingExists,
                dt, emailAddress, base.TestCase.AccountKey);
        }
    }
}