using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord(DiscriminatorValue = "8", Lazy = true)]
    public class AssetLiabilityFixedLongTermInvestment_DAO : AssetLiability_DAO
    {
        private double _liabilityValue;

        private string _companyName;

        [Property("LiabilityValue", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Liability Value is a mandatory field")]
        public virtual double LiabilityValue
        {
            get
            {
                return this._liabilityValue;
            }
            set
            {
                this._liabilityValue = value;
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
        public static AssetLiabilityFixedLongTermInvestment_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityFixedLongTermInvestment_DAO>.Find(id).As<AssetLiabilityFixedLongTermInvestment_DAO>();
        }

        public new static AssetLiabilityFixedLongTermInvestment_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityFixedLongTermInvestment_DAO>.Find(id).As<AssetLiabilityFixedLongTermInvestment_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityFixedLongTermInvestment_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityFixedLongTermInvestment_DAO>.FindFirst().As<AssetLiabilityFixedLongTermInvestment_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityFixedLongTermInvestment_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityFixedLongTermInvestment_DAO>.FindOne().As<AssetLiabilityFixedLongTermInvestment_DAO>();
        }

        #endregion Static Overrides
    }
}