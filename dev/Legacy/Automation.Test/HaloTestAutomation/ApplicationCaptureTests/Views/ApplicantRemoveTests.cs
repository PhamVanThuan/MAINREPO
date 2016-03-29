using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace ApplicationCaptureTests.Views
{
    /// <summary>
    /// Contains tests for the capturing of an application mailing address
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class ApplicantRemoveTests : TestBase<ApplicantsRemove>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.BranchConsultant3);
        }

        [Test]
        public void when_removing_applicant_from_offer_should_remove_pending_legalentity_offerdomiciliums()
        {
            int offerKey = CreateCase();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, offerKey);
            var idNumber = String.Empty;
            Helper.AddNaturalPersonApplicantToOffer(base.Browser, offerKey, out idNumber, false);
            var offerRoles = from or in base.Service<IApplicationService>().GetActiveOfferRolesByOfferKey(offerKey, OfferRoleTypeGroupEnum.Client) where or.OfferRoleTypeKey != OfferRoleTypeEnum.AssuredLife select or;

            var offerRole = offerRoles.FirstOrDefault();
            if (offerRole != null)
            {
                var leAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(offerRole.LegalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
                var leDom = Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(leAddress.LegalEntityAddressKey, offerRole.LegalEntityKey, GeneralStatusEnum.Pending);
                Service<IApplicationService>().InsertOfferRoleDomicilium(leDom.LegalEntityDomiciliumKey, offerRole.OfferRoleKey, offerRole.OfferKey);

                var legalName = Service<ILegalEntityService>().GetLegalEntityLegalName(offerRole.LegalEntityKey);

                base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
                base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalName);

                BuildingBlocks.Assertions.OfferAssertions.AssertOfferRoleDomiciliumLegalEntityDomiciliumLinkNotExist(offerKey, offerRole.LegalEntityKey);
                BuildingBlocks.Assertions.LegalEntityAssertions.AssertNoLegalEntityDomicilium(leAddress.LegalEntityKey, GeneralStatusEnum.Pending);
            }
            else
            {
                Assert.Fail("No Offer Roles Found");
            }
        }

        /// <summary>
        /// Ensures a main applicant whose legal entity address is currently being used as the mailing address results in a validation warning and the applicant is not removed from the application
        /// </summary>
        [Test, Description(@"Ensures a main applicant whose legal entity address is currently being used as the mailing address results in a validation warning
		and the applicant is not removed from the application ")]
        public void RemoveMainApplicantMailingAddressCheck()
        {
            int legalEntityKey;
            int mailingAddressKey;
            int offerKey = CreateCase();
            Service<IApplicationService>().InsertOfferMailingAddress(offerKey);
            string legalEntityName = Service<ILegalEntityService>().AddNewLegalEntity(offerKey, OfferRoleTypeEnum.LeadMainApplicant, true, out legalEntityKey, out mailingAddressKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, offerKey);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            //Remove Main Applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("This applicant cannot be removed - their address is the application mailing address.");
        }

        [Test, Description("Ensures a main applicant whose the only main applicant on a new business application cannot be removed and results in a warning message ")]
        public void RemoveOnlyMainApplicantFromNewBusinessApplication()
        {
            int offerKey = CreateCase();
            //navigate to and remove main applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant();
            //assert error message displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Mortgage Loan must have at least one active Legal Entity role of type LeadMainApplicant or MainApplicant");
        }

        private int CreateCase()
        {
            //create offer
            int offerKey = 0;
            //clear flowbo
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            //application calculator
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            //complete the calculator
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.Edge, "3500000", "2500000", EmploymentType.Salaried, null, false, "105000",
                ButtonTypeEnum.CreateApplication);
            //add the LE
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(RoleType.LeadMainApplicant, true, null, "Mr", "auto", "Luke", "Duncan",
                null, Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English, null, null, null, null, null,
                null, null, "0824968726", null, false, false, false, true, false, ButtonTypeEnum.Next);
            offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //assert offer created
            Assert.That(offerKey != 0, "Offer not created");
            return offerKey;
        }
    }
}