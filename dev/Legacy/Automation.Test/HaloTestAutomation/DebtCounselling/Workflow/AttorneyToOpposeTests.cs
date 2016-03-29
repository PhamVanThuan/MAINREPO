using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class AttorneyToOpposeTests : TestBase<CourtDetailsAdd>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test will perform the attorney to oppose action on a debt counselling case. It will also add the litigation attorney role to
        /// the case prior to performing the action as this role is required.
        /// </summary>
        [Test, Description(@"This test will perform the attorney to oppose action on a debt counselling case. It will also add the litigation attorney role to
        the case prior to performing the action as this role is required.")]
        public void AttorneyToOpposeFromPendProposal()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            AttorneyToOppose();
        }

        /// <summary>
        /// This test will remove any existing attorney role on a debt counselling case before attempting to perform the Attorney to Oppose action.
        /// As there is no litigation attorney assigned to the case the user will receive a validation message indicating a litigation attorney
        /// needs to be selected.
        /// </summary>
        [Test, Description(@"This test will remove any existing attorney role on a debt counselling case before attempting to perform the Attorney to Oppose action.
        As there is no litigation attorney assigned to the case the user will receive a validation message indicating a litigation attorney
        needs to be selected.")]
        public void AttorneyToOpposeNoActiveAttorney()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //remove the attorney role
            Service<IExternalRoleService>().DeleteExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.LitigationAttorney);
            //ensure court details
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, DateTime.Now.AddDays(+5));
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.AttorneytoOppose);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a Litigation Attorney before continuing this action.");
        }

        /// <summary>
        /// A warning should be displayed if a user tries to perform the Attorney to Oppose action when there are no debt counselling contacts added
        /// to the Litigation Attorney
        /// </summary>
        [Test, Description(@"A warning should be displayed if a user tries to perform the Attorney to Oppose action when there are no debt counselling contacts added
        to the Litigation Attorney")]
        public void AttorneyToOpposeNoAttorneyContacts()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            dynamic attorney = AddLitigationAttorneyIfOneDoesNotExist();
            base.Browser.ClickWorkflowLoanNode(Common.Constants.Workflows.DebtCounselling, base.TestCase.AccountKey);
            Service<IExternalRoleService>().DeleteExternalRole(attorney.key, GenericKeyTypeEnum.Attorney_AttorneyKey, ExternalRoleTypeEnum.DebtCounselling);
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, DateTime.Now.AddDays(+5));
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.AttorneytoOppose);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Litigation Attorney does not contain any contacts.");
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void AttorneyToOpposeLatestHearingDetailsHasNoComments()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            int legalEntityKey = WorkflowHelper.SelectLitigationAttorney(base.Browser, base.TestCase.DebtCounsellingKey);
            //we need the attorney key
            int attorneyKey = base.Service<IAttorneyService>().GetAttorneyByLegalEntityKey(legalEntityKey).AttorneyKey;
            //we need to insert a contact
            int externalRoleLegalEntityKey = Service<IExternalRoleService>().InsertExternalRole(attorneyKey, GenericKeyTypeEnum.Attorney_AttorneyKey,
                0, ExternalRoleTypeEnum.DebtCounselling, GeneralStatusEnum.Active);
            base.Browser.ClickWorkflowLoanNode(Common.Constants.Workflows.DebtCounselling, base.TestCase.AccountKey);
            //remove any court details
            base.Service<ICourtDetailsService>().DeleteCourtDetails(base.TestCase.DebtCounsellingKey);
            //needs court details
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, DateTime.Now.AddDays(+5),
                        comment: string.Empty);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.AttorneytoOppose);
            base.Browser.Page<BasePageAssertions>().AssertNotification("The Hearing Detail Comment is Mandatory");
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, DateTime.Now.AddDays(+5));
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, DateTime.Now.AddDays(+5),
                        comment: string.Empty);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.AttorneytoOppose);
            base.Browser.Page<BasePageAssertions>().AssertNotification("The Hearing Detail Comment is Mandatory");
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, DateTime.Now.AddDays(+5));
            AttorneyToOppose();
        }

        private void AttorneyToOppose()
        {
            int legalEntityKey = WorkflowHelper.SelectLitigationAttorney(base.Browser, base.TestCase.DebtCounsellingKey);
            //we need the attorney key
            int attorneyKey = Service<IAttorneyService>().GetAttorneyByLegalEntityKey(legalEntityKey).AttorneyKey;
            //we need to insert a contact
            int externalRoleLegalEntityKey = Service<IExternalRoleService>().InsertExternalRole(attorneyKey, GenericKeyTypeEnum.Attorney_AttorneyKey,
                0, ExternalRoleTypeEnum.DebtCounselling, GeneralStatusEnum.Active);
            base.Browser.ClickWorkflowLoanNode(Common.Constants.Workflows.DebtCounselling, base.TestCase.AccountKey);
            //we need to fetch the user
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            //perform the action
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            //needs court details
            Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, DateTime.Now.AddDays(+5));
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.AttorneytoOppose);
            base.Browser.Page<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>().SelectCorrespondenceRecipient(externalRoleLegalEntityKey);
            //wait for completion
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.PendCourtDecision);
            ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.LitigationAttorney, legalEntityKey);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendCourtDecision);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, adUserName);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName,
                WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, true, true);
            //check the correspondence/dataStor records
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.TestCase.DebtCounsellingKey, CorrespondenceReports.AttorneytoOpposeLetter, CorrespondenceMedium.Post);
            //check dataStor
            CorrespondenceAssertions.AssertImageIndex(base.TestCase.DebtCounsellingKey.ToString(), CorrespondenceReports.AttorneytoOpposeLetter, CorrespondenceMedium.Post,
                base.TestCase.AccountKey, base.TestCase.DebtCounsellingKey);
        }

        private dynamic AddLitigationAttorneyIfOneDoesNotExist()
        {
            var externalRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.LitigationAttorney);

            int attorneyLegalEntityKey = externalRole == null ? WorkflowHelper.SelectLitigationAttorney(base.Browser, base.TestCase.DebtCounsellingKey) : externalRole.LegalEntityKey;

            int attorneyKey = Service<IAttorneyService>().GetAttorneyByLegalEntityKey(attorneyLegalEntityKey).AttorneyKey;

            dynamic attorney = new
            {
                key = Service<IAttorneyService>().GetAttorneyByLegalEntityKey(attorneyLegalEntityKey).AttorneyKey,
                legalEntityKey = attorneyLegalEntityKey,
                //we need contacts
                attorneyContactLegalEntityKey = Service<IExternalRoleService>().InsertExternalRole(attorneyKey, GenericKeyTypeEnum.Attorney_AttorneyKey, 0,
                    ExternalRoleTypeEnum.DebtCounselling, GeneralStatusEnum.Active),
            };
            return attorney;
        }
    }
}