using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.DebtCounselling;
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
    [TestFixture, RequiresSTA]
    public sealed class CounterProposalsTests : DebtCounsellingTests.TestBase<CounterProposalDetailsMaintenanceWorkflow>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        #region tests

        /// <summary>
        /// Test that only one Draft Counter Proposal can exist
        /// 1. Assert error message is displayed on clicking the 'Add' button if a Draft Proposal already exist for the case
        /// 2. Assert error message is displayed selecting a Draft Proposal and clicking the 'Copy to Draft' button if a Draft Proposal already exist for the case
        /// 3. Assert error message is displayed selecting an Active  Proposal and clicking the 'Copy to Draft' button if a Draft Proposal already exist for the case
        /// 4. Assert error message is displayed selecting an Inactive Proposal and clicking the 'Copy to Draft' button if a Draft Proposal already exist for the case
        /// </summary>
        [Test, Description("Test that only one Draft Counter Proposal can exist")]
        public void OnlyOneDraftCounterProposalCanExist()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //Insert counter proposal test data
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 3);
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 3);
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Inactive, 3);

            //Perform tests on  Proposal Summary screen
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Draft proposal already exists - a new proposal cannot be added.");
            //draft
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft,
                ButtonTypeEnum.CopytoDraft);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot copy a draft proposal - it should be updated instead.");
            //active
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Active,
                ButtonTypeEnum.CopytoDraft);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Draft proposal already exists - cannot copy to a new draft.");
            //inactive
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Inactive,
                ButtonTypeEnum.CopytoDraft);
        }

        /// <summary>
        /// Test that the HOC and Life fields are mandatory
        /// 1. Assert that an error message is displayed  when both HOC and Life fields are set to 'Select' and the 'Add' button is clicked
        /// 2. Assert that an error message is displayed  when the HOC field is set and the Life field is not
        /// 3. Assert that an error message is displayed  when the Life field is set and the HOC field is not
        /// 3. Assert that a Proposal record is created when HOC and Life fields are set
        /// </summary>
        [Test, Description("Test that the HOC and Life fields are mandatory")]
        public void HOCAndLifeInclusiveExclusiveAreMandatoryFields()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            //we need to remove any draft proposals
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft))
                base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft, ButtonTypeEnum.Delete);

            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            //setup a proposal
            var counterProposal = new Automation.DataModels.ProposalItem()
            {
                HOC = string.Empty,
                Life = string.Empty,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                InterestRate = -1,
                EndPeriod = -1,
                AnnualEscalation = -1,
                InstalmentPercent = -1,
                Note = "Test"
            };
            base.View.ClickButton(ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.",
                "Must select whether Life is Inclusive or Exclusive.");
            base.View.ClickButton(ButtonTypeEnum.Done);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.",
                "Must select whether Life is Inclusive or Exclusive.");
            counterProposal.HOC = "Inclusive";
            base.View.AddCounterProposalEntry(counterProposal, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether Life is Inclusive or Exclusive.");
            base.View.ClickButton(ButtonTypeEnum.Done);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether Life is Inclusive or Exclusive.");

            counterProposal.HOC = "- Please Select -";
            counterProposal.Life = "Exclusive";
            base.View.AddCounterProposalEntry(counterProposal, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.");
            base.View.ClickButton(ButtonTypeEnum.Done);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.");

            counterProposal.HOC = "Exclusive";
            counterProposal.Life = "Inclusive";
            base.View.AddCounterProposalEntry(counterProposal, ButtonTypeEnum.Add, true);
            base.View.ClickButton(ButtonTypeEnum.Done);
            DebtCounsellingAssertions.AssertProposalExists(base.TestCase.DebtCounsellingKey, (int)ProposalTypeEnum.CounterProposal, (int)ProposalStatusEnum.Draft, counterProposal.HOC,
                counterProposal.Life, DateTime.Today);
        }

        /// <summary>
        /// Test the mandatory fields on the CounterProposalDetailsMaintenanceWorkflow screen
        /// 1. Assert that Start Date and End Date are mandatory fields
        /// 2. Assert that Counter Proposal Notes - Description is a mandatory field
        /// 3. Assert that Interest Rate is a mandatory field
        /// 4. Assert that Instalment % is a voluntary field
        /// 4.1 Assert that a Warning is displayed that allows the user to continue
        /// 4.2 Assert that on deciding to continue a ProposalItem record is created
        /// 5. Assert that Annual Escalation % is a voluntary field
        /// 5.1 Assert that a ProposalItem Record has been created
        /// </summary>
        [Test, Description("Test the mandatory fields on the CounterProposalDetailsMaintenanceWorkflow screen")]
        public void CounterProposalMandatoryFieldValidation()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //remove the proposal
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);

            var counterProposalDetails = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                Note = "Test",
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                InterestRate = 7.30,
                InstalmentPercent = 50.00,
                AnnualEscalation = 10.00,
                EndPeriod = -1,
                MonthlyServiceFee = true
            };
            //Check that Start Date and End Date are mandatory fields
            base.View.AddCounterProposalEntry(counterProposalDetails,
                ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Start Date is required.", "End Date is required.");

            #region Note

            //Check that Counter Proposal Notes - Description is a mandatory field
            counterProposalDetails.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            counterProposalDetails.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            counterProposalDetails.Note = "";
            counterProposalDetails.EndPeriod = 1;
            base.View.AddCounterProposalEntry(counterProposalDetails,
                ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A counter proposal note must be entered");
            //this field should be additionally validated on the click of the Done button
            base.View.ClickButton(ButtonTypeEnum.Done);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A counter proposal note must be entered");

            #endregion Note

            #region InterestRate

            //Check that Interest Rate is a mandatory field
            counterProposalDetails.Note = "Test";
            counterProposalDetails.InterestRate = 0.00;
            base.View.AddCounterProposalEntry(counterProposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Positive Interest Rate is required.");

            #endregion InterestRate

            #region AnnnualEscalation

            //Check that Annual Escalation % is a voluntary field
            counterProposalDetails.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(2).Month, 1);
            counterProposalDetails.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(2).Month, DateTime.DaysInMonth(DateTime.Today.Year,
                DateTime.Today.AddMonths(2).Month));
            counterProposalDetails.AnnualEscalation = 0.00;
            counterProposalDetails.InterestRate = 7.70;
            counterProposalDetails.EndPeriod = 3;
            base.View.AddCounterProposalEntry(counterProposalDetails, ButtonTypeEnum.Add, true);

            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft, counterProposalDetails);

            #endregion AnnnualEscalation
        }

        /// <summary>
        /// Test that the ProposalItem 'End Date' cannot be before 'Start Date'
        /// 1. Assert that an error message is displayed if the 'End Date' is before the 'Start Date'
        /// 3. Assert that a ProposalItem is successfully created when the 'End Date' equals the 'Start Date'
        /// 2. Assert that a ProposalItem is successfully created when the 'End Date' is after the 'Start Date'
        /// </summary>
        [Test, Description("Test that the ProposalItem 'End Date' cannot be before 'Start Date'")]
        public void CounterProposalItemEndDateCannotBeBeforeStartDate()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            //remove proposals
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            var counterProposalDetails = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                Note = "Test Note",
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 2),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                InterestRate = 7.30,
                InstalmentPercent = 50.00,
                AnnualEscalation = 10.00,
                EndPeriod = 0,
                MonthlyServiceFee = true
            };
            base.View.AddCounterProposalEntry(counterProposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Start Date cannot be greater than End Date.");
        }

        /// <summary>
        /// This test will test the counter proposal capture by using the date methods only. It will enter the end dates and then fire the onChange event for the End Date field.
        /// This should calculate the ending periods. The start/end periods are then sent to the GUI assertion.
        /// </summary>
        [Test, Description(@"This test will test the proposal capture by using the date methods only. It will enter the end dates and then fire the onChange event for the End Date field.
        This should calculate the ending periods. The start/end periods are then sent to the GUI assertion.")]
        public void CounterProposalCaptureUsingDates()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            //we need to remove any draft proposals
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft))
            {
                base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal,
                    DebtCounsellingProposalStatus.Draft,
                    ButtonTypeEnum.Delete);
            }
            //Add a new draft
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);

            //we need a counter proposal
            var counterProposal = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, DateTime.DaysInMonth(DateTime.Today.Year,
                    DateTime.Today.AddMonths(1).Month)),
                InterestRate = 7.30,
                EndPeriod = 1,
                Note = "Test Note",
                InstalmentPercent = 50.00,
                AnnualEscalation = 6.00,
                MonthlyServiceFee = true
            };
            //add line 1
            base.View.AddCounterProposalEntry(counterProposal, ButtonTypeEnum.Add, false);
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft, counterProposal);
            //add line 2
            counterProposal.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(2).Month, 1);
            var endDate = DateTime.Today.AddMonths(5);
            counterProposal.EndDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
            counterProposal.EndPeriod = 5;
            base.View.AddCounterProposalEntry(counterProposal, ButtonTypeEnum.Add, false);
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft, counterProposal);
        }

        /// <summary>
        /// This test ensures that a counter proposal line item can be captured with a zero instalment amount
        /// </summary>
        [Test, Description("This test ensures that a counter proposal line item can be captured with a zero instalment amount")]
        public void CounterProposalCaptureZeroAmount()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //remove the proposal
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);

            var counterProposal = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                Note = "Test",
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)),
                InterestRate = 7.30,
                InstalmentPercent = 0.00,
                AnnualEscalation = 0.00,
                EndPeriod = 1,
                MonthlyServiceFee = true
            };

            #region InstalmentPercentage

            base.View.AddCounterProposalEntry(counterProposal, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Are you sure you want to save a zero amount?");
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft, counterProposal);

            #endregion InstalmentPercentage
        }

        /// <summary>
        /// This test ensures that a counter proposal cannot be set to active before adding reasons
        /// </summary>
        [Test, Description(@"This test ensures that a counter proposal cannot be set to active before adding reasons")]
        public void CounterProposalReasonRequiredBeforeSettingActive()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need a new counter proposal
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft))
                Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            //insert counter proposal
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 5);
            //try and set the counter proposal to inactive
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft,
                ButtonTypeEnum.SetActive);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Reason must exist for the Counter Proposal.");
        }

        /// <summary>
        /// This test will capture counter proposal reasons against a counter proposal before setting it to active.
        /// </summary>
        [Test, Description("This test will capture counter proposal reasons against a counter proposal before setting it to active.")]
        public void CaptureCounterProposalReasons()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need a new counter proposal
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            //insert counter proposal
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 5);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft,
                ButtonTypeEnum.Reasons);

            int genericKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, ProposalAcceptedEnum.False,
                ProposalTypeEnum.CounterProposal);
            //add the reason
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.CounterProposals);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.CounterProposals, genericKey, GenericKeyTypeEnum.Proposal_ProposalKey);
            //set it to active
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft,
                ButtonTypeEnum.SetActive);
            //check the proposal is active
            DebtCounsellingAssertions.AssertProposalExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Active, ProposalAcceptedEnum.False);
        }

        /// <summary>
        /// Ensures that you cannot set the Counter Proposal active without providing a counter proposal reason
        /// </summary>
        [Test, Description(@"Ensures that you cannot set the Counter Proposal active without providing a counter proposal reason")]
        public void CannotSetCounterProposalActiveWithoutReason()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need a new counter proposal
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            //insert counter proposal
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 5);
            //try and set the counter proposal to inactive
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            int genericKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, ProposalAcceptedEnum.False,
                ProposalTypeEnum.CounterProposal);
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft,
                ButtonTypeEnum.SetActive);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Reason must exist for the Counter Proposal.");
        }

        /// <summary>
        /// Deleting a draft proposal should remove any reasons linked to the proposal
        /// </summary>
        [Test, Description("Deleting a draft proposal should remove any reasons linked to the proposal")]
        public void DeleteDraftProposalRemovesRelatedReasons()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need a new counter proposal
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            //insert counter proposal
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 5);
            //try and set the counter proposal to inactive
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageCounterProposals();
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft,
                ButtonTypeEnum.Reasons);
            int genericKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, ProposalAcceptedEnum.False,
                ProposalTypeEnum.CounterProposal);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.CounterProposals);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.CounterProposals, genericKey, GenericKeyTypeEnum.Proposal_ProposalKey);
            //delete it
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.CounterProposal, DebtCounsellingProposalStatus.Draft,
                ButtonTypeEnum.Delete);
            //check it no longer exists
            DebtCounsellingAssertions.AssertProposalDoesNotExist(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft, ProposalAcceptedEnum.False);
            //check that the reasons have been removed.
            ReasonAssertions.AssertReason(selectedReason, ReasonType.CounterProposals, genericKey, GenericKeyTypeEnum.Proposal_ProposalKey, reasonExists: false);
        }

        /// <summary>
        /// Create counter proposal draft copy from all types of proposals.
        /// </summary>
        [Test, Description("Create counter proposal draft copy from all types of proposals.")]
        public void CounterProposalDraftsFromProposalsTest()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //Insert Proposal for each proposal status.
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 3, TestUsers.DebtCounsellingConsultant, 1, 1);
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 3, TestUsers.DebtCounsellingConsultant, 1, 1);
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Inactive, 3, TestUsers.DebtCounsellingConsultant, 1, 1);
            //Navigate to Manage Proposals
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageProposal();
            //Test rule that prevent from having more than one counter proposal draft
            //Test that a counter proposal draft can be created off proposals of all statusses
            this.CreateCounterProposalDraftsFromProposals(base.TestCase.DebtCounsellingKey, DebtCounsellingProposalStatus.Active);
            this.CreateCounterProposalDraftsFromProposals(base.TestCase.DebtCounsellingKey, DebtCounsellingProposalStatus.Draft);
            this.DeleteCounterProposalDraft(base.TestCase.DebtCounsellingKey, DebtCounsellingProposalStatus.Draft);
            this.CreateCounterProposalDraftsFromProposals(base.TestCase.DebtCounsellingKey, DebtCounsellingProposalStatus.Draft);
            this.DeleteCounterProposalDraft(base.TestCase.DebtCounsellingKey, DebtCounsellingProposalStatus.Draft);
            this.CreateCounterProposalDraftsFromProposals(base.TestCase.DebtCounsellingKey, DebtCounsellingProposalStatus.Inactive);
            this.DeleteCounterProposalDraft(base.TestCase.DebtCounsellingKey, DebtCounsellingProposalStatus.Draft);
        }

        #endregion tests

        #region Helper

        private void CreateCounterProposalDraftsFromProposals(int debtcounsellingkey, string proposalStatus)
        {
            var propStatusEnum = (ProposalStatusEnum)Enum.Parse(typeof(ProposalStatusEnum), proposalStatus);
            //Create a Counter Proposal Draft from an active proposal.
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal, proposalStatus, ButtonTypeEnum.None);
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickCreateCounterProposal();
            DebtCounsellingAssertions.AssertProposalExists(debtcounsellingkey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft, ProposalAcceptedEnum.False);
        }

        private void DeleteCounterProposalDraft(int debtcounsellingkey, string proposalStatus)
        {
            var propStatusEnum = (ProposalStatusEnum)Enum.Parse(typeof(ProposalStatusEnum), proposalStatus);
            Service<IProposalService>().DeleteProposal(debtcounsellingkey, ProposalTypeEnum.CounterProposal, propStatusEnum);
        }

        #endregion Helper
    }
}