using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests.Views
{
    /// <summary>
    /// As a Debt Counseling Consultant, I want to be able to send a sms or an email to a legal entity who is under debt review so that we have alternative options of communication. Tests:
    /// </summary>
    [RequiresSTA]
    public sealed class ClientEmail : DebtCounsellingTests.TestBase<ClientCommunicationDebtCounsellingEmail>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        ///1.   The available recipients must reflect the various departments and contact person(s) for the NCR, Debt Counselor and all the legal entities playing a role linked to this case.
        ///2.   Capture the free format email subject and message to be sent. The “From” email address must be defaulted to the "log in" users email address.
        ///3.   The standard signature should be attached to the end of the email, the signature linked to the "log-in" user who is sending the email.
        ///4.   There should be no restriction on number of characters used for an email.
        ///5.   Once the email has been sent, the history of it must be inserted into the correspondence history. i.e. (Free format email -then the actual email message) was sent to (email address> on (Current Date)
        ///6.   The user must have the option to send the same email to multiple legal entities therefore the screen should remain open until the user presses the “Complete” button. (The email text must not automatically clear)
        ///7.   Sending an email will not move the case from the current state.
        /// </summary>
        [Test]
        public void SendEmail()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<Navigation.CorrespondenceNode>().Correspondence();
            base.Browser.Navigate<Navigation.CorrespondenceNode>().SendEmail();
            //Assert Clients is in the grid
            var recipients = base.View.GetRecipientNamesByExternalRole(ExternalRoleTypeEnum.Client);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByExternalRoleType(base.TestCase.DebtCounsellingKey, recipients, ExternalRoleTypeEnum.Client);
            //Assert DebtCounsellors is in the grid
            recipients = base.View.GetRecipientNamesByExternalRole(ExternalRoleTypeEnum.DebtCounsellor);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByExternalRoleType(base.TestCase.DebtCounsellingKey, recipients, ExternalRoleTypeEnum.DebtCounsellor);
            //Assert LitigationAttorneys is in the grid
            recipients = base.View.GetRecipientNamesByExternalRole(ExternalRoleTypeEnum.LitigationAttorney);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByExternalRoleType(base.TestCase.DebtCounsellingKey, recipients, ExternalRoleTypeEnum.LitigationAttorney);
            //Assert PDA's is in the grid
            recipients = base.View.GetRecipientNamesByExternalRole(ExternalRoleTypeEnum.PaymentDistributionAgent);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByExternalRoleType(base.TestCase.DebtCounsellingKey, recipients, ExternalRoleTypeEnum.PaymentDistributionAgent);
            //Assert NCR Companies is in the grid
            recipients = base.View.GetRecipientNamesByExternalRole(ExternalRoleTypeEnum.NationalCreditRegulator);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByExternalRoleType(base.TestCase.DebtCounsellingKey, recipients, ExternalRoleTypeEnum.NationalCreditRegulator);
            //Assert LegalEntitie Main Applicants Roles on Account
            recipients = base.View.GetRecipientNamesByLegalEntityRole(RoleTypeEnum.MainApplicant);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByLegalEntityRoleType(base.TestCase.DebtCounsellingKey, recipients, RoleTypeEnum.MainApplicant);
            //Assert LegalEntitie Suretor Roles on Account
            recipients = base.View.GetRecipientNamesByLegalEntityRole(RoleTypeEnum.Suretor);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByLegalEntityRoleType(base.TestCase.DebtCounsellingKey, recipients, RoleTypeEnum.Suretor);

            //Test Validation Messages
            base.View.Send();
            var messages = base.Browser.Page<BasePage>().GetValidationMessages();
            DebtCounsellingAssertions.AssertClientCommunicationValidationMessages(messages.ToArray(), isEmail: true);

            //Capture and Send
            base.View.CheckRecipients();
            base.View.Populate("Test Email Send", "BlahBlahBlah Test");
            base.View.Send();

            //Check that there are no validation messages
            messages = base.Browser.Page<BasePage>().GetValidationMessages();
            Assert.That(messages.Count == 0, "There were validation messages");
            //Assert ClientEmail
            DebtCounsellingAssertions.AssertClientCommunication(base.TestCase.DebtCounsellingKey, isEmail: true);
        }
    }
}