using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "2", Lazy = true)]
    public class AssetLiabilityInvestmentPublic_DAO : AssetLiability_DAO
    {
        private string _companyName;

        private double _assetValue;

        [Property("AssetValue", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Asset Value is a mandatory field")]
        public virtual double AssetValue
        {
            get
            {
                return this._assetValue;
            }
            set
            {
                this._assetValue = value;
            }
        }

        [Property("CompanyName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Company Name is a mandatory field")]
        public virtual string CompanyName
        {
            get
            {
                return this._companyName;
            }
            set
            {
                this._companyName = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityInvestmentPublic_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPublic_DAO>.Find(id).As<AssetLiabilityInvestmentPublic_DAO>();
        }

        public new static AssetLiabilityInvestmentPublic_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPublic_DAO>.Find(id).As<AssetLiabilityInvestmentPublic_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityInvestmentPublic_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPublic_DAO>.FindFirst().As<AssetLiabilityInvestmentPublic_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityInvestmentPublic_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPublic_DAO>.FindOne().As<AssetLiabilityInvestmentPublic_DAO>();
        }

        #endregion Static Overrides
    }
}