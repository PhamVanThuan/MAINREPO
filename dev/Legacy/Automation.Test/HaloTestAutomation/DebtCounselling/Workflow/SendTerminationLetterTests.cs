using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class SendTerminationLetterTests : DebtCounsellingTests.TestBase<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        [Test]
        public void when_termination_letter_sent_by_post_where_clients_share_domicilium_should_email_one_copy_of_letter_to_selected_debt_counsellor()
        {
            StartTestWithClientsThatShareADomiciliumAddress();
            var debtCounsellor = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            var clients = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);

            var debtCounsellorEmail = "debtcounselling@test.com";
            base.View.SelectMultipleCorrespondenceRecipientsForEmail(new List<Automation.DataModels.ExternalRole> { debtCounsellor }, debtCounsellorEmail);
            base.View.SelectMultipleCorrespondenceRecipientsForPost(clients);
            base.View.ClickSendCorrespondence();

            //Assert Debt Counsellor have only one copy of letter (will be the only one that have mail correspondence method checked)
            var leKeys = new List<int>
                {
                    clients.ToArray().Select(x => x.LegalEntityKey).FirstOrDefault()
                };
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded
                (
                    TestCase.DebtCounsellingKey,
                    CorrespondenceReports.DebtCounsellingTerminationLetter,
                    CorrespondenceMedium.Email,
                    base.TestCase.AccountKey,
                    leKeys,
                    true
                );
            //Assert client have only one copy of letter
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded
                (
                    TestCase.DebtCounsellingKey,
                    CorrespondenceReports.DebtCounsellingTerminationLetter,
                    CorrespondenceMedium.Post,
                    base.TestCase.AccountKey,
                    leKeys,
                    true
                );

            leKeys = new List<int>
                {
                    clients.ToArray().Select(x => x.LegalEntityKey).LastOrDefault()
                };
            CorrespondenceAssertions.AssertCorrespondenceRecordNotAdded
                (TestCase.DebtCounsellingKey, CorrespondenceReports.DebtCounsellingTerminationLetter, CorrespondenceMedium.Post, leKeys);
            //check the scheduled activity is setup
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling.Tendays, 10, true);
        }

        [Test]
        public void when_multiple_termination_letters_sent_by_post_where_clients_do_not_share_domicilium_should_email_one_copy_of_each_letter_to_selected_debt_counsellor()
        {
            StartTestWithClientsThatHaveDifferentDomiciliumAddresses();
            var debtCounsellor = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            var clients = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);

            var debtCounsellorEmail = "debtcounselling@test.com";
            base.View.SelectMultipleCorrespondenceRecipientsForEmail(new List<Automation.DataModels.ExternalRole> { debtCounsellor }, debtCounsellorEmail);
            base.View.SelectMultipleCorrespondenceRecipientsForPost(clients);
            base.View.ClickSendCorrespondence();

            //Assert Debt Counsellor have only 2 copies of letter (will be the only one that have mail correspondence method checked)
            var leKeys = clients.ToArray().Select(x => x.LegalEntityKey).ToList();
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded
                (
                    TestCase.DebtCounsellingKey,
                    CorrespondenceReports.DebtCounsellingTerminationLetter,
                    CorrespondenceMedium.Email,
                    base.TestCase.AccountKey,
                    leKeys,
                    true
                );
            //Assert Client have 2 copies of letter for Post
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded
                (
                    TestCase.DebtCounsellingKey,
                    CorrespondenceReports.DebtCounsellingTerminationLetter,
                    CorrespondenceMedium.Post,
                    base.TestCase.AccountKey,
                    leKeys,
                    true
                );
        }

        [Test]
        public void when_multiple_termination_letters_sent_by_email_and_post_where_clients_do_not_share_domicilium_should_email_one_copy_of_each_letter_to_selected_debt_counsellor()
        {
            StartTestWithClientsThatHaveDifferentDomiciliumAddresses();
            var debtCounsellor = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            var clients = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            var debtCounsellorEmail = "debtcounselling@test.com";
            var clientEmail = "client@test.com";
            base.View.SelectMultipleCorrespondenceRecipientsForEmail(new List<Automation.DataModels.ExternalRole> { debtCounsellor }, debtCounsellorEmail);
            base.View.SelectMultipleCorrespondenceRecipientsForPost(clients);
            base.View.SelectMultipleCorrespondenceRecipientsForEmail(clients, clientEmail);
            base.View.ClickSendCorrespondence();

            //Assert Debt Counsellor have only 2 copies of letter (will be the only one that have mail correspondence method checked)
            var leKeys = clients.ToArray().Select(x => x.LegalEntityKey).ToList();
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(
                    TestCase.DebtCounsellingKey,
                    CorrespondenceReports.DebtCounsellingTerminationLetter,
                    CorrespondenceMedium.Email,
                    base.TestCase.AccountKey,
                    leKeys,
                    true,
                    debtCounsellorEmail
                );
            //Assert Client have 2 copies of letter for Post
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded
                (
                    TestCase.DebtCounsellingKey,
                    CorrespondenceReports.DebtCounsellingTerminationLetter,
                    CorrespondenceMedium.Post,
                    base.TestCase.AccountKey,
                    leKeys,
                    true
                );
            //Assert Client have 2 copies of letter for email
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded
                (
                    TestCase.DebtCounsellingKey,
                    CorrespondenceReports.DebtCounsellingTerminationLetter,
                    CorrespondenceMedium.Email,
                    base.TestCase.AccountKey,
                    leKeys,
                    true,
                    clientEmail
                );
        }

        [Test]
        public void when_no_legal_entitity_domicilium_exists_the_termination_letter_cannot_be_sent()
        {
            //search for a case at Termination
            base.StartTest(WorkflowStates.DebtCounsellingWF.Termination, TestUsers.DebtCounsellingConsultant);
            var clientList = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            foreach (var client in clientList)
            {
                Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(client.LegalEntityKey);
            }
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendTerminationLetter);
            base.Browser.Page<BasePageAssertions>().AssertNotification("An Active Domicilium Address must be captured for all Clients on a Debt Counselling Case.");
        }

        [Test]
        public void when_sending_a_termination_letter_as_post_and_email_where_clients_share_domicilium_should_send_one_as_post_and_multiple_as_email()
        {
            StartTestWithClientsThatShareADomiciliumAddress();
            var debtCounsellor = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            var clients = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);

            base.View.SelectMultipleRecipientsForPostAndEmailCorrespondence(clients);

            //For email it's sent for both legal lentities when selected
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(TestCase.DebtCounsellingKey,
                CorrespondenceReports.DebtCounsellingTerminationLetter, CorrespondenceMedium.Email, base.TestCase.AccountKey, new List<int>(clients.Select(x => x.LegalEntityKey)), true);

            //For post it's sent for the first legal entity from the legalentities that share the domicilium.
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(TestCase.DebtCounsellingKey, CorrespondenceReports.DebtCounsellingTerminationLetter, CorrespondenceMedium.Post, base.TestCase.AccountKey,
              new List<int>() { clients.Select(x => x.LegalEntityKey).FirstOrDefault() }, true);
            CorrespondenceAssertions.AssertCorrespondenceRecordNotAdded
                (TestCase.DebtCounsellingKey, CorrespondenceReports.DebtCounsellingTerminationLetter, CorrespondenceMedium.Post, new List<int>() { clients.Select(x => x.LegalEntityKey).LastOrDefault() });
        }

        [Test]
        public void when_sending_a_termination_letter_at_least_one_client_must_be_selected()
        {
            StartTestWithClientsThatShareADomiciliumAddress();
            base.View.ClickSendCorrespondence();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select at least one client.");
        }

        [Test]
        public void when_sending_a_termination_letter_where_clients_do_not_share_domicilium_each_client_must_be_selected()
        {
            StartTestWithClientsThatHaveDifferentDomiciliumAddresses();
            var clients = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            var clientEmail = "client@test.com";
            base.View.SelectCorrespondenceRecipient(clients[0].LegalEntityKey, false, string.Empty, string.Empty, true, clientEmail, false, ButtonTypeEnum.SendCorrespondence);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("Must also select the following Clients as they have different Domicilium Addresses");
        }

        /// <summary>
        /// Given that there is no active postal address and no active residential address at any of the levels for the Debt Counsellor:
        /// 1. Assert that the user is informed that 'Every Debt Counsellor, Designation or Company requires an active address.'
        /// </summary>
        [Test, Description("Tests that before sending a Termination letter, there has to be an active residential or postal address for the Debt Counsellor company.")]
        public void when_sending_a_termination_letter_where_no_active_postal_or_residential_address_exists_for_debt_counsellor_should_alert_user()
        {
            // Insert a new Debt Counsellor without a residential and postal address
            var debtCounsellorLegalEntitKey = Service<ILegalEntityService>().CreateNewLegalEntityAsDebtCounsellor("Test Debt Counsellor");

            // Locate a case at stage 'Termination' and change their Debt Counsellor to the one created above
            StartTestWithClientsWhoseDebtCounsellorHasNoActivePostalOrResidentailAddress(debtCounsellorLegalEntitKey);

            // Perform the 'Send Termination Letter' action
            var clients = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendTerminationLetter);

            // Assert that the user is informed that there is no active postal address captured for the Debt Counsellor.
            base.Browser.Page<BasePageAssertions>().AssertNotification("Every Debt Counsellor, Designation or Company requires an active address.");

            // Cleanup by removing test debt counsellor
            Service<IExternalRoleService>().DeleteExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            Service<ILegalEntityService>().DeleteLegalEntity(debtCounsellorLegalEntitKey);
        }

        private void StartTestWithClientsThatHaveDifferentDomiciliumAddresses()
        {
            base.StartTestWithMultipleClientsUnderDebtCounselling(new[] { TestUsers.DebtCounsellingConsultant }, WorkflowStates.DebtCounsellingWF.ManageProposal);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.TerminateApplication, base.TestCase.DebtCounsellingKey);
            base.ReloadTestCase();
            base.LoadCase(WorkflowStates.DebtCounsellingWF.Termination);
            Service<IDebtCounsellingService>().SetupClientsWithDifferentDomiciliumsOnDebtCounsellingCase(base.TestCase.DebtCounsellingKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendTerminationLetter);
        }

        private void StartTestWithClientsThatShareADomiciliumAddress()
        {
            base.StartTestWithMultipleClientsUnderDebtCounselling(new[] { TestUsers.DebtCounsellingConsultant }, WorkflowStates.DebtCounsellingWF.ManageProposal);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.TerminateApplication, base.TestCase.DebtCounsellingKey);
            Service<IDebtCounsellingService>().SetupClientsWithSharedDomiciliumOnDebtCounsellingCase(base.TestCase.DebtCounsellingKey);
            base.ReloadTestCase();
            base.LoadCase(WorkflowStates.DebtCounsellingWF.Termination);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendTerminationLetter);
        }

        private void StartTestWithClientsWhoseDebtCounsellorHasNoActivePostalOrResidentailAddress(int debtCounsellorLegalEntitKey)
        {
            base.StartTestWithMultipleClientsUnderDebtCounselling(new[] { TestUsers.DebtCounsellingConsultant }, WorkflowStates.DebtCounsellingWF.ManageProposal);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.TerminateApplication, base.TestCase.DebtCounsellingKey);
            Service<IDebtCounsellingService>().SetupClientsWithDifferentDomiciliumsOnDebtCounsellingCase(base.TestCase.DebtCounsellingKey);
            Service<IDebtCounsellingService>().ChangeDebtCounsellor(debtCounsellorLegalEntitKey, base.TestCase.AccountKey);
            base.ReloadTestCase();
            base.LoadCase(WorkflowStates.DebtCounsellingWF.Termination);
        }
    }
}