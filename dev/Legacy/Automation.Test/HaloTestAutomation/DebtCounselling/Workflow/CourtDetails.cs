using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [TestFixture, RequiresSTA]
    public sealed class CourtDetailsTests : DebtCounsellingTests.TestBase<CourtDetailsAdd>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test ensures that both a Hearing Type of Court and Tribunal can be captured against a debt counselling case. Once the action has been completed
        /// it will run assertions to ensure that the expected court consultant user is assigned to the case, the court details have been added to the database
        /// and that the case has not moved states.
        /// </summary>
        [Test, Sequential, Description(@"This test ensures that both a Hearing Type of Court and Tribunal can be captured against a debt counselling case. Once the action
		has been completed it will run assertions to ensure that the expected court consultant user is assigned to the case, the court details have been added to the database
		and that the case has not moved states.")]
        public void CourtDetailsAdd([Values(HearingType.Court, HearingType.Tribunal)]  string hearingType,
                                         [Values(AppearanceType.OrderGranted, AppearanceType.AppealPostponed)] string appearanceType,
                                         [Values("Comment", "")] string comment
                             )
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD,
                Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CourtDetails);
            //we need court details to add
            Automation.DataModels.CourtDetails courtDetails = new Automation.DataModels.CourtDetails
            {
                hearingType = hearingType,
                appearanceType = appearanceType,
                caseNumber = "123/abc/456",
                hearingDate = DateTime.Now.ToString(Formats.DateFormat),
                court = Court.Pinetown,
                comments = comment,
            };
            base.View.AddCourtDetails(courtDetails);
            //the case should be assigned to the next court consultant
            DebtCounsellingAssertions.AssertX2StateByAccountKey(base.TestCase.AccountKey, WorkflowStates.DebtCounsellingWF.ManageProposal);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, adUserName);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName,
                WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, true, true);
            //check that the details have been added.
            DebtCounsellingAssertions.AssertCourtDetailsExist(base.TestCase.DebtCounsellingKey, courtDetails);
        }

        /// <summary>
        /// This test will ensure that that if the Court Details action is performed on multiple cases in a group, then the same Debt Counselling Court Consultant
        /// is assigned to the debt counselling case.
        /// </summary>
        [Test, Description("Performing the court details action on a group of cases should assign the same debt counselling court consultant onto the case.")]
        public void CourtDetailsAddGroupedCases()
        {
            List<Automation.DataModels.Account> accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(false, 2, false);
            foreach (var acc in accounts)
            {
                base.TestCase = Service<IDebtCounsellingService>().GetDebtCounsellingCases(accountkey: acc.AccountKey, debtcounsellingstatus: DebtCounsellingStatusEnum.Open).FirstOrDefault();
                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, "RespondToDebtCounsellor", base.TestCase.DebtCounsellingKey);
                string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
                base.LoadCase(WorkflowStates.DebtCounsellingWF.PendProposal);
                base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CourtDetails);
                var courtDetails = new Automation.DataModels.CourtDetails
                {
                    hearingType = HearingType.Court,
                    appearanceType = AppearanceType.OrderGranted,
                    caseNumber = "123/abc/456",
                    court = Court.Pinetown,
                    comments = "Comment",
                    hearingDate = DateTime.Now.AddDays(7).ToString(Formats.DateFormat)
                };
                base.Browser.Page<CourtDetailsAdd>().AddCourtDetails(courtDetails);
                //the case should be assigned to the next court consultant
                DebtCounsellingAssertions.AssertX2StateByAccountKey(acc.AccountKey, WorkflowStates.DebtCounsellingWF.PendProposal);
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, adUserName);
                WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName,
                    WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, true, true);
                //check that the details have been added.
                DebtCounsellingAssertions.AssertCourtDetailsExist(base.TestCase.DebtCounsellingKey, courtDetails);
            }
        }

        /// <summary>
        /// This test ensures that a case number, court and hearing date are provided before the user can save Court Details with a Hearing Type of COURT
        /// </summary>
        [Test, Description(@"This test ensures that a case number, court and hearing date are provided before the user can save Court Details with a Hearing Type of COURT")]
        public void CourtDetailsValidationTests()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CourtDetails);
            var courtDetails = new Automation.DataModels.CourtDetails
            {
                hearingType = HearingType.Court,
                appearanceType = AppearanceType.OrderGranted,
                caseNumber = "123/abc/456",
                hearingDate = DateTime.Now.ToString(Formats.DateFormat),
                court = Court.Pinetown,
                comments = "Court Details Automation Comment"
            };
            base.View.AddCourtDetailsNoCourt(courtDetails);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Court must be selected.");
            base.View.ClearCourtDetails();
            base.View.AddCourtDetailsNoCaseNumber(courtDetails);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Case Number must be entered.");
            base.View.ClearCourtDetails();
            base.View.AddCourtDetailsNoHearingDate(courtDetails);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Hearing Date must be entered.");
        }

        /// <summary>
        /// The case number should be the same for all of the court details captured against a debt counselling case. This test will add a court detail record
        /// and then try and add a subsequent record with a different case number. A warning should appear which the user should be allowed to ignore and the
        /// record should be added.
        /// </summary>
        [Test, Description(@"The case number should be the same for all of the court details captured against a debt counselling case. This test will add a court detail
		record and then try and add a subsequent record with a different case number. A warning should appear which the user should be allowed to ignore and the
		record should be added.")]
        public void CourtDetailsCheckCaseNumberDoesntChange()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            //remove court details
            base.Service<ICourtDetailsService>().DeleteCourtDetails(base.TestCase.DebtCounsellingKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CourtDetails);
            var courtDetails = new Automation.DataModels.CourtDetails
            {
                hearingType = HearingType.Court,
                appearanceType = AppearanceType.OrderGranted,
                caseNumber = "123/abc/456",
                hearingDate = DateTime.Now.ToString(Formats.DateFormat),
                court = Court.Pinetown,
                comments = "Court Details Automation Comment"
            };
            base.View.AddCourtDetails(courtDetails);
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            //the case should be assigned to the next court consultant
            DebtCounsellingAssertions.AssertX2StateByAccountKey(base.TestCase.AccountKey, WorkflowStates.DebtCounsellingWF.PendProposal);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, adUserName);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, true, true);
            //check that the details have been added.
            DebtCounsellingAssertions.AssertCourtDetailsExist(base.TestCase.DebtCounsellingKey, courtDetails);
            //load the case and add an additional record
            var browser = new TestBrowser(adUserName);
            browser.Page<X2Worklist>().SelectCaseFromWorklist(browser, WorkflowStates.DebtCounsellingWF.PendProposal, base.TestCase.AccountKey);
            browser.ClickAction(WorkflowActivities.DebtCounselling.CourtDetails);
            //change the case number
            courtDetails.caseNumber = "111/abc/222";
            courtDetails.appearanceType = AppearanceType.CourtApplication;
            browser.Page<CourtDetailsAdd>().AddCourtDetails(courtDetails);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Case Number must be the same as on previous Hearing Detail records : 123/abc/456");
            browser.Page<BasePage>().DomainWarningClickYes();
            //check that the details have been added.
            DebtCounsellingAssertions.AssertCourtDetailsExist(base.TestCase.DebtCounsellingKey, courtDetails);
            browser.Dispose();
        }

        /// <summary>
        /// This test ensures that both a Hearing Type of Court and Tribunal can be captured against a debt counselling case. Once the action has been completed
        /// it will run assertions to ensure that the expected court consultant user is assigned to the case, the court details have been added to the database
        /// and that the case has not moved states.
        /// </summary>
        [Test, Sequential, Description(@"This test ensures that both a Hearing Type of Court and Tribunal can be captured against a debt counselling case. Once the action
		has been completed it will run assertions to ensure that the expected court consultant user is assigned to the case, the court details have been added to the database
		and that the case has not moved states.")]
        public void CourtDetailsConsentOrderGrantedDoesNotAssignTheCourtConsultant()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CourtDetails);
            //we need court details to add
            Automation.DataModels.CourtDetails courtDetails = new Automation.DataModels.CourtDetails
            {
                hearingType = HearingType.Court,
                appearanceType = AppearanceType.ConsentOrderGranted,
                caseNumber = "123/abc/456",
                hearingDate = DateTime.Now.ToString(Formats.DateFormat),
                court = Court.Pinetown,
                comments = "TEST",
            };
            base.View.AddCourtDetails(courtDetails);
            //the case should be assigned to the next court consultant
            DebtCounsellingAssertions.AssertX2StateByAccountKey(base.TestCase.AccountKey, WorkflowStates.DebtCounsellingWF.ManageProposal);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, TestUsers.DebtCounsellingConsultant);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, TestUsers.DebtCounsellingConsultant, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
            //check that the details have been added.
            DebtCounsellingAssertions.AssertCourtDetailsExist(base.TestCase.DebtCounsellingKey, courtDetails);
        }

        [Test, Description(@"This test ensures that when a hearing type of 'Court' is selected and the user selects the various Appearance Status's, the 'Next Hearing Date' name changes to 'Date' depending on the business rules provided")]
        public void CourtDetailsCheckDateOrNextHearingDateDependingOnAppearanceStatus()
        {
            //sets the correct state for the test
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CourtDetails);

            //Sets the various Appearance status's
            string Appeal = AppearanceType.Appeal;
            string AppealDeclined = AppearanceType.AppealDeclined;
            string AppealGranted = AppearanceType.AppealGranted;
            string AppealPostponed = AppearanceType.AppealPostponed;
            string ConsentOrderGranted = AppearanceType.ConsentOrderGranted;
            string CourtApplication = AppearanceType.CourtApplication;
            string CourtApplicationPostponed = AppearanceType.CourtApplicationPostponed;
            string OrderGranted = AppearanceType.OrderGranted;

            //Goes through each appearance status and checks that the correct label text is displayed
            base.View.SelectAppearanceStatus(Appeal);
            base.View.AssertLabelText("Next Hearing Date");

            base.View.SelectAppearanceStatus(AppealDeclined);
            base.View.AssertLabelText("Date");

            base.View.SelectAppearanceStatus(AppealGranted);
            base.View.AssertLabelText("Date");

            base.View.SelectAppearanceStatus(AppealPostponed);
            base.View.AssertLabelText("Next Hearing Date");

            base.View.SelectAppearanceStatus(ConsentOrderGranted);
            base.View.AssertLabelText("Date");

            base.View.SelectAppearanceStatus(CourtApplication);
            base.View.AssertLabelText("Next Hearing Date");

            base.View.SelectAppearanceStatus(CourtApplicationPostponed);
            base.View.AssertLabelText("Next Hearing Date");

            base.View.SelectAppearanceStatus(OrderGranted);
            base.View.AssertLabelText("Date");
        }
    }
}