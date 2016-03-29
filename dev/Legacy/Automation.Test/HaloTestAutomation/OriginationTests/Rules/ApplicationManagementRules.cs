using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Exceptions;
using WatiN.Core.Logging;
using Description = NUnit.Framework.DescriptionAttribute;

namespace Origination.Rules
{
    /// <summary>
    /// Contains rule tests for the Application Management workflow.
    /// </summary>
    [TestFixture, RequiresSTA]
    public class ApplicationManagementRules : Origination.OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            Service<ICommonService>().DeleteTestMethodDataForFixture("ApplicationManagementRules");
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (base.Browser != null)
            {
                try
                {
                    base.View.CheckForErrorMessages();
                }
                finally
                {
                    base.Browser.Dispose();
                    base.Browser = null;
                }
            }
        }

        #region Tests

        /// <summary>
        /// Ensures a main applicant whose legal entity address is currently being used as the mailing address results in a validation warning and the applicant is
        /// not removed from the application
        /// </summary>
        [Test, Description(@"Ensures a main applicant whose legal entity address is currently being used as the mailing address results in a validation warning
			and the applicant is not removed from the application")]
        public void _001_RemoveMainApplicantMailingAddressCheck()
        {
            int maAddressKey;
            int lEkey;
            //Get an open offer in the Application Management map to use for the test
            QueryResults queryResults = Service<IX2WorkflowService>().GetOpenApplicationManagementOffers(1);
            //Get the ADUser who has this case on their worklist, use this to login later
            string adusername = (queryResults.RowList[0].Column(1).Value);
            int offerkey = queryResults.RowList[0].Column(2).GetValueAs<int>();

            //Add a new legalEntity and setup the Mailing address for this test
            string legalEntityName = Service<ILegalEntityService>().AddNewLegalEntity(offerkey, OfferRoleTypeEnum.MainApplicant, true,
                out lEkey, out maAddressKey);

            base.Browser = new TestBrowser(adusername, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerkey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            //Remove Main Applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);
            //Assert
            OfferAssertions.AssertOfferRoleExists(lEkey, offerkey, GeneralStatusEnum.Active, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("This applicant cannot be removed - their address is the application mailing address.");
        }

        /// <summary>
        /// Ensure that removing an Income Contributor that has an Active Employment record should result in the recalculation of Employment Type, Household Income and PTI
        /// </summary>
        [Test, Description(@"Ensure that removing an Income Contributor that has an Active Employment record should result in the recalculation of Employment Type,
		Household Income and PTI")]
        public void _005_RemoveApplicantIncomingContributorRecalculation()
        {
            //Get Any Offer
            QueryResults applicationManagementOffers = Service<IX2WorkflowService>().GetOpenApplicationManagementOffers(1);
            //Get the User who has this offer assigned to him/her
            string adusername = (applicationManagementOffers.RowList[0].Column(1).Value);
            int offerKey = applicationManagementOffers.RowList[0].Column(2).GetValueAs<int>();

            //Get Any Legal Entity not linked to this offer
            var legalEntity = Service<ILegalEntityService>().GetLegalEntityIDNumberNotLinkedToOffer(offerKey);

            base.Browser = new TestBrowser(adusername, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            double householdIncomeBeforeAdd =
                Service<IApplicationService>().GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(offerKey).Rows(0).Column("HouseholdIncome").GetValueAs<double>();
            double ptiBeforeAdd = Service<IApplicationService>().GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(offerKey).Rows(0).Column("PTI").GetValueAs<double>();

            //Add an existing legal entity to this offer from the frontend
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerKey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddExistingLegalEntity(legalEntity.Column(0).Value);

            //Check whether the PTI and Household Income has changed (Recalculated)
            double householdIncomeAfterAdd =
                Service<IApplicationService>().GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(offerKey).Rows(0).Column("HouseholdIncome").GetValueAs<double>();
            double ptiAfterAdd = Service<IApplicationService>().GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(offerKey).Rows(0).Column("PTI").GetValueAs<double>();

            Assert.AreNotEqual(householdIncomeBeforeAdd, householdIncomeAfterAdd);
            Assert.AreNotEqual(ptiBeforeAdd, ptiAfterAdd);

            //Let's Remove the Applicant
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerKey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntity.Column(1).Value);

            //Check whether the PTI and Household Income has changed (Recalculated)
            double householdIncomeAfterRemove =
                Service<IApplicationService>().GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(offerKey).Rows(0).Column("HouseholdIncome").GetValueAs<double>();
            double ptiAfterRemove =
                Service<IApplicationService>().GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(offerKey).Rows(0).Column("PTI").GetValueAs<double>();
            Assert.AreNotEqual(householdIncomeAfterAdd, householdIncomeAfterRemove);
            Assert.AreNotEqual(ptiAfterAdd, ptiAfterRemove);
        }

        /// <summary>
        /// Ensures a surety whose legal entity address is currently being used as the mailing address results in a
        /// validation warning and the applicant is not removed from the application
        /// </summary>
        [Test, Description(@"Ensures a surety whose legal entity address is currently being used as the mailing address results in a
							validation warning and the applicant is not removed from the application")]
        public void _002_RemoveSuretyMailingAddressCheck()
        {
            int maAddressKey;
            int lEkey;
            //Get an open offer in the Application Management map to use for the test
            QueryResults queryResults = Service<IX2WorkflowService>().GetOpenApplicationManagementOffers(1);

            //Get the ADUser who has this case on their worklist, use this to login later
            string adusername = (queryResults.RowList[0].Column(1).Value);
            int offerkey = queryResults.RowList[0].Column(2).GetValueAs<int>();

            //Add a new legalEntity and setup the Mailing address for this test
            string legalEntityName = Service<ILegalEntityService>().AddNewLegalEntity(offerkey, OfferRoleTypeEnum.Suretor, true,
                out lEkey, out maAddressKey);
            base.Browser = new TestBrowser(adusername, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerkey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            //Remove Main Applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);
            //Assert
            OfferAssertions.AssertOfferRoleExists(lEkey, offerkey, GeneralStatusEnum.Active, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("This applicant cannot be removed - their address is the application mailing address.");
        }

        /// <summary>
        ///	Removing a surety from a new business application whose address is not being used
        ///	as the application mailing address AND whose bank account is not the application
        ///	debit order bank account results in NO validation message and the applicant is removed
        /// </summary>
        [Test, Description("Removing a surety from a new business application whose address is not being used as the application mailing address")]
        public void _003_RemoveSurety()
        {
            //Get an open offer in the Application Management map to use for the test
            QueryResults queryResults = Service<IX2WorkflowService>().GetOpenApplicationManagementOffers(1);
            //Get the ADUser who has this case on their worklist, use this to login later
            string adusername = (queryResults.RowList[0].Column(1).Value);
            int offerkey = queryResults.RowList[0].Column(2).GetValueAs<int>();

            //Add a new legalEntity and setup the Mailing address for this test
            int maAddressKey;
            int lEkey;
            string legalEntityName = Service<ILegalEntityService>().AddNewLegalEntity(offerkey, OfferRoleTypeEnum.Suretor, false,
                out lEkey, out maAddressKey);

            base.Browser = new TestBrowser(adusername, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerkey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            //Remove Main Applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);
            OfferAssertions.AssertOfferRoleExists(lEkey, offerkey, GeneralStatusEnum.Inactive, false);
        }

        /// <summary>
        /// Ensures a main applicant can be removed from a new business application whose address is not being used as the application mailing address
        /// AND whose bank account is not the application debit order bank account
        /// AND who is not the last main applicant on an application
        /// </summary>
        [Test, Description("Ensures a main applicant can be removed from a new business application when all validation passes")]
        public void _004_RemoveMainApplicant()
        {
            int maAddressKey = -1;
            int lEkey = -1;
            int offerkey = -1;

            //Get an open offer in the Application Management map to use for the test
            QueryResults queryResults = Service<IX2WorkflowService>().GetOpenApplicationManagementOffers(1);
            //Get the ADUser who has this case on their worklist, use this to login later
            string adusername = (queryResults.RowList[0].Column(1).Value);
            offerkey = queryResults.RowList[0].Column(2).GetValueAs<int>();

            //Add a new legalEntity and setup the Mailing address for this test
            string legalEntityName = Service<ILegalEntityService>().AddNewLegalEntity(offerkey, OfferRoleTypeEnum.MainApplicant, false,
                out lEkey, out maAddressKey);
            base.Browser = new TestBrowser(adusername, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerkey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);

            //Remove Main Applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);

            OfferAssertions.AssertOfferRoleExists(lEkey, offerkey, GeneralStatusEnum.Inactive, false);
        }

        /// <summary>
        /// Ensures a main applicant can not be removed when his/her bank account is currently being used
        /// as the debit order bank account
        /// </summary>
        [Test, Description(@"Trying to remove a main applicant from a new business application whose bank account is currently being used
							as the debit order bank account results in a validation warning and the applicant is not removed from the application ")]
        public void _006_RemoveApplicantWhoseBankIsAppDebitOrderAccount()
        {
            RemoveApplicantWithValidation(OfferRoleTypeEnum.MainApplicant);
        }

        /// <summary>
        /// Trying to remove a surety from a new business application whose bank account is
        /// currently being used as the debit order bank account results in a validation warning
        /// and the applicant is not removed from the application
        /// </summary>
        [Test, Description(@"Ensure surety cannot be removed from a new business application when their Bank Account is currently being used as the Debit Order Bank Account.")]
        public void _007_RemoveSuretorWhoseBankIsAppDebitOrderAccount()
        {
            RemoveApplicantWithValidation(OfferRoleTypeEnum.Suretor);
        }

        /// <summary>
        /// Removing a surety from a new business application whose address is not being used as the application mailing address
        /// AND whose bank account is not the application debit order bank account results in NO validation message
        /// and the applicant is removed
        /// </summary>
        [Test, Description(@"Ensure surety can be removed when their application mailing address and bank account is not being against the offer.")]
        public void _008_RemoveSuretorWhoseBankIsNotAppDebitOrderAccount()
        {
            RemoveApplicantWithOutValidation(OfferRoleTypeEnum.Suretor);
        }

        /// <summary>
        /// Removing a main applicant from a new business application whose address is not being used as the application mailing address
        /// AND whose bank account is not the application debit order bank account AND who is not the last main applicant on an application results in NO validation message
        /// and the applicant is removed
        /// </summary>
        [Test, Description(@"Ensure Main Applicant can be removed when their application mailing address and bank account is not being against the offer.")]
        public void _009_RemoveApplicantWhoseBankIsNotAppDebitOrderAccount()
        {
            RemoveApplicantWithOutValidation(OfferRoleTypeEnum.MainApplicant);
        }

        /// <summary>
        /// Check that a Pricing for Risk, rate override is removed on 'QA Complete',
        /// if a legal entity is added at 'QA' state that results in the max empirica score moving from between
        /// 575 and 595 to > 595
        /// </summary>
        ///
        [Test, Description(@"Check that a Pricing for Risk, rate override is removed on 'QA Complete'")]
        public void _010_CheckThatAPricingForRiskRateOverRideIsRemovedOnQAComplete()
        {
            int offerKey = -1;
            //Setup data
            //Data Setup\precondition: A loan exists at 'QA' state that has a Pricing for Risk, rate override applied because its
            //LTV < 80% and max empirica score is between 575 and 595
            var applicationResults = base.Service<IApplicationService>().GetOfferByStateAndfinancialAdjustmentSourceType(1,
                WorkflowStates.ApplicationManagementWF.QA, FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment, 0, 80);
            offerKey = applicationResults.RowList[0].Column("ApplicationKey").GetValueAs<int>();
            if (offerKey != -1)
            {
                //Insert the ITC
                Service<ILegalEntityService>().InsertITC(offerKey, 594, 576);

                //QA Complete
                base.Browser = new TestBrowser(TestUsers.CreditUnderwriter);
                base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.QA);

                base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QAComplete);

                ApplicationLoanDetailsAssertions.AssertOfferInformationFinancialAdjustmentExists(offerKey, FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                    false, 0.0);
            }
            else
            {
                Assert.Fail("No application found for test");
            }
        }

        /// <summary>
        /// Check that it is not possible to perform the 'Instruct Attorney'
        /// action for a case at 'Application Check' state that has the same property record as
        /// an active Registration Pipeline case. Ensure the wording of the error message is
        /// “Application AccountKey, containing the clients ID number and the respective property,
        /// already exists in the origination process.”.
        /// Ensure the error message displays the application number which already exists in 'Registration Pipeline'.
        /// </summary>
        [Test, Description(@"Check that it is not possible to perform the 'Instruct Attorney'
		action for a case at 'Application Check' state that has the same property record as
		an active Registration Pipeline case. Ensure the wording of the error message is
		“Application AccountKey, containing the clients ID number and the respective property,
		already exists in the origination process.”.
		Ensure the error message displays the application number which already exists in 'Registration Pipeline'.")]
        public void _011_AppManInstructAttorneySamePropExistsInRegPipeline()
        {
            QueryResults results;
            string applicationKeyColumn = "ApplicationKey";
            string adUsernameColumn = "adusername";
            string offerRoleKey = "offerRoleKey";
            string loginADUser = string.Empty;
            int origPropertyKey = 0;
            int applicationKey = 0;

            try
            {
                // Find an Offer that we test with at Application Check
                results = Service<IApplicationService>().GetTestOfferByStateAndAdUserForDuplicatePropertyRuleTest(1, WorkflowStates.ApplicationManagementWF.ApplicationCheck, "6,7,8",
                    ADUserGroups.RegistrationsAdministrator);

                applicationKey = results.RowList[0].Column(applicationKeyColumn).GetValueAs<int>();
                string adUserName = results.RowList[0].Column(adUsernameColumn).Value;
                string attorney = results.RowList[0].Column(offerRoleKey).Value;
                origPropertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(applicationKey);

                //Check if this offer has an attorney selected
                if (String.IsNullOrEmpty(attorney))
                {
                    var attorneyModel = base.Service<IAttorneyService>().GetAttorney(true, true);
                    int attorneyLegalEntityKey = attorneyModel.LegalEntity.LegalEntityKey;
                    if (!base.Service<IApplicationService>().CreateOfferRole(attorneyLegalEntityKey, applicationKey, OfferRoleTypeEnum.ConveyanceAttorney, GeneralStatusEnum.Active))
                    {
                        Logger.LogAction("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})",
                            applicationKey, attorneyLegalEntityKey);
                        throw new WatiNException(
                            String.Format("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})",
                            applicationKey, attorneyLegalEntityKey));
                    }
                }
                // Find an Offer that we test with at Application Check
                results = Service<IApplicationService>().GetOfferByStateAndAdUserForDuplicatePropertyRuleTest(1,
                    WorkflowStates.ApplicationManagementWF.RegistrationPipeline, "6,7,8", ADUserGroups.RegistrationsAdministrator);

                // Find an Offer from Reg PipeLine whose property we use to update the offer above so we can get the rule to fire
                int testApplicationKey = results.RowList[0].Column(applicationKeyColumn).GetValueAs<int>();

                // Fetch the PropertyKey of the case in Reg PipeLine
                int propertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(testApplicationKey);

                // Update the test Application with that of the PropertyKey from the case in Reg PipeLine
                Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(propertyKey, Convert.ToInt32(applicationKey));

                switch (adUserName)
                {
                    case ADUserGroups.RegistrationsAdministrator:
                        loginADUser = TestUsers.RegAdminUser;
                        break;

                    case ADUserGroups.RegistrationsLOAAdmin:
                        loginADUser = TestUsers.RegistrationsLOAAdmin;
                        break;

                    case ADUserGroups.RegistrationsSupervisor:
                        loginADUser = TestUsers.RegistrationsSupervisor;
                        break;

                    case ADUserGroups.ResubmissionAdmin:
                        loginADUser = TestUsers.ResubmissionAdmin;
                        break;

                    default:
                        loginADUser = TestUsers.RegistrationsManager;
                        break;
                }

                if (!string.IsNullOrEmpty(loginADUser))
                {
                    // Login & Find case
                    base.Browser = new TestBrowser(loginADUser, TestUsers.Password);
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
                    base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ApplicationCheck, applicationKey);

                    // Try to Instruct Attorney
                    base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);

                    base.Browser.Page<CorrespondenceProcessing>().Submit(CorrespondenceSendMethodEnum.Email);

                    // Assert validaton fires and correct message appears on the screen
                    base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(@"Application {0}, linked to the same property has already been instructed.", testApplicationKey));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (applicationKey > 0)
                {
                    //Revert the data
                    Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(origPropertyKey), Convert.ToInt32(applicationKey));
                }
            }
        }

        /// <summary>
        /// Check that it is possible to perform the 'Instruct Attorney' action
        /// for a case that has the same property record as an open Account.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the 'Instruct Attorney' action
		for a case that has the same property record as an open Account")]
        public void _012_AppManInstructAttorneySamePropOnOpenAccount()
        {
            QueryResults results = null;
            string applicationKeyColumn = "ApplicationKey";
            string adUsernameColumn = "adusername";
            string offerRoleKey = "offerRoleKey";
            string loginADUser = string.Empty;
            int origPropertyKey = 0;
            int applicationKey = 0;
            try
            {
                // Find an Offer that we test with at Application Check
                results = Service<IApplicationService>().GetTestOfferByStateAndAdUserForDuplicatePropertyRuleTest(1,
                    WorkflowStates.ApplicationManagementWF.ApplicationCheck, "6,7,8", ADUserGroups.RegistrationsAdministrator);
                applicationKey = results.RowList[0].Column(applicationKeyColumn).GetValueAs<int>();
                string adUserName = results.RowList[0].Column(adUsernameColumn).Value;
                string attorney = results.RowList[0].Column(offerRoleKey).Value;
                origPropertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(applicationKey);

                //Check if this offer has an attorney selected
                if (String.IsNullOrEmpty(attorney))
                {
                    var attorneyModel = base.Service<IAttorneyService>().GetAttorney(true, true);
                    int attorneyLegalEntityKey = attorneyModel.LegalEntity.LegalEntityKey;
                    if (!base.Service<IApplicationService>().CreateOfferRole(attorneyLegalEntityKey, applicationKey, OfferRoleTypeEnum.ConveyanceAttorney, GeneralStatusEnum.Active))
                    {
                        Logger.LogAction("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})", applicationKey, attorneyLegalEntityKey);
                        throw new WatiN.Core.Exceptions.WatiNException(String.Format("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})", applicationKey, attorneyLegalEntityKey));
                    }
                }

                // Find an Open Account
                int accountKey = Convert.ToInt32(base.Service<IAccountService>().GetAccountByAccountStatus(AccountStatusEnum.Open));

                // Fetch the PropertyKey of the case in Reg PipeLine
                var property = Service<IPropertyService>().GetPropertyByAccountKey(accountKey);

                // Update the test Application with that of the PropertyKey from the case in Reg PipeLine
                Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(property.PropertyKey, applicationKey);

                switch (adUserName)
                {
                    case ADUserGroups.RegistrationsAdministrator:
                        loginADUser = TestUsers.RegAdminUser;
                        break;

                    case ADUserGroups.RegistrationsLOAAdmin:
                        loginADUser = TestUsers.RegistrationsLOAAdmin;
                        break;

                    case ADUserGroups.RegistrationsSupervisor:
                        loginADUser = TestUsers.RegistrationsSupervisor;
                        break;

                    case ADUserGroups.ResubmissionAdmin:
                        loginADUser = TestUsers.ResubmissionAdmin;
                        break;

                    default:
                        loginADUser = TestUsers.RegistrationsManager;
                        break;
                }

                if (!string.IsNullOrEmpty(loginADUser))
                {
                    // Login & Find case
                    base.Browser = new TestBrowser(loginADUser, TestUsers.Password);
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
                    base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ApplicationCheck, applicationKey);

                    // Try to Instruct Attorney
                    base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);

                    base.Browser.Page<CorrespondenceProcessing>().Submit(CorrespondenceSendMethodEnum.Email);

                    Thread.Sleep(10000);

                    //Assert that the case does move to Reg Pipeline
                    X2Assertions.AssertApplicationExistAtAppManageState(Convert.ToInt32(applicationKey), WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Revert the data
                Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(origPropertyKey), Convert.ToInt32(applicationKey));
            }
        }

        /// <summary>
        /// Check that it is not possible to perform the 'Instruct Attorney' action for a case that has the same property record as another case at the 'Resubmission' state, until that resubmitted offer is Declined
        /// </summary>
        [Test, Description(@"Check that it is not possible to perform the 'Instruct Attorney' action for a case that has the same property record
							 as another case at the 'Resubmission' state, until that resubmitted offer is Declined")]
        public void _013_AppManInstructAttorneySamePropertyExistsInResubmissionState()
        {
            QueryResults qr = null;
            string applicationKeyColumn = "ApplicationKey";
            string adUsernameColumn = "adusername";
            string offerRoleKey = "offerRoleKey";
            string loginADUser = string.Empty;
            int origPropertyKey = 0;
            int applicationKey = 0;

            try
            {
                // Find an Offer that we test with at Application Check
                qr = Service<IApplicationService>().GetTestOfferByStateAndAdUserForDuplicatePropertyRuleTest(1, WorkflowStates.ApplicationManagementWF.ApplicationCheck, "6,7,8",
                    ADUserGroups.RegistrationsAdministrator);
                applicationKey = qr.RowList[0].Column(applicationKeyColumn).GetValueAs<int>();
                string adUserName = qr.RowList[0].Column(adUsernameColumn).Value;
                string attorney = qr.RowList[0].Column(offerRoleKey).Value;
                origPropertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(applicationKey);

                //Check if this offer has an attorney selected
                if (String.IsNullOrEmpty(attorney))
                {
                    var attorneyModel = base.Service<IAttorneyService>().GetAttorney(true, true);
                    int attorneyLegalEntityKey = attorneyModel.LegalEntity.LegalEntityKey;
                    if (!base.Service<IApplicationService>().CreateOfferRole(attorneyLegalEntityKey, applicationKey, OfferRoleTypeEnum.ConveyanceAttorney, GeneralStatusEnum.Active))
                    {
                        Logger.LogAction("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})", applicationKey,
                            attorneyLegalEntityKey);
                        throw new WatiNException(String.Format("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})",
                            applicationKey, attorneyLegalEntityKey));
                    }
                }

                qr = Service<IApplicationService>().GetOfferByStateAndAdUserForDuplicatePropertyRuleTest(1, WorkflowStates.ApplicationManagementWF.Resubmission,
                    "6,7,8", ADUserGroups.RegistrationsAdministrator);

                Assert.True(qr.HasResults, "Failed to find an application at the resubmission state.");

                // Find an Offer from Reg PipeLine whose property we use to update the offer above so we can get the rule to fire
                var testApplicationKey = qr.RowList[0].Column(applicationKeyColumn).GetValueAs<int>();

                // Fetch the PropertyKey of the case in Reg PipeLine
                int propertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(testApplicationKey);

                // Update the test Application with that of the PropertyKey from the case in Reg PipeLine
                Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(propertyKey), Convert.ToInt32(applicationKey));

                switch (adUserName)
                {
                    case ADUserGroups.RegistrationsAdministrator:
                        loginADUser = TestUsers.RegAdminUser;
                        break;

                    case ADUserGroups.RegistrationsLOAAdmin:
                        loginADUser = TestUsers.RegistrationsLOAAdmin;
                        break;

                    case ADUserGroups.RegistrationsSupervisor:
                        loginADUser = TestUsers.RegistrationsSupervisor;
                        break;

                    case ADUserGroups.ResubmissionAdmin:
                        loginADUser = TestUsers.ResubmissionAdmin;
                        break;

                    default:
                        loginADUser = TestUsers.RegistrationsManager;
                        break;
                }

                if (!string.IsNullOrEmpty(loginADUser))
                {
                    // Login & Find case
                    base.Browser = new TestBrowser(loginADUser, TestUsers.Password);
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
                    base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ApplicationCheck, applicationKey);

                    // Try to Instruct Attorney
                    base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);

                    base.Browser.Page<CorrespondenceProcessing>().Submit(CorrespondenceSendMethodEnum.Email);

                    // Assert validaton fires and correct message appears on the screen
                    base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                        string.Format(@"Application {0}, linked to the same property has already been instructed.", testApplicationKey));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Revert the data
                Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(origPropertyKey), Convert.ToInt32(applicationKey));
            }
        }

        /// <summary>
        ///Check that it is possible to perform the 'Instruct Attorney' action
        ///for a Further Lending case which has the same property record as an
        ///active Further Lending case already in Registration Pipeline.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the 'Instruct Attorney' action for a
							Further Lending case which has the same property record as an active
							Further Lending case already in Registration Pipeline. ")]
        public void _014_AppManInstructAttorneySamePropExistsInRegPipeline()
        {
            QueryResults qr = null;
            string applicationKeyColumn = "ApplicationKey";
            string adUsernameColumn = "adusername";
            string offerRoleKey = "offerRoleKey";
            string loginADUser = string.Empty;
            int origPropertyKey = 0;
            int applicationKey = 0;

            try
            {
                // Find an Offer that we test with at Application Check
                qr = Service<IApplicationService>().GetTestOfferByStateAndAdUserForDuplicatePropertyRuleTest(1, WorkflowStates.ApplicationManagementWF.ApplicationCheck,
                    "2,3,4", ADUserGroups.RegistrationsAdministrator);

                applicationKey = qr.RowList[0].Column(applicationKeyColumn).GetValueAs<int>();
                string adUserName = qr.RowList[0].Column(adUsernameColumn).Value;
                string attorney = qr.RowList[0].Column(offerRoleKey).Value;
                origPropertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(applicationKey);

                //Check if this offer has an attorney selected
                if (String.IsNullOrEmpty(attorney))
                {
                    var attorneyModel = base.Service<IAttorneyService>().GetAttorney(true, true);
                    int attorneyLegalEntityKey = attorneyModel.LegalEntity.LegalEntityKey;
                    if (!base.Service<IApplicationService>().CreateOfferRole(attorneyLegalEntityKey, applicationKey, OfferRoleTypeEnum.ConveyanceAttorney, GeneralStatusEnum.Active))
                    {
                        Logger.LogAction("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})", applicationKey, attorneyLegalEntityKey);
                        throw new WatiNException(String.Format("Unable to create offer role for Application {0} with Attorney (Legal Entity Key {1})", applicationKey, attorneyLegalEntityKey));
                    }
                }

                //Find an Offer where we can pull the property from
                // Find an Offer that we test with at Application Check
                qr = Service<IApplicationService>().GetOfferByStateAndAdUserForDuplicatePropertyRuleTest(1, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, "6,7,8",
                    ADUserGroups.RegistrationsAdministrator);

                // Find an Offer from Reg PipeLine whose property we use to update the offer above so we can get the rule to fire
                var testApplicationKey = qr.RowList[0].Column(applicationKeyColumn).GetValueAs<int>();

                // Fetch the PropertyKey of the case in Reg PipeLine
                var propertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(testApplicationKey);

                // Update the test Application with that of the PropertyKey from the case in Reg PipeLine
                Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(propertyKey, applicationKey);

                switch (adUserName)
                {
                    case ADUserGroups.RegistrationsAdministrator:
                        loginADUser = TestUsers.RegAdminUser;
                        break;

                    case ADUserGroups.RegistrationsLOAAdmin:
                        loginADUser = TestUsers.RegistrationsLOAAdmin;
                        break;

                    case ADUserGroups.RegistrationsSupervisor:
                        loginADUser = TestUsers.RegistrationsSupervisor;
                        break;

                    case ADUserGroups.ResubmissionAdmin:
                        loginADUser = TestUsers.ResubmissionAdmin;
                        break;

                    default:
                        loginADUser = TestUsers.RegistrationsManager;
                        break;
                }

                if (!string.IsNullOrEmpty(loginADUser))
                {
                    // Login & Find case
                    base.Browser = new TestBrowser(loginADUser, TestUsers.Password);
                    base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
                    base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ApplicationCheck, applicationKey);

                    // Try to Instruct Attorney
                    base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);

                    base.Browser.Page<CorrespondenceProcessing>().Submit(CorrespondenceSendMethodEnum.Email);

                    Thread.Sleep(10000);

                    //Assert that the case does move to Reg Pipeline
                    X2Assertions.AssertApplicationExistAtAppManageState(Convert.ToInt32(applicationKey),
                        WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Revert the data
                Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(origPropertyKey), Convert.ToInt32(applicationKey));
            }
        }

        #endregion Tests

        #region Helper Methods

        private void RemoveApplicantWithValidation(OfferRoleTypeEnum ort)
        {
            int maAddressKey = -1;
            int lEkey = -1;
            int offerkey = -1;
            int lebaKey = -1;
            int odoKey = -1;

            //Get an open offer in the Application Management map to use for the test
            QueryResults queryResults = Service<IX2WorkflowService>().GetOpenApplicationManagementOffers(1);

            //Get the ADUser who has this case on their worklist, use this to login later
            string adusername = (queryResults.RowList[0].Column(1).Value);
            offerkey = queryResults.RowList[0].Column(2).GetValueAs<int>();

            //Add a new legalEntity and setup the Mailing address for this test
            string legalEntityName = Service<ILegalEntityService>().AddNewLegalEntity(offerkey, ort, false,
                out lEkey, out maAddressKey);
            // Set Up a BankAccount for the Legal Entity that will be used as the Offer
            int baKey = -1;
            Service<ILegalEntityService>().AddNewBankAccount(lEkey, offerkey, out baKey, out lebaKey, out odoKey);

            base.Browser = new TestBrowser(adusername, TestUsers.Password);

            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerkey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);

            //Remove Main Applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);

            OfferAssertions.AssertOfferRoleExists(lEkey, offerkey, GeneralStatusEnum.Active, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageDoesNotExist("This applicant cannot be removed -  Applicant's bank account is the application debit order bank account.");
        }

        private void RemoveApplicantWithOutValidation(OfferRoleTypeEnum ort)
        {
            int maAddressKey = -1;
            int leKey = -1;
            int offerkey = -1;

            //Get an open offer in the Application Management map to use for the test
            QueryResults queryResults = Service<IX2WorkflowService>().GetOpenApplicationManagementOffers(1);

            //Get the ADUser who has this case on their worklist, use this to login later
            string adusername = (queryResults.RowList[0].Column(1).Value);
            offerkey = queryResults.RowList[0].Column(2).GetValueAs<int>();

            //Add a new legalEntity and setup the Mailing address for this test
            string legalEntityName = Service<ILegalEntityService>().AddNewLegalEntity(offerkey, ort, false, out leKey, out maAddressKey);

            base.Browser = new TestBrowser(adusername, TestUsers.Password);

            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationManagementWF.ManageApplication, offerkey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);

            //Remove Main Applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);

            OfferAssertions.AssertOfferRoleExists(leKey, offerkey, GeneralStatusEnum.Active, false);
        }

        #endregion Helper Methods
    }
}