using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.LegalEntity
{
    [RequiresSTA]
    public sealed class LegalEntityDetailTests : BuildingBlocks.TestBase<LegalEntityDetails>
    {
        private Random randomNumber = new Random();

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        /// <summary>
        ///Find a legalentity with valid exception status and check if it can be updated, if it can't then pass the test
        /// </summary>
        [Test]
        public void CannotUpdateLegalEntityWithValidIDNumberExceptionStatus()
        {
            //Get natural person legalentity where the firstnames and surname was not used before i.e contains the word "Update"
            var naturalLegalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                 (
                     LegalEntityTypeEnum.NaturalPerson,
                     LanguageEnum.English,
                     LegalEntityExceptionStatusEnum.Valid,
                     citizenShipType: CitizenTypeEnum.SACitizen,
                     firstnamesExclusionExpression: "Update",
                     surnameExclusionExpression: "Update"
                 );
            Assert.That(naturalLegalEntity != null, "No legal entity found.");
            Helper.NavigateToClientSuperSearchBasic(base.Browser);

            this.OpenLegalEntityAtLegalEntityUpdate(naturalLegalEntity);

            //Check that the legalentity is 'Really' disabled for update
            var cantEditIdNumber = base.View.IsIDNumberEditable();
            Assert.That(cantEditIdNumber, "LegalEntity was enabled, expected false");
        }

        /// <summary>
        ///Find a legalentity with InvalidIDNumber exception status and enable legalentity update and check if it can be updated, if it can't then fail the test
        /// </summary>
        [Test]
        public void CanUpdateLegalEntityWithInvalidIDNumberExceptionStatus()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                 (
                     LegalEntityTypeEnum.NaturalPerson,
                     LanguageEnum.English,
                     LegalEntityExceptionStatusEnum.InvalidIDNumber,
                     citizenShipType: CitizenTypeEnum.SACitizen,
                     firstnamesExclusionExpression: "Update",
                     surnameExclusionExpression: "Update"
                 );

            Assert.That(legalEntity != null, "No legal entity found.");

            EnableLegalEntityUpdate(legalEntity);
            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            legalEntity = ChangeLegalEntityDetails(legalEntity);
            legalEntity.IdNumber = IDNumbers.GetNextIDNumber();
            UpdateLegalEntity(legalEntity);

            //If the legal entity that we are updating has an invalid idnumber exception status then we need to assert that it changed to valid status
            legalEntity.LegalEntityExceptionStatusKey = LegalEntityExceptionStatusEnum.Valid;
            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntity);
        }

        /// <summary>
        ///Find a legalentity with valid exception status and check if it can be updated, if it can't then pass the test
        /// </summary>
        [Test]
        public void CannotUpdateLegalEntityMaritalStatusWithValidIDNumber()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                  (
                      LegalEntityTypeEnum.NaturalPerson,
                      LanguageEnum.English,
                      LegalEntityExceptionStatusEnum.Valid,
                      citizenShipType: CitizenTypeEnum.SACitizen,
                      firstnamesExclusionExpression: "Update",
                      surnameExclusionExpression: "Update"
                  );

            Assert.That(legalEntity != null, "No legal entity found.");

            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            //Check that the legalentity marital status can't be updated.
            var canEditMaritalStatus = base.View.IsMaritalStatusEditable();
            Assert.That(!canEditMaritalStatus, "LegalEntity marital status was enabled, expected false");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateNaturalPersonLegalEntity()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                  (
                      LegalEntityTypeEnum.NaturalPerson,
                      LanguageEnum.English,
                      LegalEntityExceptionStatusEnum.Valid,
                      citizenShipType: CitizenTypeEnum.SACitizen,
                      firstnamesExclusionExpression: "Update",
                      surnameExclusionExpression: "Update"
                  );
            Assert.That(legalEntity != null, "No legal entity found.");

            EnableLegalEntityUpdate(legalEntity);
            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            legalEntity = ChangeLegalEntityDetails(legalEntity);
            UpdateLegalEntity(legalEntity);

            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntity);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateCompanyLegalEntity()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
               (
                   LegalEntityTypeEnum.Company,
                   LanguageEnum.English,
                   registeredNameExclusionExpression: "Update"
               );
            Assert.That(legalEntity != null, "No legal entity found.");

            EnableLegalEntityUpdate(legalEntity);
            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            legalEntity = ChangeCompanyDetails(legalEntity);
            UpdateLegalEntity(legalEntity);

            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntity);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateTrustLegalEntity()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                (
                    LegalEntityTypeEnum.Trust,
                    LanguageEnum.English,
                    registeredNameExclusionExpression: "Update"
                );
            Assert.That(legalEntity != null, "No legal entity found.");

            EnableLegalEntityUpdate(legalEntity);
            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            legalEntity = ChangeCompanyDetails(legalEntity);
            UpdateLegalEntity(legalEntity);

            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntity);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateCloseCorporationLegalEntity()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                 (
                     LegalEntityTypeEnum.CloseCorporation,
                     LanguageEnum.English,
                     registeredNameExclusionExpression: "Update"
                 );

            Assert.That(legalEntity != null, "No legal entity found.");

            EnableLegalEntityUpdate(legalEntity);
            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            legalEntity = ChangeCompanyDetails(legalEntity);
            UpdateLegalEntity(legalEntity);

            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntity);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateLegalEntityNaturalPersonMandatoryFields()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                (
                    LegalEntityTypeEnum.NaturalPerson,
                    LanguageEnum.English,
                    legalEntityExceptionStatusKey: LegalEntityExceptionStatusEnum.InvalidIDNumber,
                    citizenShipType: CitizenTypeEnum.SACitizen,
                    firstnamesExclusionExpression: "Update",
                    surnameExclusionExpression: "Update"
                );

            Assert.That(legalEntity != null, "No legal entity found.");

            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            //Keep the orignal values
            var legalEntityCopy = legalEntity.Copy();

            legalEntity = ClearLegalEntityProperties(legalEntity);
            UpdateLegalEntity(legalEntity);

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity First Name Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Surname Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("At least one contact detail (Email, Home, Work or Cell Number) is required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Salutation Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Population Group Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Education Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Document Language Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Document Language is a mandatory field");

            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntityCopy);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateLegalEntityNonNaturalPersonMandatoryFields()
        {
            var legalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                 (
                     LegalEntityTypeEnum.Company,
                     LanguageEnum.English,
                     registeredNameExclusionExpression: "Update"
                 );
            Assert.That(legalEntity != null, "No legal entity found.");

            EnableLegalEntityUpdate(legalEntity);
            this.OpenLegalEntityAtLegalEntityUpdate(legalEntity);

            //Keep the orignal values
            var legalEntityCopy = legalEntity.Copy();

            legalEntity = ClearLegalEntityProperties(legalEntity);
            UpdateLegalEntity(legalEntity);

            //NOTE: Need to check why we are not required to Capture Contact details here.
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Company Name Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Registration Number Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("You must supply a Trading Name");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Document Language Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Document Language is a mandatory field");

            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntityCopy);
        }

        #region Helper

        private void OpenLegalEntityAtLegalEntityUpdate(Automation.DataModels.LegalEntity legalEntity)
        {
            //Search navigate and load the legal entity and navigate the the udpate screen screen
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().PopulateSearch(legalEntity);
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByLegalEntityKey(legalEntity.LegalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().ClickLegalEntityDetails();
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdateLegalEntityDetails();
        }

        private void EnableLegalEntityUpdate(Automation.DataModels.LegalEntity legalEntity)
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().PopulateSearch(legalEntity);
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByLegalEntityKey(legalEntity.LegalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().ClickLegalEntityDetails();
            base.Browser.Navigate<LoanServicingCBO>().ClickEnableUpdateLegalEntity();
            base.Browser.Page<LegalEntityEnableUpdateUpdate>().ClickYes();
            //The legalentity exception status should have changed assert the legalentity.
            legalEntity.LegalEntityExceptionStatusKey = LegalEntityExceptionStatusEnum.InvalidIDNumber;
            LegalEntityAssertions.AssertAllLegalEntityDetails(legalEntity);
        }

        private void UpdateLegalEntity(Automation.DataModels.LegalEntity legalEntity)
        {
            base.View.PopulateLegalEntity(legalEntity);
            base.View.ClickUpdate();
        }

        private Automation.DataModels.LegalEntity ClearLegalEntityProperties(Automation.DataModels.LegalEntity legalEntity)
        {
            legalEntity.FirstNames = String.Empty;
            legalEntity.Surname = String.Empty;
            legalEntity.WorkPhoneCode = String.Empty;
            legalEntity.WorkPhoneNumber = String.Empty;
            legalEntity.FaxCode = String.Empty;
            legalEntity.FaxNumber = String.Empty;
            legalEntity.HomePhoneCode = String.Empty;
            legalEntity.HomePhoneNumber = String.Empty;
            legalEntity.PassportNumber = String.Empty;
            legalEntity.CellPhoneNumber = String.Empty;
            legalEntity.PreferredName = String.Empty;
            legalEntity.SalutationDescription = "- Please select -";
            legalEntity.EducationDescription = "- Please select -";
            legalEntity.PopulationGroupDescription = "- Please select -";
            legalEntity.MaritalStatusDescription = "- Please select -";
            legalEntity.EmailAddress = "";
            legalEntity.IdNumber = String.Empty;
            legalEntity.TaxNumber = String.Empty;
            legalEntity.RegisteredName = String.Empty;
            legalEntity.RegistrationNumber = String.Empty;
            legalEntity.TradingName = String.Empty;
            legalEntity.DocumentLanguageDescription = "- Please select -";
            return legalEntity;
        }

        private Automation.DataModels.LegalEntity ChangeLegalEntityDetails(Automation.DataModels.LegalEntity legalEntity)
        {
            //Change some things on the existing legalentity to update with
            legalEntity.Initials = "AAA";
            legalEntity.FirstNames = "Update FirstNames";
            legalEntity.Surname = "Update Surname";
            legalEntity.TaxNumber = "Update TaxNumber";
            legalEntity.PreferredName = "Update PreferredName";
            legalEntity.SalutationKey = SalutationTypeEnum.Dr;
            legalEntity.SalutationDescription = "Dr";
            legalEntity.FaxCode = "000";
            legalEntity.FaxNumber = "1234567";
            legalEntity.HomePhoneCode = "111";
            legalEntity.HomePhoneNumber = "7654321";
            legalEntity.EmailAddress = "NaturalLEUpdate@test.com";
            legalEntity.WorkPhoneCode = "222";
            legalEntity.WorkPhoneNumber = "1236547";
            legalEntity.CellPhoneNumber = "0821234567";
            legalEntity.DocumentLanguageDescription = "Afrikaans";
            legalEntity.DocumentLanguageKey = LanguageEnum.Afrikaans;
            legalEntity.EducationDescription = "Matric";
            legalEntity.EducationKey = EducationEnum.Matric;
            legalEntity.MaritalStatusDescription = "Single";
            legalEntity.MaritalStatusKey = MaritalStatusEnum.Single;
            legalEntity.PopulationGroupDescription = "Asian";
            legalEntity.PopulationGroupKey = PopulationGroupEnum.Asian;
            legalEntity.PreferredName = "PreferredName Update";
            return legalEntity;
        }

        private Automation.DataModels.LegalEntity ChangeCompanyDetails(Automation.DataModels.LegalEntity legalEntity)
        {
            //Change some things on the existing legalentity to update with
            legalEntity.RegisteredName = "Update RegisteredName";
            legalEntity.TradingName = "Update TradingName";
            legalEntity.RegistrationNumber = String.Format("No{0}", this.randomNumber.Next(0, Int32.MaxValue));
            legalEntity.EmailAddress = "CompanyLEUpdate@test.com";

            legalEntity.FaxCode = "111";
            legalEntity.FaxNumber = "7654321";
            legalEntity.WorkPhoneCode = "222";
            legalEntity.WorkPhoneNumber = "1236547";
            legalEntity.CellPhoneNumber = "0821234567";

            legalEntity.TaxNumber = "TaxNumber Update";
            return legalEntity;
        }

        #endregion Helper
    }
}