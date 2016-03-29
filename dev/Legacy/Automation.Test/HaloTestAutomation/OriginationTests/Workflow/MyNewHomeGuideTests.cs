using System;
using System.Collections.Generic;
using System.Linq;
using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace Origination.Workflow
{
    [RequiresSTA]
    public class MyNewHomeGuideTests : OriginationTestBase<BasePage>
    {
        private OfferAttributeTypeEnum offerAttributeTypeKey;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            offerAttributeTypeKey = OfferAttributeTypeEnum.AlphaHousing;
            base.Browser = new TestBrowser(TestUsers.RegistrationsLOAAdmin);
        }

        [Test]
        public void when_attorney_intructed_for_new_business_alpha_housing_application_my_new_home_info_is_sent()
        {
            //search for application at LOA
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.LOA, Workflows.ApplicationManagement,
                OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);
            //SETUP TEST CASE
            //ensure application is alpha housing
            var isAlphaHousing = (from att in Service<IApplicationService>().GetOfferAttributes(offerKey)
                                  where att.OfferAttributeTypeKey == offerAttributeTypeKey
                                  select att != null).FirstOrDefault();
            if (isAlphaHousing == false)
            {
                Service<IApplicationService>().InsertOfferAttribute(offerKey, offerAttributeTypeKey);
            }
            //ensure correspondence medium for application mailing address is set to email
            var emailAddress = "emailsent@email.com";
            var emailSubject = "My New Home";
            var offerMailingAddress = Service<IApplicationService>().GetOfferMailingAddress(offerKey);
            if (offerMailingAddress.CorrespondenceMedium != CorrespondenceMedium.Email)
            {
                Service<IApplicationService>().UpdateCorrespondenceMedium(offerKey, CorrespondenceMediumEnum.Email);
            }
            Service<ILegalEntityService>().UpdateEmailAddress(offerMailingAddress.LegalEntityKey, emailAddress);
            //set email sent indicator to false
            Service<IX2WorkflowService>().UpdateAlphaHousingSurveyEmailSent(offerKey, false);
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            //instruct the attorney
            InstructAttorney(offerKey);
            //assert that the my new home email was sent to the application email address
            ClientEmailAssertions.AssertClientEmailRecordWithSubjectAndToAddressRecordExists(emailAddress, emailSubject, date);
            //assert that the email sent indicator set to true
            X2Assertions.AssertAlphaHousingSurveyEmailSentIsTrue(offerKey);
            //CLEANUP TEST CASE
            if (isAlphaHousing == false)
            {
                Service<IApplicationService>().DeleteOfferAttribute(offerKey, offerAttributeTypeKey);
            }
        }

        [Test]
        public void when_attorney_instructed_for_new_business_non_alpha_housing_application_my_new_home_info_is_not_sent()
        {
            //search for application at LOA
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.LOA, Workflows.ApplicationManagement,
                OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);
            //SETUP TEST CASE
            //ensure application is non alpha housing
            var isAlphaHousing = (from att in Service<IApplicationService>().GetOfferAttributes(offerKey)
                                  where att.OfferAttributeTypeKey == offerAttributeTypeKey
                                  select att != null).FirstOrDefault();
            if (isAlphaHousing == true)
            {
                Service<IApplicationService>().DeleteOfferAttribute(offerKey, offerAttributeTypeKey);
            }
            //ensure correspondence medium for application mailing address is set to email
            var emailAddress = "emailnotsent@email.com";
            var emailSubject = "My New Home";
            var offerMailingAddress = Service<IApplicationService>().GetOfferMailingAddress(offerKey);
            if (offerMailingAddress.CorrespondenceMedium != CorrespondenceMedium.Email)
            {
                Service<IApplicationService>().UpdateCorrespondenceMedium(offerKey, CorrespondenceMediumEnum.Email);
            }
            Service<ILegalEntityService>().UpdateEmailAddress(offerMailingAddress.LegalEntityKey, emailAddress);
            //set email sent indicator to false
            Service<IX2WorkflowService>().UpdateAlphaHousingSurveyEmailSent(offerKey, false);
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            //instruct the attorney
            InstructAttorney(offerKey);
            //assert that the my new home email was not sent
            ClientEmailAssertions.AssertNoClientEmailRecordExists(emailAddress, emailSubject, date);
            //assert that the email sent indicator was not set to true
            X2Assertions.AssertAlphaHousingSurveyEmailSentIsFalse(offerKey);
            //CLEANUP TEST CASE
            if (isAlphaHousing == true)
            {
                Service<IApplicationService>().InsertOfferAttribute(offerKey, offerAttributeTypeKey);
            }
        }

        [Test]
        public void when_application_mailing_address_is_not_set_to_email_send_my_new_home_info_to_all_main_applicants()
        {
            //search for application at LOA
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.LOA, Workflows.ApplicationManagement,
                    OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);

            //SETUP TEST CASE
            //ensure application is alpha housing
            var isAlphaHousing = (from att in Service<IApplicationService>().GetOfferAttributes(offerKey)
                                  where att.OfferAttributeTypeKey == offerAttributeTypeKey
                                  select att != null).FirstOrDefault();
            if (isAlphaHousing == false)
            {
                Service<IApplicationService>().InsertOfferAttribute(offerKey, offerAttributeTypeKey);
            }
            var emailSubject = "My New Home";
            //ensure correspondence medium for application mailing address is not set to email
            var offerMailingAddress = Service<IApplicationService>().GetOfferMailingAddress(offerKey);
            if (offerMailingAddress.CorrespondenceMedium == CorrespondenceMedium.Email)
            {
                Service<IApplicationService>().UpdateCorrespondenceMedium(offerKey, CorrespondenceMediumEnum.Post);
            }
            //set email sent indicator to false
            Service<IX2WorkflowService>().UpdateAlphaHousingSurveyEmailSent(offerKey, false);
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            //get email addresses for all active main applicants
            var results = GetEmailAddressesForAllActiveMainApplicants(offerKey);
            //instruct the attorney
            InstructAttorney(offerKey);
            //assert that the my new home email was sent to each main applicant
            foreach (var emailAddress in results)
            {
                ClientEmailAssertions.AssertClientEmailRecordWithSubjectAndToAddressRecordExists(emailAddress, emailSubject, date);
            }
            //assert that the email sent indicator was set to true
            X2Assertions.AssertAlphaHousingSurveyEmailSentIsTrue(offerKey);
            //CLEANUP TEST CASE
            if (isAlphaHousing == false)
            {
                Service<IApplicationService>().DeleteOfferAttribute(offerKey, offerAttributeTypeKey);
            }
        }

        [Test]
        public void when_my_home_info_has_previously_been_sent_the_info_is_not_resent()
        {
            //search for application at LOA
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.LOA, Workflows.ApplicationManagement,
            OfferTypeEnum.SwitchLoan, Exclusions.OrginationAutomation);
            //SETUP TEST CASE
            //ensure application is alpha housing
            var isAlphaHousing = (from att in Service<IApplicationService>().GetOfferAttributes(offerKey)
                                  where att.OfferAttributeTypeKey == offerAttributeTypeKey
                                  select att != null).FirstOrDefault();
            if (isAlphaHousing == false)
            {
                Service<IApplicationService>().InsertOfferAttribute(offerKey, offerAttributeTypeKey);
            }
            //ensure correspondence medium for application mailing address is set to email
            var emailAddress = "emailnotresent@email.com";
            var emailSubject = "My New Home";
            var offerMailingAddress = Service<IApplicationService>().GetOfferMailingAddress(offerKey);
            if (offerMailingAddress.CorrespondenceMedium != CorrespondenceMedium.Email)
            {
                Service<IApplicationService>().UpdateCorrespondenceMedium(offerKey, CorrespondenceMediumEnum.Email);
            }
            Service<ILegalEntityService>().UpdateEmailAddress(offerMailingAddress.LegalEntityKey, emailAddress);
            //set email sent indicator to true
            Service<IX2WorkflowService>().UpdateAlphaHousingSurveyEmailSent(offerKey, true);
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            //instruct the attorney
            InstructAttorney(offerKey);
            //assert that the my new home email was not sent
            ClientEmailAssertions.AssertNoClientEmailRecordExists(emailAddress, emailSubject, date);
            //assert that the email sent indicator was set to true
            X2Assertions.AssertAlphaHousingSurveyEmailSentIsTrue(offerKey);
            //CLEANUP TEST CASE
            if (isAlphaHousing == false)
            {
                Service<IApplicationService>().DeleteOfferAttribute(offerKey, offerAttributeTypeKey);
            }
        }

        private List<string> GetEmailAddressesForAllActiveMainApplicants(int offerKey)
        {
            var offerRoleTypeKey = (int)OfferRoleTypeEnum.MainApplicant;
            List<string> contactDetails = new List<string>();

            IQueryable<QueryResultsRow> results = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(offerKey).Where(x =>
                            x.Column("OfferRoleTypeKey").GetValueAs<int>() == offerRoleTypeKey);
            var emailAddresses = results.Where<QueryResultsRow>(x => x.Column("EmailAddress").GetValueAs<string>() != null);
            if (emailAddresses.Count<QueryResultsRow>() == 0)
            {
                foreach (var result in results)
                {   
                    var emailAddress = string.Format(@"{0}@test.com", result.Column("LegalEntityKey").GetValueAs<string>());
                    Service<ILegalEntityService>().UpdateEmailAddress(result.Column("LegalEntityKey").GetValueAs<int>(), emailAddress);
                    contactDetails.Add(emailAddress);
                }
            }
            else
            {
                foreach (var emailAddress in emailAddresses)
                {
                    contactDetails.Add(emailAddress.Column("EmailAddress").GetValueAs<string>());
                }
            }

            return contactDetails;
        }

        private void InstructAttorney(int offerKey)
        {
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.LOAReceived);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.LOAAccepted);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.SelectAttorney);
            base.Browser.Page<Orig_SelectAttorney>().SelectAttorneyByDeedsOffice(DeedsOffice.CapeTown);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);
            base.Browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);
        }
    }
}