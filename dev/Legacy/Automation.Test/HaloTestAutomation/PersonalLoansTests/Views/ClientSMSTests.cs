using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.PersonalLoans;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using WatiN.Core;
using Navigation = BuildingBlocks.Navigation;

namespace PersonalLoansTests.Views
{
    [RequiresSTA]
    public sealed class ClientSms : PersonalLoansTests.PersonalLoansWorkflowTestBase<ClientCommunicationPersonalLoansSMS>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.PersonalLoanConsultant2);
        }

        [Test]
        public void SendSms()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            base.Browser.Navigate<Navigation.CorrespondenceNode>().Correspondence();
            base.Browser.Navigate<Navigation.CorrespondenceNode>().SendSMS();

            //Assert Clients is in the grid
            //var recipients = base.View.GetRecipientNamesByExternalRole(ExternalRoleTypeEnum.Client);
            //if (recipients.Count > 0)
            //    DebtCounsellingAssertions.AssertClientCommunicationRecipientsByExternalRoleType(base.TestCase.DebtCounsellingKey, recipients, ExternalRoleTypeEnum.Client);
            ////Assert LegalEntitie Main Applicants Roles on Account
            //recipients = base.View.GetRecipientNamesByLegalEntityRole(RoleTypeEnum.MainApplicant);
            //if (recipients.Count > 0)
            //    DebtCounsellingAssertions.AssertClientCommunicationRecipientsByLegalEntityRoleType(base.TestCase.DebtCounsellingKey, recipients, RoleTypeEnum.MainApplicant);
            ////Assert LegalEntitie Suretor Roles on Account
            //recipients = base.View.GetRecipientNamesByLegalEntityRole(RoleTypeEnum.Suretor);
            //if (recipients.Count > 0)
            //    DebtCounsellingAssertions.AssertClientCommunicationRecipientsByLegalEntityRoleType(base.TestCase.DebtCounsellingKey, recipients, RoleTypeEnum.Suretor);

            //Test Validation Messages
            base.View.Send();
            List<string> messages = base.Browser.Page<BasePage>().GetValidationMessages();

            //create identical assertion for PersonalLoans
            //DebtCounsellingAssertions.AssertClientCommunicationValidationMessages(messages.ToArray(), isSMS: true);
            base.View.CheckRecipients();
            base.View.Populate("Free Format", "Testing Free Format SMS");
            base.View.Send();
            //Check that there are no validation messages
            messages = base.Browser.Page<BasePage>().GetValidationMessages();
            Assert.That(messages.Count == 0, "There were validation messages");

            //Assert ClientEmail
            //DebtCounsellingAssertions.AssertClientCommunication(base.TestCase.DebtCounsellingKey, isSMS: true);
        }
    }
}