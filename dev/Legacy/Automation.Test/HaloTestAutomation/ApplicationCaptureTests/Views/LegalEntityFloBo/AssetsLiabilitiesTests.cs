using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.LegalEntity;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    [TestFixture, RequiresSTA]
    public class AssetsLiabilitiesTest : ApplicationCaptureTests.TestBase<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>
    {
        #region PrivateVar

        private TestBrowser browser;
        private int _offerkey;
        private string _legalentityIdNumber;
        private string _relatedlegalentityIdNumber;
        private Automation.DataModels.LegalEntityAssetLiabilities _assetAndLiabilityAddParameters;
        private Automation.DataModels.LegalEntityAssetLiabilities _assetAndLiabilityChangedParameters;

        #endregion PrivateVar

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            string _consultant = TestUsers.BranchConsultant10;
            Helper.CreateApplication(ref browser, _consultant, OfferTypeEnum.Refinance, out _offerkey, out _legalentityIdNumber);
            Helper.AddNaturalPersonApplicantToOffer(this.browser, this._offerkey, out _relatedlegalentityIdNumber, false);

            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityByIdNumber(this._legalentityIdNumber);

            var randomNumber = new Random(System.Int32.MaxValue);

            _assetAndLiabilityAddParameters = new Automation.DataModels.LegalEntityAssetLiabilities
            {
                Address = new Automation.DataModels.Address
                {
                    UnitNumber = "1",
                    BuildingNumber = "1",
                    BuildingName = String.Format("AssetsAndLiability {0}", randomNumber.Next()),
                    StreetNumber = "12",
                    StreetName = "FixedProperty",
                    RRR_ProvinceDescription = "North West",
                    RRR_SuburbDescription = "Fanie"
                },
                AssetValue = 500000f,
                LiabilityValue = 500000f,
                AssetLiabilitySubTypeKey = AssetLiabilitySubTypeEnum.PersonalLoan,
                OtherDescription = "Other",
                DateRepayable = DateTime.Now,
                CompanyName = "AssetsAndLiability Company",
                DateAcquired = DateTime.Now
            };
            //Set up AssetAndLiabilities update parameters
            _assetAndLiabilityChangedParameters = new Automation.DataModels.LegalEntityAssetLiabilities
            {
                Address = new Automation.DataModels.Address
                {
                    UnitNumber = _assetAndLiabilityAddParameters.Address.UnitNumber,
                    BuildingNumber = _assetAndLiabilityAddParameters.Address.BuildingNumber,
                    BuildingName = _assetAndLiabilityAddParameters.Address.BuildingName,
                    StreetNumber = _assetAndLiabilityAddParameters.Address.StreetNumber,
                    StreetName = _assetAndLiabilityAddParameters.Address.StreetName,
                    RRR_ProvinceDescription = _assetAndLiabilityAddParameters.Address.RRR_ProvinceDescription,
                    RRR_SuburbDescription = _assetAndLiabilityAddParameters.Address.RRR_SuburbDescription
                },
                AssetValue = 200000f,
                LiabilityValue = 900000f,
                AssetLiabilitySubTypeKey = AssetLiabilitySubTypeEnum.PersonalLoan,
                OtherDescription = "Updated Description",
                DateRepayable = DateTime.Parse("10/10/2005"),
                CompanyName = "AssetsAndLiability Company Updated",
                DateAcquired = DateTime.Now
            };
        }

        #region Tests

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void _01_AddAssetsAndLiabilities()
        {
            browser.Navigate<LegalEntityNode>().AssetsAndLiabilities(NodeTypeEnum.Add);
            //Test that every assetliability type can be addded.
            foreach (AssetLiabilityTypeEnum type in Enum.GetValues(typeof(AssetLiabilityTypeEnum)))
            {
                if (type != AssetLiabilityTypeEnum.None)
                {
                    this._assetAndLiabilityAddParameters.AssetLiabilityTypeKey = type;
                    browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>().AddAssetsAndLiabilities(this._assetAndLiabilityAddParameters);
                    LegalEntityAssertions.AssertAssetsAndLiabilities(this._legalentityIdNumber, this._assetAndLiabilityAddParameters, false);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void _02_UpdateAssetsAndLiabilities()
        {
            //Test that every assetliability type can be updated.
            foreach (AssetLiabilityTypeEnum type in Enum.GetValues(typeof(AssetLiabilityTypeEnum)))
            {
                if (type != AssetLiabilityTypeEnum.None)
                {
                    browser.Navigate<LegalEntityNode>().AssetsAndLiabilities(NodeTypeEnum.Update);
                    this._assetAndLiabilityChangedParameters.AssetLiabilityTypeKey = type;
                    browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>().UpdateAssetsAndLiabilities(this._assetAndLiabilityChangedParameters);
                    LegalEntityAssertions.AssertAssetsAndLiabilities(this._legalentityIdNumber, this._assetAndLiabilityChangedParameters, false);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void _03_AssociateAssetsAndLiabilitiesNonCOPMarriage()
        {
            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityByIdNumber(this._relatedlegalentityIdNumber);
            //Assert fixedproperty only on the assetliability associate screen.
            browser.Navigate<LegalEntityNode>().AssetsAndLiabilities(NodeTypeEnum.Associate);
            browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>().AssertOnlyFixedPropertyAvailable();
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void _04_AssociateAssetsAndLiabilitiesCOPMarriage()
        {
            //Goto legal entity and related legal entity and update maritalstatus to COP
            UpdateLegalEntity(this._legalentityIdNumber, MaritalStatusEnum.MarriedCommunityOfProperty);
            UpdateLegalEntity(this._relatedlegalentityIdNumber, MaritalStatusEnum.MarriedCommunityOfProperty);

            //Setup relationships after maritalstatus update
            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityByIdNumber(_legalentityIdNumber);
            Helper.SetupSelectedLegalEntityRelationship(this.browser, this._relatedlegalentityIdNumber, RelationshipType.Spouse);
            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityByIdNumber(_relatedlegalentityIdNumber);
            Helper.SetupSelectedLegalEntityRelationship(this.browser, this._legalentityIdNumber, RelationshipType.Spouse);

            //Test that when married COP that every assetliability against the related legalentity can be associated.
            foreach (AssetLiabilityTypeEnum type in Enum.GetValues(typeof(AssetLiabilityTypeEnum)))
            {
                if (type != AssetLiabilityTypeEnum.None)
                {
                    browser.Navigate<LegalEntityNode>().AssetsAndLiabilities(NodeTypeEnum.Associate);
                    this._assetAndLiabilityChangedParameters.AssetLiabilityTypeKey = type;
                    browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>().AssociateAssetsLiability(type);
                    LegalEntityAssertions.AssertAssetsAndLiabilities(this._relatedlegalentityIdNumber, this._assetAndLiabilityChangedParameters, false);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void _05_DeleteAssetsAndLiabilities()
        {
            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityByIdNumber(this._legalentityIdNumber);
            //Delete every associated assetliability type in against the legal entity.
            foreach (AssetLiabilityTypeEnum type in Enum.GetValues(typeof(AssetLiabilityTypeEnum)))
            {
                if (type != AssetLiabilityTypeEnum.None)
                {
                    browser.Navigate<LegalEntityNode>().AssetsAndLiabilities(NodeTypeEnum.Delete);
                    this._assetAndLiabilityChangedParameters.AssetLiabilityTypeKey = type;
                    browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>().DeleteAssetsAndLiabilities(this._assetAndLiabilityChangedParameters);
                    LegalEntityAssertions.AssertAssetsAndLiabilities(this._legalentityIdNumber, this._assetAndLiabilityChangedParameters, true);
                }
            }
        }

        #endregion Tests

        #region Helpers

        private void UpdateLegalEntity(string leIdnumber, MaritalStatusEnum maritalstatus)
        {
            //Goto legal entity detail and update maritalstatus to ANC
            browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityByIdNumber(leIdnumber);
            browser.Navigate<LegalEntityNode>().LegalEntityDetails(NodeTypeEnum.Update);
            this.browser.Page<LegalEntityDetails>().Update(maritalstatus, LanguageEnum.English);
        }

        #endregion Helpers
    }
}