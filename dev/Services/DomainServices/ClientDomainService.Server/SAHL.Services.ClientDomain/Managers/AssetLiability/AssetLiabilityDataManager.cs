using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ClientDomain.Managers.AssetLiability.Statements;
using SAHL.Services.Interfaces.ClientDomain.Models;


namespace SAHL.Services.ClientDomain.Managers.AssetLiability
{
    public class AssetLiabilityDataManager : IAssetLiabilityDataManager
    {
        private IDbFactory dbFactory;
        public AssetLiabilityDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool CheckIsAddressLinkedToClientAsFixedPropertyAsset(int clientKey, int addressKey)
        {
            var addressLinkedAsFixedPropertyQuery = new IsAddressLinkedToClientAsGivenAssetTypeStatement(addressKey, clientKey, AssetLiabilityType.FixedProperty);
            int matchingLinkedAddressesCount;
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                matchingLinkedAddressesCount = db.SelectOne(addressLinkedAsFixedPropertyQuery);
            }
            return matchingLinkedAddressesCount == 1;
        }

        public int SaveFixedPropertyAsset(FixedPropertyAssetModel fixedPropertyAsset)
        {
            var assetLiability = new AssetLiabilityDataModel(
                                          (int)AssetLiabilityType.FixedProperty
                                        , null
                                        , fixedPropertyAsset.AddressKey
                                        , fixedPropertyAsset.AssetValue
                                        , fixedPropertyAsset.LiabilityValue
                                        , null
                                        , null
                                        , fixedPropertyAsset.DateAquired
                                        , null
                                  );

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AssetLiabilityDataModel>(assetLiability);
                db.Complete();
                return assetLiability.AssetLiabilityKey;
            }
        }

        public int LinkAssetLiabilityToClient(int clientKey, int assetLiabilityKey)
        {
            var legalEntityAssetLiability = new LegalEntityAssetLiabilityDataModel(clientKey, assetLiabilityKey, (int)GeneralStatus.Active);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LegalEntityAssetLiabilityDataModel>(legalEntityAssetLiability);
                db.Complete();
                return legalEntityAssetLiability.LegalEntityAssetLiabilityKey;
            }
        }

        public int SaveFixedLongTermInvestmentLiability(FixedLongTermInvestmentLiabilityModel fixedLongTermInvestmentLiability)
        {
            var assetLiability = new AssetLiabilityDataModel(
                                          (int)AssetLiabilityType.FixedLongTermInvestment
                                        , null
                                        , null
                                        , null
                                        , fixedLongTermInvestmentLiability.LiabilityValue
                                        , fixedLongTermInvestmentLiability.CompanyName
                                        , null
                                        , null
                                        , null
                                  );

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AssetLiabilityDataModel>(assetLiability);
                db.Complete();
                return assetLiability.AssetLiabilityKey;
            }
        }

        public int SaveLiabilityLoan(LiabilityLoanModel liabilityLoan)
        {
            var assetLiabilityDataModel = new AssetLiabilityDataModel((int)AssetLiabilityType.LiabilityLoan, (int)liabilityLoan.LoanType, null
            , null, liabilityLoan.LiabilityValue, liabilityLoan.FinancialInstitution, liabilityLoan.InstalmentValue, liabilityLoan.DateRepayable, null);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AssetLiabilityDataModel>(assetLiabilityDataModel);
                db.Complete();
                return assetLiabilityDataModel.AssetLiabilityKey;
            }
        }
        public int SaveOtherAsset(OtherAssetModel otherAsset)
        {
            var assetLiability = new AssetLiabilityDataModel(
                                          (int)AssetLiabilityType.OtherAsset
                                        , null
                                        , null
                                        , otherAsset.AssetValue
                                        , otherAsset.LiabilityValue
                                        , null
                                        , null
                                        , null
                                        , otherAsset.Description
                                  );

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AssetLiabilityDataModel>(assetLiability);
                db.Complete();
                return assetLiability.AssetLiabilityKey;
            }
        }
        public int SaveLifeAssuranceAsset(LifeAssuranceAssetModel lifeAssuranceAsset)
        {
            var assetLiability = new AssetLiabilityDataModel(
                                          (int)AssetLiabilityType.LifeAssurance
                                        , null
                                        , null
                                        , lifeAssuranceAsset.SurrenderValue
                                        , null
                                        , lifeAssuranceAsset.CompanyName
                                        , null
                                        , null
                                        , null
                                  );

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AssetLiabilityDataModel>(assetLiability);
                db.Complete();
                return assetLiability.AssetLiabilityKey;
            }
        }

        public int SaveLiabilitySurety(LiabilitySuretyModel liabilitySuretyModel)
        {
            var assetLiability = new AssetLiabilityDataModel((int)AssetLiabilityType.LiabilitySurety, null, null, 
                liabilitySuretyModel.AssetValue, liabilitySuretyModel.LiabilityValue, null, null, null, liabilitySuretyModel.Description);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AssetLiabilityDataModel>(assetLiability);
                db.Complete();
                return assetLiability.AssetLiabilityKey;
            }
        }

        public int SaveInvestmentAsset(InvestmentAssetModel investmentAsset)
        {
            var assetLiability = new AssetLiabilityDataModel(
                                          (int)investmentAsset.InvestmentType
                                        , null
                                        , null
                                        , investmentAsset.AssetValue
                                        , null
                                        , investmentAsset.CompanyName
                                        , null
                                        , null
                                        , null
                                  );

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AssetLiabilityDataModel>(assetLiability);
                db.Complete();
                return assetLiability.AssetLiabilityKey;
            }
        }
    }
}