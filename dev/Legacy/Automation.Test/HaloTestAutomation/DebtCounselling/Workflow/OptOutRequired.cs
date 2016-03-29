using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    /// <summary>
    /// Contains tests for the sending and receiving of the opt out legal agreements for debt counselling.
    /// </summary>
    [RequiresSTA]
    public class OptOutRequired : DebtCounsellingTests.TestBase<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.StartTest(WorkflowStates.DebtCounsellingWF.SendLoanAgreements, TestUsers.DebtCounsellingConsultant, product: ProductEnum.Edge, searchForCase: false);
        }

        /// <summary>
        /// Completes the workflow action to send the new Legal Agreements to the Debt Counsellor, ensuring that the document is sent to the Correspondence table
        /// and that the document is stored correctly in DataStor
        /// </summary>
        [Test, Description(@"Completes the workflow action to send the new Legal Agreements to the Debt Counsellor, ensuring that the document is sent to the Correspondence
        table and that the document is stored correctly in DataStor")]
        public void SendOptOutDocuments()
        {
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendDocuments);
            int legalEntityKey = Service<IDebtCounsellingService>().GetDCTestCaseDebtCounsellorCorrespondenceDetails(null, base.TestCase.AccountKey);
            base.View.SelectCorrespondenceRecipient(legalEntityKey);
            //check transition exists
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_SendOptOutLegalDocuments);
            //check the correspondence/dataStor records
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.TestCase.DebtCounsellingKey, CorrespondenceReports.LegalAgreementStandardVariable, CorrespondenceMedium.Post);
            //check dataStor
            CorrespondenceAssertions.AssertImageIndex(base.TestCase.DebtCounsellingKey.ToString(), CorrespondenceReports.LegalAgreementStandardVariable, CorrespondenceMedium.Post,
                base.TestCase.AccountKey, base.TestCase.DebtCounsellingKey);
        }

        /// <summary>
        /// Performs the action to confirm that the new legal agreements have been received. We then run the same set of assertions ensuring that the debt counselling opt in
        /// has been performed correctly.
        /// </summary>
        [Test, Description(@"Performs the action to confirm that the new legal agreements have been received. We then run the same set of assertions ensuring that the debt
        counselling opt in has been performed correctly.")]
        public void OptOutDocumentsReceived()
        {
            var tranDate = DateTime.Now;
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.True,
                ProposalTypeEnum.Proposal);
            //check for the updated remaining term
            DateTime proposalEndDate = Service<IProposalService>().GetProposalEndDate(proposalKey);
            int remainingTerm = Service<IDebtCounsellingService>().GetDebtCounsellingRemainingTerm(base.TestCase.DebtCounsellingKey, proposalEndDate);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SignedDocumentsReceived);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.AcceptedProposal);
            DebtCounsellingAssertions.AssertProposalAcceptance(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, proposalKey, remainingTerm);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_OptOutLegalDocumentsReceived);
        }
    }
}