using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "3", Lazy = true)]
    public class AssetLiabilityInvestmentPrivate_DAO : AssetLiability_DAO
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
        public static AssetLiabilityInvestmentPrivate_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPrivate_DAO>.Find(id).As<AssetLiabilityInvestmentPrivate_DAO>();
        }

        public new static AssetLiabilityInvestmentPrivate_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPrivate_DAO>.Find(id).As<AssetLiabilityInvestmentPrivate_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityInvestmentPrivate_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPrivate_DAO>.FindFirst().As<AssetLiabilityInvestmentPrivate_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityInvestmentPrivate_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityInvestmentPrivate_DAO>.FindOne().As<AssetLiabilityInvestmentPrivate_DAO>();
        }

        #endregion Static Overrides
    }
}