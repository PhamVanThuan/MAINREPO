using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using WatiN.Core;
using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests.Views
{
    /// <summary>
    /// As a Debt Counseling Consultant, I want to be able to send a sms or an email to a legal entity who is under debt review so that we have alternative options of
    /// communication. Tests:
    /// </summary>
    [RequiresSTA]
    public sealed class ClientSms : DebtCounsellingTests.TestBase<ClientCommunicationDebtCounsellingSMS>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        ///1.   The SMS options are:
        ///     SAHL /RCS Banking Details (pre-defined sms)
        ///     Free Format SMS. They will only ever select one of these options, cannot select both at same time.
        ///2.   Test to be sure that if it’s an RCS account we send the RCS banking details.
        ///3.   The user must select ANY legal entity playing a role within Debt Counseling case from the grid and then be able to send a free format text sms to a mobile number for the selected legal entity when the “Free Format SMS” option is checked.
        ///4.   The user must be able to send the predefined banking details SMS to the selected legal entity.
        ///     The SAHL banking details sms should read as follows:
        ///     SAHL Bank Details: Bank Name: STD Bank Name of Acc: SAHL Trust Acc Branch Code: 042826 Acc No: 251171442 Ref:(Account Number)
        ///     The RCS banking details sms should read as follows:
        ///     RCS Bank Details: Bank Name: STD Bank Name of Acc: RCS Home Loans Branch Code: 042826 Acc No: 251191214 Ref:(Account Number)
        ///5.   If the user chooses to send a free-format sms then the “Free Format SMS” must not exceed 160 characters.
        ///6.   After a SMS has been sent, the history of the sms must reflect under the correspondence summary. E.g. (Banking Details / Free format text> was sent to (Cell phone number) on (Current Date).
        ///7.   In the case of the free format sms, the actual sms wording must be inserted into the correspondence history. i.e. (Free format sms then the actual message> was sent to (Cell phone number> on (Current Date)
        ///8.   The user must have the option to send the same sms to multiple legal entities therefore the SMS screen should remain open until the user presses the “Complete” button.
        ///9.   The “Free Format SMS” is not automatically cleared.
        ///10.  The sms request will not move the case from the current state.
        /// </summary>
        [Test]
        public void SendSms()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<Navigation.CorrespondenceNode>().Correspondence();
            base.Browser.Navigate<Navigation.CorrespondenceNode>().SendSMS();
            //Assert Clients is in the grid
            var recipients = base.View.GetRecipientNamesByExternalRole(ExternalRoleTypeEnum.Client);
            if (recipients.Count > 0)
                DebtCounsellingAssertions.AssertClientCommunicationRecipientsByExternalRoleType(base.TestCase.DebtCounsellingKey, recipients, ExternalRoleTypeEnum.Client);
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
            List<string> messages = base.Browser.Page<BasePage>().GetValidationMessages();
            DebtCounsellingAssertions.AssertClientCommunicationValidationMessages(messages.ToArray(), isSMS: true);
            base.View.CheckRecipients();
            base.View.Populate("Free Format", "Testing Free Format SMS");
            base.View.Send();
            //Check that there are no validation messages
            messages = base.Browser.Page<BasePage>().GetValidationMessages();
            Assert.That(messages.Count == 0, "There were validation messages");
            //Assert ClientEmail
            DebtCounsellingAssertions.AssertClientCommunication(base.TestCase.DebtCounsellingKey, isSMS: true);
        }
    }
}