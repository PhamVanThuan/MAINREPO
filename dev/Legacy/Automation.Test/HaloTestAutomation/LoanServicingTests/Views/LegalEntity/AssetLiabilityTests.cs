using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.LegalEntity
{
    [RequiresSTA]
    public class LegalEntityAssetLiabilityTests : BuildingBlocks.TestBase<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        [Test]
        public void TestAssetLiabilityUpdateSave()
        {
            var legalentitykey = 0;
            var assetLiabilityKey = 0;
            try
            {
                var naturalPersonLegalEntity
                    = Service<ILegalEntityService>().GetRandomLegalEntityRecord(LegalEntityTypeEnum.NaturalPerson);
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(naturalPersonLegalEntity.LegalEntityKey, 0);
                legalentitykey = naturalPersonLegalEntity.LegalEntityKey;

                //Inserts
                var newLegalEntityAssetLiability = SetupLegalEntityAssetLiabilityModel(naturalPersonLegalEntity);
                var insertedAssetLiabilityLegalEntity = base.Service<ILegalEntityService>().InsertLegalEntityAssetLiability(newLegalEntityAssetLiability);
                assetLiabilityKey = insertedAssetLiabilityLegalEntity.AssetLiabilityKey;

                LoadLegalEntityAtAssetLiabilities(naturalPersonLegalEntity, NodeTypeEnum.Update);

                //Change some things on the assetliaiblity so that we can capture, save and assert
                insertedAssetLiabilityLegalEntity.CompanyName = "AutomationTest Company Update";
                base.View.SelectAssetLiabilityRowByType(insertedAssetLiabilityLegalEntity.AssetLiabilityTypeKey);
                base.View.PopulateAssetLiabilityView(insertedAssetLiabilityLegalEntity);
                base.View.ClickUpdate();

                LegalEntityAssertions.AssertAssetsAndLiabilities
                    (naturalPersonLegalEntity.IdNumber, insertedAssetLiabilityLegalEntity, false);
            }
            finally
            {
                //Cleanup
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(legalentitykey, assetLiabilityKey);
            }
        }

        [Test]
        public void TestAssetLiabilityDelete()
        {
            var legalentitykey = 0;
            var assetLiabilityKey = 0;
            try
            {
                var naturalPersonLegalEntity
                      = Service<ILegalEntityService>().GetRandomLegalEntityRecord(LegalEntityTypeEnum.NaturalPerson);
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(naturalPersonLegalEntity.LegalEntityKey, 0);
                legalentitykey = naturalPersonLegalEntity.LegalEntityKey;

                //Inserts
                var newLegalEntityAssetLiability = SetupLegalEntityAssetLiabilityModel(naturalPersonLegalEntity);
                var insertedAssetLiabilityLegalEntity = base.Service<ILegalEntityService>().InsertLegalEntityAssetLiability(newLegalEntityAssetLiability);
                assetLiabilityKey = insertedAssetLiabilityLegalEntity.AssetLiabilityKey;

                LoadLegalEntityAtAssetLiabilities(naturalPersonLegalEntity, NodeTypeEnum.Delete);

                base.View.SelectAssetLiabilityRowByType(insertedAssetLiabilityLegalEntity.AssetLiabilityTypeKey);
                base.View.ClickDelete();

                insertedAssetLiabilityLegalEntity.GeneralStatusKey = GeneralStatusEnum.Inactive;
                LegalEntityAssertions.AssertAssetsAndLiabilities(naturalPersonLegalEntity.IdNumber, insertedAssetLiabilityLegalEntity, true);
            }
            finally
            {
                //Cleanup
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(legalentitykey, assetLiabilityKey);
            }
        }

        [Test]
        public void AssetLiability_WhenUpdatingAssetLiability_ShouldNotDisplayInactiveAssetLiabilities()
        {
            var legalentitykey = 0;
            var assetLiabilityKey = 0;
            try
            {
                var naturalPersonLegalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord(LegalEntityTypeEnum.NaturalPerson);
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(naturalPersonLegalEntity.LegalEntityKey, 0);
                legalentitykey = naturalPersonLegalEntity.LegalEntityKey;

                //Inserts
                var newLegalEntityAssetLiability = SetupLegalEntityAssetLiabilityModel(naturalPersonLegalEntity, legalentityAssetLiabilityStatus: GeneralStatusEnum.Inactive);
                var insertedAssetLiabilityLegalEntity = base.Service<ILegalEntityService>().InsertLegalEntityAssetLiability(newLegalEntityAssetLiability);
                assetLiabilityKey = insertedAssetLiabilityLegalEntity.AssetLiabilityKey;

                LoadLegalEntityAtAssetLiabilities(naturalPersonLegalEntity, NodeTypeEnum.Update);

                base.View.AssertInactiveAssetLiabilityNotDisplay(insertedAssetLiabilityLegalEntity);
            }
            finally
            {
                //Cleanup
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(legalentitykey, assetLiabilityKey);
            }
        }

        #region AssetLiabilityAddControlTests

        [Test]
        public void AssetLiabilityFixedPropertyAddControlTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.FixedProperty, Common.Constants.AssetLiabilityType.FixedProperty);
        }

        [Test]
        public void AssetLiabilityFixedLongTermInvestmentAddControlTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.FixedLongTermInvestment, Common.Constants.AssetLiabilityType.FixedLongTermInvestment);
        }

        [Test]
        public void AssetLiabilityLiabilityLoanAddControlTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.LiabilityLoan, Common.Constants.AssetLiabilityType.LiabilityLoan);
        }

        [Test]
        public void AssetLiabilityLiabilitySuretyAddControlTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.LiabilitySurety, Common.Constants.AssetLiabilityType.LiabilitySurety);
        }

        [Test]
        public void AssetLiabilityLifeAssuranceAddControlTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.LifeAssurance, Common.Constants.AssetLiabilityType.LifeAssurance);
        }

        [Test]
        public void AssetLiabilityListedInvestmentsAddControlTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.ListedInvestments, Common.Constants.AssetLiabilityType.ListedInvestments);
        }

        [Test]
        public void AssetLiabilityOtherAssetUpdateAddTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.OtherAsset, Common.Constants.AssetLiabilityType.OtherAsset);
        }

        [Test]
        public void AssetLiabilityUnlistedInvestmentstAddControlTests()
        {
            AssetLiabilityAddControlTests(AssetLiabilityTypeEnum.UnlistedInvestments, Common.Constants.AssetLiabilityType.UnlistedInvestments);
        }

        #endregion AssetLiabilityAddControlTests

        #region AssetLiabilityUpdateControlTests

        [Test]
        public void AssetLiabilityFixedPropertyUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.FixedProperty);
        }

        [Test]
        public void AssetLiabilityFixedLongTermInvestmentUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.FixedLongTermInvestment);
        }

        [Test]
        public void AssetLiabilityLiabilityLoanUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.LiabilityLoan);
        }

        [Test]
        public void AssetLiabilityLiabilitySuretyUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.LiabilitySurety);
        }

        [Test]
        public void AssetLiabilityLifeAssuranceUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.LifeAssurance);
        }

        [Test]
        public void AssetLiabilityListedInvestmentsUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.ListedInvestments);
        }

        [Test]
        public void AssetLiabilityOtherAssetUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.OtherAsset);
        }

        [Test]
        public void AssetLiabilityUnlistedInvestmentstUpdateControlTests()
        {
            AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum.UnlistedInvestments);
        }

        #endregion AssetLiabilityUpdateControlTests

        #region Helper

        private void AssetLiabilityUpdateControlTests(AssetLiabilityTypeEnum assetLiabilityType)
        {
            var legalentitykey = 0;
            var assetLiabilityKey = 0;
            try
            {
                var naturalPersonLegalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord(LegalEntityTypeEnum.NaturalPerson);

                //Scrap all the assets liabilities
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(naturalPersonLegalEntity.LegalEntityKey, 0);
                legalentitykey = naturalPersonLegalEntity.LegalEntityKey;

                var newLegalEntityAssetLiability = SetupLegalEntityAssetLiabilityModel(naturalPersonLegalEntity, assetLiabilityTypeKey: assetLiabilityType);

                var insertedAssetLiabilityLegalEntity = base.Service<ILegalEntityService>().InsertLegalEntityAssetLiability(newLegalEntityAssetLiability);
                assetLiabilityKey = insertedAssetLiabilityLegalEntity.AssetLiabilityKey;

                //Load the legalentity
                LoadLegalEntityAtAssetLiabilities(naturalPersonLegalEntity, NodeTypeEnum.Update);

                base.View.SelectAssetLiabilityRowByType(assetLiabilityType);
                base.View.AssertControlsMatchAssetLiability(assetLiabilityType, expectedLegalEntityAssetLiability: insertedAssetLiabilityLegalEntity);
                base.View.AssertButtonsExist(isAddUpdateMode: true);
            }
            finally
            {
                //Cleanup
                base.Service<ILegalEntityService>().DeleteLegalEntityAssetLiabilities(legalentitykey, assetLiabilityKey);
            }
        }

        private void AssetLiabilityAddControlTests(AssetLiabilityTypeEnum assetLiabilityTypeEnum, string assetLiabilityType)
        {
            var naturalPersonLegalEntity = Service<ILegalEntityService>().GetRandomLegalEntityRecord(LegalEntityTypeEnum.NaturalPerson);

            //Load the legalentity
            LoadLegalEntityAtAssetLiabilities(naturalPersonLegalEntity, NodeTypeEnum.Add);

            base.View.SelectAssetLiabilityType(assetLiabilityType);
            base.View.AssertControlsMatchAssetLiability(assetLiabilityTypeEnum);
            base.View.AssertButtonsExist(isAddUpdateMode: true);
        }

        private void LoadLegalEntityAtAssetLiabilities(Automation.DataModels.LegalEntity legalEntity, NodeTypeEnum nodeType)
        {
            //Search and load the legal entity and navigate the the udpate screen screen
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().PopulateSearch(legalEntity);
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByLegalEntityKey(legalEntity.LegalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().AssetsLiabilities(nodeType);
        }

        private Automation.DataModels.LegalEntityAssetLiabilities SetupLegalEntityAssetLiabilityModel(Automation.DataModels.LegalEntity legalentity,
                AssetLiabilityTypeEnum assetLiabilityTypeKey = AssetLiabilityTypeEnum.ListedInvestments,
                GeneralStatusEnum legalentityAssetLiabilityStatus = GeneralStatusEnum.Active)
        {
            var newLegalEntityAssetLiability = new Automation.DataModels.LegalEntityAssetLiabilities();
            newLegalEntityAssetLiability.LegalEntityKey = legalentity.LegalEntityKey;
            newLegalEntityAssetLiability.GeneralStatusKey = legalentityAssetLiabilityStatus;
            newLegalEntityAssetLiability.AssetLiabilityTypeKey = assetLiabilityTypeKey;
            newLegalEntityAssetLiability.Date = DateTime.Parse(DateTime.Now.ToString(Formats.DateTimeFormatSQL));
            switch (assetLiabilityTypeKey)
            {
                case AssetLiabilityTypeEnum.FixedLongTermInvestment:
                    {
                        newLegalEntityAssetLiability.LiabilityValue = 60000f;
                        newLegalEntityAssetLiability.CompanyName = "AutomationTest Company";
                        return newLegalEntityAssetLiability;
                    }
                case AssetLiabilityTypeEnum.FixedProperty:
                    {
                        var legalentityaddress = Service<ILegalEntityAddressService>().GetRandomLegalEntityAddress(legalentityIdnumber: legalentity.IdNumber);
                        newLegalEntityAssetLiability.Address = legalentityaddress.Address;
                        newLegalEntityAssetLiability.AddressKey = legalentityaddress.Address.AddressKey;
                        newLegalEntityAssetLiability.LiabilityValue = 60000f;
                        newLegalEntityAssetLiability.AssetValue = 50000f;
                        return newLegalEntityAssetLiability;
                    }
                case AssetLiabilityTypeEnum.LiabilityLoan:
                    {
                        newLegalEntityAssetLiability.CompanyName = "Financial Institute Test";
                        newLegalEntityAssetLiability.LiabilityValue = 60000f;
                        newLegalEntityAssetLiability.AssetLiabilitySubTypeKey = AssetLiabilitySubTypeEnum.PersonalLoan;
                        return newLegalEntityAssetLiability;
                    }
                case AssetLiabilityTypeEnum.LiabilitySurety:
                    {
                        newLegalEntityAssetLiability.LiabilityValue = 60000f;
                        return newLegalEntityAssetLiability;
                    }
                case AssetLiabilityTypeEnum.LifeAssurance:
                    {
                        newLegalEntityAssetLiability.CompanyName = "AutomationTest Company";
                        return newLegalEntityAssetLiability;
                    }
                case AssetLiabilityTypeEnum.ListedInvestments:
                    {
                        newLegalEntityAssetLiability.AssetValue = 50000f;
                        newLegalEntityAssetLiability.CompanyName = "AutomationTest Company";
                        return newLegalEntityAssetLiability;
                    }
                case AssetLiabilityTypeEnum.OtherAsset:
                    {
                        newLegalEntityAssetLiability.LiabilityValue = 60000f;
                        newLegalEntityAssetLiability.AssetValue = 50000f;
                        return newLegalEntityAssetLiability;
                    }
                case AssetLiabilityTypeEnum.UnlistedInvestments:
                    {
                        newLegalEntityAssetLiability.AssetValue = 50000f;
                        newLegalEntityAssetLiability.CompanyName = "AutomationTest Company";
                        return newLegalEntityAssetLiability;
                    }
                default:
                    return null;
            }
        }

        #endregion Helper
    }
}