using BuildingBlocks;
using BuildingBlocks.Navigation.FLOBO.PersonalLoan;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PersonalLoansTests.Views
{
    [RequiresSTA]
    public class CampaignExtractTests : PersonalLoansWorkflowTestBase<PersonalLoanCampaignExtract>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.PersonalLoanManager);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<PersonalLoanNode>().PersonalLoan();
            base.Browser.Navigate<PersonalLoanNode>().ImportLead();
        }

        protected override void OnTestFixtureTearDown()
        {
            base.OnTestFixtureTearDown();
            Service<IWatiNService>().KillAllIEProcesses();
        }

        //Negative test to check the correct alert messages appear when you click "Import" without uploading a file.
        [Test]
        public void when_adding_leads_with_no_csv_file_a_validation_message_should_be_received()
        {
            base.Browser.Page<PersonalLoanCampaignExtract>().ClickImport();
            var messages = base.Browser.Page<BasePage>().GetValidationMessages();
            string expectedMessage = "No header record was found.";
            Assert.That(messages.Contains(expectedMessage), "The expected validation message: \"{0}\" does not equal the actual message: \"{1}\"",
                expectedMessage, messages[0].ToString());
        }

        //Positive test to just check that the objects exist on the screen
        [Test]
        public void when_navigating_to_the_upload_screen_all_controls_should_exist()
        {
            base.Browser.Page<PersonalLoanCampaignExtract>().CampaignExtractPageAssertions();
        }

        [Test]
        public void when_uploading_a_file_all_leads_should_be_created_and_assigned()
        {
            IEnumerable<Automation.DataModels.LegalEntity> peopleWhoQualifyForPersonalLoans = Service<IPersonalLoanService>().GetXNumberOfLegalEntitiesWhoQualifyForPersonalLoans(10);
            string filePath = CreateCSVFileForImport(peopleWhoQualifyForPersonalLoans);
            base.View.UploadFile(filePath);
            List<string> results = Service<IPersonalLoanService>().WaitUntilAllBatchLeadsHaveBeenCreated(peopleWhoQualifyForPersonalLoans.Count());
            Assert.That(results.Count == peopleWhoQualifyForPersonalLoans.Count(), "Not all batch leads were successfully created.");
            foreach (var idNumber in results)
            {
                Assert.That(Service<IPersonalLoanService>().CheckLegalEntityHasValidPersonalLoanApplication(idNumber, WorkflowStates.PersonalLoansWF.ManageLead) == true,
                    string.Format(@"No Application found at the Manage Lead state for ID Number: {0}", idNumber));
            }
        }

        /// <summary>
        /// Assert that a client with an existing personal loan application cannot be added as a lead
        /// </summary>
        [Test]
        public void Given_That_An_Existing_Personal_Loan_Application_Is_Added_As_A_Lead_The_Import_Should_Fail()
        {
            // Get a test case
            var personalLoanApplication = Service<IApplicationService>().GetRandomOfferRecord(ProductEnum.PersonalLoan, OfferTypeEnum.UnsecuredLending, OfferStatusEnum.Open);
            var activeOfferRoles = Service<IApplicationService>().GetExternalRolesByOfferKey(personalLoanApplication.OfferKey);
            var legalEntityIDNumber = Service<ILegalEntityService>().GetLegalEntity(string.Empty, string.Empty, string.Empty, string.Empty, activeOfferRoles.FirstOrDefault().LegalEntityKey).IdNumber.ToString();

            // Try and import the legal entities
            string filePath = CreateCSVFileForImport(legalEntityIDNumber);
            base.View.UploadFile(filePath);
            List<string> importResults = Service<IPersonalLoanService>().WaitUntilAllBatchLeadsHaveBeenCreated(1);
            Assert.That(importResults.Count == 0, "An existing and open Personal Loan application cannot be be imported as a Personal Loan lead.");
        }

        /// <summary>
        /// Assert that when trying to import an existing Capitec client, the requisite rules are run and the import is not allowed.
        /// </summary>
        [Test]
        public void Given_That_A_Capitec_Client_Is_Added_As_A_Lead_The_Import_Should_Fail()
        {
            // Get a test case
            var results = Service<IAccountService>().GetOpenAccountAndAssociatedOfferWithoutAGivenProductType(OfferTypeEnum.NewPurchase, ProductEnum.PersonalLoan);
            int accountKey = int.Parse(results.First().Columns[0].Value);
            int offerKey = int.Parse(results.First().Columns[1].Value);
            IEnumerable<Automation.DataModels.LegalEntity> legalEntities = Service<IPersonalLoanService>().GetLegalEntityDetailsForCapitecAccount(accountKey);

            // Delete Offer Attribute if it already exists
            Service<IApplicationService>().DeleteOfferAttribute(offerKey, OfferAttributeTypeEnum.Capitec);

            // Insert the Capitec offer attribute against the offer
            Service<IApplicationService>().InsertOfferAttribute(offerKey, OfferAttributeTypeEnum.Capitec);

            // Try and import the legal entities
            string filePath = CreateCSVFileForImport(legalEntities);
            base.View.UploadFile(filePath);
            List<string> importResults = Service<IPersonalLoanService>().WaitUntilAllBatchLeadsHaveBeenCreated(1);
            Assert.That(importResults.Count == 0, "This is a Capitec Client and therefore cannot be offered a Personal Loan with SAHL.");

            // Delete the inserted offer attribute.
            Service<IApplicationService>().DeleteOfferAttribute(offerKey, OfferAttributeTypeEnum.Capitec);
        }

        /// <summary>
        /// Assert that when trying to import existing Capitec applicants, the requisite rules are run and the import is not allowed.
        /// </summary>
        [Test]
        public void Given_That_A_Capitec_Application_Is_Added_As_A_Lead_The_Import_Should_Fail()
        {
            Automation.DataModels.Offer capitecApplication = Service<IApplicationService>().GetCapitecMortgageLoanApplication();
            IEnumerable<Automation.DataModels.LegalEntity> legalEntities = Service<IPersonalLoanService>().GetLegalEntityDetailsForCapitecApplication(capitecApplication.OfferKey);

            string filePath = CreateCSVFileForImport(legalEntities);
            base.View.UploadFile(filePath);
            List<string> results = Service<IPersonalLoanService>().WaitUntilAllBatchLeadsHaveBeenCreated(1);
            Assert.That(results.Count == 0, "This is a Capitec Client and therefore cannot be offered a Personal Loan with SAHL.");
        }

        private static string CreateCSVFileForImport(IEnumerable<Automation.DataModels.LegalEntity> peopleWhoQualifyForPersonalLoans)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string scriptsFolder = System.IO.Path.Combine(startupPath, "Scripts");
            var filePath = System.IO.Path.Combine(scriptsFolder, string.Format("test_{0}.csv", Guid.NewGuid()));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("IDNumber");
                int i = 0;
                int count = peopleWhoQualifyForPersonalLoans.Count();
                foreach (var item in peopleWhoQualifyForPersonalLoans)
                {
                    if (i != count - 1)
                    {
                        writer.WriteLine(string.Format("{0},", item.IdNumber));
                    }
                    else
                    {
                        writer.WriteLine(string.Format("{0}", item.IdNumber));
                    }
                    i++;
                }
            }
            return filePath;
        }

        private string CreateCSVFileForImport(string IDNumber)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string scriptsFolder = System.IO.Path.Combine(startupPath, "Scripts");
            var filePath = System.IO.Path.Combine(scriptsFolder, string.Format("test_{0}.csv", Guid.NewGuid()));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("IDNumber");
                writer.WriteLine(string.Format("{0}", IDNumber));
            }
            return filePath;
        }
    }
}