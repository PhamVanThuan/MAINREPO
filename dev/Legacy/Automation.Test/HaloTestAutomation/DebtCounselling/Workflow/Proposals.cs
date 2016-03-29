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
    public sealed class ProposalTests : DebtCounsellingTests.TestBase<ProposalDetailsMaintenanceWorkflow>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        #region Tests

        /// <summary>
        /// Test that only one Draft Proposal can exist
        /// 1. Assert error message is displayed on clicking the 'Add' button if a Draft Proposal already exist for the case
        /// 2. Assert error message is displayed selecting a Draft Proposal and clicking the 'Copy to Draft' button if a Draft Proposal already exist for the case
        /// 3. Assert error message is displayed selecting an Active  Proposal and clicking the 'Copy to Draft' button if a Draft Proposal already exist for the case
        /// 4. Assert error message is displayed selecting an Inactive Proposal and clicking the 'Copy to Draft' button if a Draft Proposal already exist for the case
        /// </summary>
        [Test, Description("This test will run the validation rules to ensure that only a single draft proposal can exist.")]
        public void OnlyOneDraftProposalCanExist()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //Insert proposal test data
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 3, TestUsers.DebtCounsellingConsultant, 1, 1);
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 3, TestUsers.DebtCounsellingConsultant, 1, 1);
            Service<IProposalService>().InsertProposal(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Inactive, 3, TestUsers.DebtCounsellingConsultant, 1, 1);

            //Perform tests on  Proposal Summary screen
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageProposal();
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Draft proposal already exists - a new proposal cannot be added.");

            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal,
                DebtCounsellingProposalStatus.Draft, ButtonTypeEnum.CopytoDraft);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot copy a draft proposal - it should be updated instead.");

            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal,
                DebtCounsellingProposalStatus.Active, ButtonTypeEnum.CopytoDraft);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Draft proposal already exists - cannot copy to a new draft.");

            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal,
                DebtCounsellingProposalStatus.Inactive, ButtonTypeEnum.CopytoDraft);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Draft proposal already exists - cannot copy to a new draft.");
        }

        /// Test that the HOC and Life fields are mandatory
        /// 1. Assert that an error message is displayed  when both HOC and Life fields are set to '--Select--' and the 'Add' button is clicked
        /// 2. Assert that an error message is displayed  when the HOC field is set and the Life field is not
        /// 3. Assert that an error message is displayed  when the Life field is set and the HOC field is not
        /// 3. Assert that a Proposal record is created when HOC and Life fields are set
        [Test, Description("Test that the HOC and Life fields are mandatory")]
        public void HOCAndLifeInclusiveExclusiveAreMandatoryFields()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageProposal();
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft))
                base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft,
                    ButtonTypeEnum.Delete);
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            //we need to set up the proposal Details
            var proposalDetails = new Automation.DataModels.ProposalItem()
            {
                HOC = string.Empty,
                Life = string.Empty,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                MarketRate = null,
                InterestRate = -1,
                Amount = -1,
                AdditionalAmount = -1,
                EndPeriod = -1
            };
            base.View.ClickButton(ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.", "Must select whether Life is Inclusive or Exclusive.");
            base.View.ClickButton(ButtonTypeEnum.Done);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.", "Must select whether Life is Inclusive or Exclusive.");

            proposalDetails.HOC = "Inclusive";
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether Life is Inclusive or Exclusive.");
            base.View.ClickButton(ButtonTypeEnum.Done);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether Life is Inclusive or Exclusive.");

            proposalDetails.HOC = "- Please Select -";
            proposalDetails.Life = "Exclusive";
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.");
            base.View.ClickButton(ButtonTypeEnum.Done);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select whether HOC is Inclusive or Exclusive.");

            proposalDetails.HOC = "Exclusive";
            proposalDetails.Life = "Inclusive";
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, false);
            base.View.ClickButton(ButtonTypeEnum.Done);
            DebtCounsellingAssertions.AssertProposalExists(base.TestCase.AccountKey, (int)ProposalTypeEnum.Proposal, (int)ProposalStatusEnum.Draft, proposalDetails.HOC, proposalDetails.Life,
                DateTime.Today);
        }

        /// <summary>
        /// Test the mandatory fields on the ProposalDetailsMaintenanceWorkflow screen
        /// 1. Assert that Start Date and End Date are mandatory fields
        /// 2. Assert that Market Rate is a mandatory field
        /// 3. Assert that Interest Rate is a mandatory field
        /// 4. Assert that Amount is a voluntary field
        /// 4.1 Assert that a Warning is displayed that allows the user to continue
        /// 4.2 Assert that on deciding to continue a ProposalItem record is created
        /// 5. Assert that Additional Amount is a voluntary field
        /// 5.1 Assert that a ProposalItem Record has been created
        /// </summary>
        [Test, Description("Test the mandatory fields on the ProposalDetailsMaintenanceWorkflow screen")]
        public void CheckProposalMandatoryFields()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //we need to remove any draft proposals
            //WorkflowHelper.DeleteProposal(debtCounsellingKey, ProposalType.Proposal, ProposalStatus.Draft);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageProposal();
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft))
                base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft,
                    ButtonTypeEnum.Delete);
            //Add a new draft
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            //we need a proposal
            var proposalDetails = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                MarketRate = MarketRate.Fixed,
                InterestRate = 10,
                Amount = 1000,
                AdditionalAmount = 0,
                EndPeriod = 1,
                MonthlyServiceFee = true
            };

            //Check that Start Date and End Date are mandatory fields
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Start Date is required.", "End Date is required.");

            //Check that Market Rate is a mandatory field
            proposalDetails.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            proposalDetails.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            proposalDetails.MarketRate = "- Please Select -";
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select a Market Rate.");

            //Check that Interest Rate is a mandatory field
            proposalDetails.MarketRate = MarketRate.Fixed;
            proposalDetails.InterestRate = 0;
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Positive Interest Rate is required.");

            //Check that Amount is a voluntary field
            proposalDetails.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1);
            proposalDetails.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, DateTime.DaysInMonth(DateTime.Today.Year,
                DateTime.Today.AddMonths(1).Month));
            proposalDetails.InterestRate = 10;
            proposalDetails.Amount = 0;
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Are you sure you want to save a zero amount?");
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active,
                proposalDetails);

            //Check that Additional Amount is a voluntary field
            proposalDetails.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(2).Month, 1);
            proposalDetails.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(2).Month, DateTime.DaysInMonth(DateTime.Today.Year,
                DateTime.Today.AddMonths(2).Month));
            proposalDetails.Amount = 1000;
            proposalDetails.EndPeriod = 2;
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active,
                proposalDetails);
        }

        /// <summary>
        /// Boundary analysis of Max Fixed Rate allowed
        /// 1. Assert that an error message is dispalayed when the Fixed Rate exceeds 99.99%
        /// 2. Assert that a ProposalItem can be created with a Fixed Rate of 99.99% or less
        /// </summary>
        [Test, Description("Boundary analysis of Max Fixed Rate allowed")]
        public void VerifyProposalItemMaxFixedRateAllowed()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need to remove any draft proposals
            //WorkflowHelper.DeleteProposal(debtCounsellingKey, ProposalType.Proposal, ProposalStatus.Draft);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageProposal();
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft))
                base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft,
                    ButtonTypeEnum.Delete);
            //Add a new draft
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            //setup a proposal
            var proposalDetails = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)),
                MarketRate = MarketRate.Fixed,
                InterestRate = 100,
                Amount = 1500,
                AdditionalAmount = 0,
                EndPeriod = 1,
                MonthlyServiceFee = true
            };

            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Interest Rate exceeds the maximum expected of 99.99%");
            proposalDetails.InterestRate = 99.99;
            base.Browser.Page<ProposalDetailsMaintenanceWorkflow>().AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active,
                proposalDetails);
        }

        /// <summary>
        /// Test that the ProposalItem 'End Date' cannot be before 'Start Date'
        /// 1. Assert that an error message is displayed if the 'End Date' is before the 'Start Date'
        /// </summary>
        [Test, Description("Test that the ProposalItem 'End Date' cannot be before 'Start Date'")]
        public void ProposalItemEndDateCannotBeBeforeStartDate()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need to remove any draft proposals
            //WorkflowHelper.DeleteProposal(debtCounsellingKey, ProposalType.Proposal, ProposalStatus.Draft);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageProposal();
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft))
                base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft,
                    ButtonTypeEnum.Delete);
            //Add a new draft
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endDate = startDate.AddDays(-1);
            //setup proposal with End Date 1 day before Start Date
            var proposalDetails = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                StartDate = startDate, //new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                EndDate = endDate, //new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month)),
                MarketRate = MarketRate.Fixed,
                InterestRate = 8,
                Amount = 1000,
                AdditionalAmount = -0,
                EndPeriod = 0,
                MonthlyServiceFee = false
            };

            //add the entry
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Start Date cannot be greater than End Date.");
            //Set End Date after Start Date
            proposalDetails.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            proposalDetails.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 2);
            proposalDetails.EndPeriod = 1;
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, true);
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active,
                proposalDetails);
        }

        /// <summary>
        /// This test will test the proposal capture by using the date methods only. It will enter the end dates and then fire the onChange event for the End Date field.
        /// This should calculate the ending periods. The start/end periods are then sent to the GUI assertion.
        /// </summary>
        [Test, Description(@"This test will test the proposal capture by using the date methods only. It will enter the end dates and then fire the onChange event for the End Date field.
        This should calculate the ending periods. The start/end periods are then sent to the GUI assertion.")]
        public void ProposalCaptureUsingDates()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<ProposalsNode>().Proposals();
            base.Browser.Navigate<ProposalsNode>().ManageProposal();
            //we need to remove any draft proposals
            if (base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft))
                base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().SelectProposal(DebtCounsellingProposalType.Proposal, DebtCounsellingProposalStatus.Draft,
                    ButtonTypeEnum.Delete);
            //Add a new draft
            base.Browser.Page<DebtCounsellingProposalSummaryWorkflow>().ClickButton(ButtonTypeEnum.Add);
            //we need a proposal
            var proposalDetails = new Automation.DataModels.ProposalItem()
            {
                HOC = "Exclusive",
                Life = "Inclusive",
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, DateTime.DaysInMonth(DateTime.Today.Year,
                    DateTime.Today.AddMonths(1).Month)),
                MarketRate = MarketRate.Fixed,
                InterestRate = 10,
                Amount = 1000,
                AdditionalAmount = 0,
                EndPeriod = 1,
                MonthlyServiceFee = true
            };
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, false);
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active,
                proposalDetails);
            proposalDetails.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(2).Month, 1); ;
            var endDate = DateTime.Today.AddMonths(5);
            proposalDetails.EndDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
            proposalDetails.EndPeriod = 5;
            base.View.AddProposalEntry(proposalDetails, ButtonTypeEnum.Add, false);
            DebtCounsellingAssertions.AssertProposalItemExists(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active,
                proposalDetails);
        }

        #endregion Tests
    }
}