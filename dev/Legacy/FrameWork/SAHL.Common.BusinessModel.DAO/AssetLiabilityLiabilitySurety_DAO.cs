using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "7", Lazy = true)]
    public class AssetLiabilityLiabilitySurety_DAO : AssetLiability_DAO
    {
        private double? _assetValue;

        private string _description;

        private double? _liabilityValue;

        [Property("AssetValue", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Asset Value is a mandatory field")]
        public virtual double? AssetValue
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

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [Property("LiabilityValue", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Liability Value is a mandatory field")]
        public virtual double? LiabilityValue
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

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLiabilitySurety_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityLiabilitySurety_DAO>.Find(id).As<AssetLiabilityLiabilitySurety_DAO>();
        }

        public new static AssetLiabilityLiabilitySurety_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityLiabilitySurety_DAO>.Find(id).As<AssetLiabilityLiabilitySurety_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLiabilitySurety_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityLiabilitySurety_DAO>.FindFirst().As<AssetLiabilityLiabilitySurety_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLiabilitySurety_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityLiabilitySurety_DAO>.FindOne().As<AssetLiabilityLiabilitySurety_DAO>();
        }

        #endregion Static Overrides
    }
}