using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;
using System.Linq;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class SubmitApplicationSMSTests : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
            base.Browser.Navigate<NavigationHelper>().Task();
            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
        }

        [Test]
        public void when_submitting_application_should_send_sms_with_assigned_consultant_cellphone_number()
        {
            string branchConsultantCellphoneNumber = "0795242320";
            PerformTest(branchConsultantCellphoneNumber, branchConsultantCellphoneNumber);
        }

        [Test]
        public void when_submitting_application_should_send_sms_with_assigned_consultant_name_and_helpdesk_number_when_consultant_has_no_cellphone_number()
        {
            string helpDeskContactNumber = "0861 888 777";
            PerformTest(string.Empty, helpDeskContactNumber);
        }

        [Test]
        public void when_submitting_application_should_send_sms_with_application_number()
        {
            string branchConsultantCellphoneNumber = "0795242320";
            PerformTest(branchConsultantCellphoneNumber, branchConsultantCellphoneNumber);
        }

        private void PerformTest(string branchConsultantCellphoneNumber, string expectedContactNumberForSMS)
        {
            base.TestCase = new Automation.DataModels.OriginationTestCase();
            base.TestCase.OfferKey = Service<IX2WorkflowService>()
                .GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.ApplicationCaptureWF.ApplicationCapture, TestUsers.BranchConsultant, 7, 8, 9)
                .Rows(0).Column("offerkey")
                .GetValueAs<int>();
            Service<IApplicationService>().CleanupNewBusinessOffer(base.TestCase.OfferKey);
            var branchConsultantOfferRole = Service<IApplicationService>().GetActiveOfferRolesByOfferKey(base.TestCase.OfferKey, Common.Enums.OfferRoleTypeGroupEnum.Operator)
                                                            .Where(x => x.OfferRoleTypeKey == Common.Enums.OfferRoleTypeEnum.BranchConsultantD
                                                                  && x.GeneralStatusKey == Common.Enums.GeneralStatusEnum.Active).FirstOrDefault();
            var applicantRoles = ServiceLocator.Instance.GetService<IApplicationService>()
                .GetActiveOfferRolesByOfferKey(base.TestCase.OfferKey, Common.Enums.OfferRoleTypeGroupEnum.Client)
                .Where(x => x.OfferRoleTypeKey == Common.Enums.OfferRoleTypeEnum.LeadMainApplicant && x.GeneralStatusKey == Common.Enums.GeneralStatusEnum.Active);

            Service<ILegalEntityService>().UpdateCellphone(branchConsultantOfferRole.LegalEntityKey, branchConsultantCellphoneNumber);
            foreach (var mainAppRole in applicantRoles)
                Service<ILegalEntityService>().UpdateCellphone(mainAppRole.LegalEntityKey, String.Format("079{0}111222", randomizer.Next(1, 10)));

            base.Browser.Page<WorkflowSuperSearch>().WorkflowSearch(base.TestCase.OfferKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            base.Browser.Page<WorkflowYesNo>().ClickYes();
            base.Browser.Page<WorkflowYesNo>().ContinueWithWarnings(true);

            var le = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: branchConsultantOfferRole.LegalEntityKey);
            var legalName = le.FirstNames.Replace(@"\\", @"\");
            var sms = FormatSMS(legalName, expectedContactNumberForSMS, base.TestCase.OfferKey);

            ClientSMSAssertions.AssertSMSSentToClients(sms, base.TestCase.OfferKey);
        }

        private string FormatSMS(string consultantName, string consultantNumber, int accountKey)
        {
            return String.Format("Thank you for choosing SA Home Loans. Your consultant is {0}, contact number {1}. Your reference number is {2}", consultantName, consultantNumber, accountKey);
        }
    }
}