using System;
using System.Linq;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace DebtCounsellingTests.Workflow
{
    [TestFixture, RequiresSTA]
    public sealed class RecoveriesProposal : DebtCounsellingTests.TestBase<RecoveriesProposalCapture>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test performs the Capture Recoveries Proposal action. It ensures that the Debt Counselling Consultant can capture a Recoveries Proposal and the
        /// case is sent to the Recoveries Proposal state.
        /// </summary>
        [Test]
        public void CaptureRecoveriesProposal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendCancellation, TestUsers.DebtCounsellingConsultant);
            decimal shortfallAmount = 50000, repaymentAmount = 2500;
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CaptureRecoveriesProposal);
            base.Browser.Page<RecoveriesProposalCapture>().CaptureRecoveriesProposalDetails(shortfallAmount, repaymentAmount, date, false);
            //Assertions
            RecoveriesProposalAssertions.AssertActiveRecoveriesProposalExists(base.TestCase.AccountKey, (double)shortfallAmount, (double)repaymentAmount, date);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.RecoveriesProposal);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_RecoveriesProposalReceived);
        }

        /// <summary>
        /// This test ensures that the Shortfall Amount, Repayment Amount and Start Date are required fields for the capture of a Recoveries Proposal.
        /// </summary>
        [Test]
        public void CaptureInvalidRecoveriesProposal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendCancellation, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CaptureRecoveriesProposal);
            base.View.EnterZeroShortfallAmount();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please enter a Shortfall Amount greater than 0.");
            base.View.EnterZeroRepaymentAmount();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please enter a Repayment Amount greater than 0.");
            base.View.EnterInvalidStartDate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please enter a Start Date.");
        }

        /// <summary>
        /// This test performs the Escalate Recoveries Proposal action. It ensures that the Debt Counselling Consultant can escalate the Recoveries Proposal to a
        /// supervisorand for approval and that the case is sent to the Recoveries Proposal Decision state.
        /// </summary>
        [Test]
        public void EscalateRecoveriesProposal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.RecoveriesProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.EscalateRecoveriesProposal);
            base.Browser.Page<DebtCounsellingAssignSupervisor>().AssignToUser(TestUsers.DebtCounsellingSupervisor, ButtonTypeEnum.Submit);
            //Assertions
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.RecoveriesProposalDecision);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, TestUsers.DebtCounsellingSupervisor,
                WorkflowRoleTypeEnum.DebtCounsellingSupervisorD, true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_EscalateRecoveriesProposal);
        }

        #region CancellationTests

        /// <summary>
        /// Whe Under Cancellation detail type is removed, thent he case needs to move back to the stage it came from.
        /// </summary>
        [Test]
        public void UnderCancellationRemoveTest()
        {
            var debtcounsellingcase = CreateCaseAtPendProposalFromEworks();
            WorkflowHelper.CaptureDetailType(debtcounsellingcase.AccountKey, debtcounsellingcase.DebtCounsellingKey, DetailType.UnderCancellation);
            //Wait for case to move to PendCancellation
            Service<IX2WorkflowService>().WaitForX2State
             (
                 debtcounsellingcase.DebtCounsellingKey,
                 Workflows.DebtCounselling,
                 WorkflowStates.DebtCounsellingWF.PendCancellation,
                 3
             );
            StageTransitionAssertions.AssertStageTransitionCreated(debtcounsellingcase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_UnderCancellation);

            WorkflowHelper.RemoveDetailType(debtcounsellingcase.AccountKey, debtcounsellingcase.DebtCounsellingKey, DetailType.RemoveCancellation);
            //Wait for case to move back to prev stage
            Service<IX2WorkflowService>().WaitForX2State
             (
                 debtcounsellingcase.DebtCounsellingKey,
                 Workflows.DebtCounselling,
                 WorkflowStates.DebtCounsellingWF.PendProposal,
                 3
             );

            //Assert that debtcounselling case is moved back to previous state
            DebtCounsellingAssertions.AssertDebtCounsellingStatus(DebtCounsellingStatusEnum.Open, debtcounsellingcase.DebtCounsellingKey);
            DebtCounsellingAssertions.AssertX2State(debtcounsellingcase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendProposal);
        }

        /// <summary>
        /// Tests that when Under Cancellation and Cancellation registered detail types are added to an account
        /// that the debtcounselling case at the Pend Cancellation state is archived, the email is created informing that payment is not necessary anymore.
        /// Will also check that it can be cancelled with notification of death.
        /// NOTE: Notification of Death is also being send as INTERNAL mail if the reason exist when cancelling a debtcounselling case through the detail type screens.
        /// </summary>
        [Test]
        public void CancellationRegisteredTest()
        {
            try
            {
                var debtcounsellingcase = CreateCaseAtPendProposalFromEworks();

                //insert the reason
                var roleList = Service<IExternalRoleService>().GetActiveExternalRoleList(debtcounsellingcase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.Client);
                var legalEntityKey = (from r in roleList select r.LegalEntityKey).FirstOrDefault();
                //remove existing
                Service<IReasonService>().RemoveReasonsAgainstGenericKeyByReasonType(legalEntityKey,
                    GenericKeyTypeEnum.LegalEntity_LegalEntityKey, ReasonTypeEnum.DebtCounsellingNotification);
                //add new
                Service<IReasonService>().InsertReason(legalEntityKey, ReasonDescription.NotificationofDeath, ReasonTypeEnum.DebtCounsellingNotification,
                     GenericKeyTypeEnum.LegalEntity_LegalEntityKey);

                WorkflowHelper.CaptureDetailType(debtcounsellingcase.AccountKey, debtcounsellingcase.DebtCounsellingKey, DetailType.UnderCancellation);
                WorkflowHelper.CaptureDetailType(debtcounsellingcase.AccountKey, debtcounsellingcase.DebtCounsellingKey, DetailType.CancellationRegistered);

                //Wait for case to move to ArchiveDebtCounselling
                Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(Common.Constants.ExternalActivities.DebtCounselling.EXTCancellationRegistered, debtcounsellingcase.InstanceID, 1);

                //Assert that debtcounselling case is archived.
                DebtCounsellingAssertions.AssertDebtCounsellingStatus(DebtCounsellingStatusEnum.Cancelled, debtcounsellingcase.DebtCounsellingKey);
                DebtCounsellingAssertions.AssertX2State(debtcounsellingcase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.ArchiveDebtCounselling);
                StageTransitionAssertions.AssertStageTransitionCreated(debtcounsellingcase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_UnderCancellation);

                //Get archived(Cancelled) debtcounselling record, but with registered debtcounsellor on it.
                debtcounsellingcase = Service<IDebtCounsellingService>().GetDebtCounsellingCases
                                            (
                                                debtcounsellingcase.DebtCounsellingKey,
                                                debtcounsellingstatus: DebtCounsellingStatusEnum.Cancelled,
                                                extRoleType: ExternalRoleTypeEnum.DebtCounsellor,
                                                isArchivedCases: true
                                            ).FirstOrDefault();

                //Assert email created
                CorrespondenceAssertions.AssertClientEmailByCorrespondenceTemplate
                                      (
                                          debtcounsellingcase.DebtCounsellingKey,
                                          Common.Enums.CorrespondenceTemplateEnum.MortgageLoanCancelledDontContinuePaying,
                                          DateTime.Now.Date,
                                          debtcounsellingcase.EmailAddress,
                                          debtcounsellingcase.AccountKey
                                      );

                eWorkAssertions.AssertNotEworkCaseExists(debtcounsellingcase.AccountKey.ToString(), EworkStages.AssignAttorney, EworkMaps.LossControl);
            }
            catch
            {
                throw;
            }
        }

        #endregion CancellationTests

        #region Helpers

        /// <summary>
        /// Gets an existing ework case and create a debtcounselling case using the account.
        /// </summary>
        /// <returns></returns>
        private Automation.DataModels.DebtCounselling CreateCaseAtPendProposalFromEworks(out string eWorkBackToStage)
        {
            eWorkBackToStage = string.Empty;
            var eWorkStages = new string[]
                                {
                                    EworkStages.Allocation,                     EworkStages.ArrearCases,
                                    EworkStages.Assign,                         EworkStages.Collections,
                                    EworkStages.DebtCounselling,                EworkStages.DebtCounselling_Arrears,
                                    EworkStages.DebtCounselling_Collections,    EworkStages.DebtCounselling_Estates,
                                    EworkStages.DebtCounselling_Facilitation,   EworkStages.DebtCounselling_Seq,
                                    EworkStages.Recoveries,                     EworkStages.UpForFees
                                };

            var lossControlCase = default(Automation.DataModels.eWorkCase);
            foreach (var stateName in eWorkStages)
            {
                lossControlCase = Service<IEWorkService>().GetEWorkCase(EworkMaps.LossControl, stateName);
                if (lossControlCase != null)
                {
                    eWorkBackToStage = stateName;
                    break;
                }
            }
            Assert.That(lossControlCase != null, "No ework case found in Loss Control");
            var idNumbers = Service<IAccountService>().GetIDNumbersForRoleOnAccount(lossControlCase.Account.AccountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active);
            return WorkflowHelper.CreateCaseAndSendToState(WorkflowStates.DebtCounsellingWF.PendProposal, idNumber: idNumbers[0]).FirstOrDefault();
        }

        private Automation.DataModels.DebtCounselling CreateCaseAtPendProposalFromEworks()
        {
            string eWorkBackToStage;
            return CreateCaseAtPendProposalFromEworks(out eWorkBackToStage);
        }

        #endregion Helpers
    }
}