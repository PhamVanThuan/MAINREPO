using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    //TODO: Add Legal Entity Update Applicant tests for Legal Entity FloBo
    [RequiresSTA]
    public sealed class LegalEntityUpdateApplicantTests : TestBase<BasePage>
    {
        #region PrivateVar

        /// <summary>
        /// IE TestBrowser Object
        /// </summary>
        private TestBrowser browser;

        /// <summary>
        /// Application Number
        /// </summary>
        private int _offerKey;

        /// <summary>
        /// Use to seed random numbers (use in passport and registrationnumber)
        /// </summary>
        private Random _randomGen;

        #endregion PrivateVar

        #region TestSetUpTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //Initialize browser get and load offer
            QueryResults results = Service<IApplicationService>().GetOpenApplicationCaptureOffer();
            QueryResultsRow row = results.Rows(0);
            this._offerKey = row.Column("offerkey").GetValueAs<int>();
            this.browser = new TestBrowser(TestUsers.BranchConsultant);
            this.browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            this.browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(this.browser);
            this.browser.Page<WorkflowSuperSearch>().Search(this._offerKey);
            this._randomGen = new Random();
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
        }

        protected override void OnTestTearDown()
        {
            browser.Page<BasePage>().CheckForErrorMessages();
        }

        protected override void OnTestFixtureTearDown()
        {
            base.OnTestFixtureTearDown();
        }

        #endregion TestSetUpTearDown

        #region Tests

        /// <summary>
        /// This test ensures that you can add update the details of an applicant on an application
        /// </summary>
        [Test]
        public void _01_UpdateLegalEntityNaturalPerson()
        {
            //Add company to the application
            var legalEntity = AddSACitizenLegalEntityApplicantToOffer();
            SelectNaturalLegalEntity(legalEntity.FirstNames);
            legalEntity.CitizenTypeKey = CitizenTypeEnum.SACitizenNonResident;
            //change the legal entity
            browser.Navigate<LegalEntityNode>().LegalEntityDetails(NodeTypeEnum.Update);
            browser.Page<LegalEntityDetailsUpdateApplicant>().UpdateLegalEntityDetails(legalEntity);
            LegalEntityAssertions.AssertLegalEntityCitizenType(legalEntity.IdNumber, CitizenTypeEnum.SACitizenNonResident);
        }

        /// <summary>
        /// You should no longer be allowed to update a legal entity to have a Citizen Type of Foreigner. This test attempts to update an applicant to have
        /// that citizen type, ensuring that a validation warning is displayed to the user.
        /// </summary>
        [Test]
        public void _02_UpdateForeignNaturalPersonLegalEntity()
        {
            //Add company to the application
            var legalEntity = AddForeignLegalEntityApplicantToOffer();
            SelectNaturalLegalEntity(legalEntity.FirstNames);
            legalEntity.CitizenTypeKey = CitizenTypeEnum.Foreigner;
            //change the legal entity
            browser.Navigate<LegalEntityNode>().LegalEntityDetails(NodeTypeEnum.Update);
            browser.Page<LegalEntityDetailsUpdateApplicant>().UpdateLegalEntityDetails(legalEntity);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("It is no longer possible to update to a Foreigner citizen type.");
        }

        [Test]
        public void _03_UpdateLegalEntityCompany()
        {
            //Add company to the application
            var legalEntity = AddCompanyApplicantToOffer();
            SelectCompanyLegalEntity(legalEntity.TradingName);
            string regNumber = string.Format(@"{0}/{1}", _randomGen.Next(0, 9999999), _randomGen.Next(0, 9999999));
            legalEntity.RegistrationNumber = regNumber;
            browser.Navigate<LegalEntityNode>().LegalEntityDetails(NodeTypeEnum.Update);
            browser.Page<LegalEntityDetailsUpdateApplicant>().UpdateLegalEntityDetails(legalEntity);
            //add check for the update
            var updatedlegalEntity = Service<ILegalEntityService>().GetLegalEntity(registrationNumber: regNumber);
            Assert.That(updatedlegalEntity != null);
        }

        #endregion Tests

        #region HelperMethods

        /// <summary>
        /// Add an company applicant to an application.
        /// </summary>
        private Automation.DataModels.LegalEntity AddCompanyApplicantToOffer()
        {
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            var legalEntity = Service<ILegalEntityService>().GetCompanyLegalEntity();
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(legalEntity);
            return legalEntity;
        }

        /// <summary>
        /// Add a foreign legalentity applicant to an application.
        /// </summary>
        private Automation.DataModels.LegalEntity AddForeignLegalEntityApplicantToOffer()
        {
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            var legalEntity = Service<ILegalEntityService>().GetNaturalPersonLegalEntity();
            legalEntity.CitizenTypeKey = CitizenTypeEnum.NonResident;
            legalEntity.IdNumber = String.Empty;
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(legalEntity);
            return legalEntity;
        }

        /// <summary>
        /// Add a south african legalentity applicant to an application.
        /// </summary>
        private Automation.DataModels.LegalEntity AddSACitizenLegalEntityApplicantToOffer()
        {
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            var legalEntity = Service<ILegalEntityService>().GetNaturalPersonLegalEntity();
            legalEntity.CitizenTypeKey = CitizenTypeEnum.SACitizen;
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(legalEntity);
            return legalEntity;
        }

        /// <summary>
        /// Select the legal entity in the FloBo
        /// </summary>
        /// <param name="companyName"></param>
        private void SelectCompanyLegalEntity(string companyName)
        {
            //Get LegalEntitkey
            int leKey = Service<ILegalEntityService>().GetLegalEntityKeyByTradeName(companyName);
            //go to the legal entity node
            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(leKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityName"></param>
        private void SelectNaturalLegalEntity(string legalEntityName)
        {
            //Get LegalEntitkey
            int leKey = Service<ILegalEntityService>().GetLegalEntityKeyByFirstNames(legalEntityName);
            //go to the legal entity node
            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(leKey);
        }

        #endregion HelperMethods
    }
}