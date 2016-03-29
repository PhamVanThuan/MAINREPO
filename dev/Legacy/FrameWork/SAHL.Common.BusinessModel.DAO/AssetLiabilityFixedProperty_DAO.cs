using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "1", Lazy = true)]
    public class AssetLiabilityFixedProperty_DAO : AssetLiability_DAO
    {
        private System.DateTime? _dateAcquired;

        private Address_DAO _address;

        private double _assetValue;

        private double _liabilityValue;

        [Property("Date", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Date Acquired is a mandatory field")]
        public virtual System.DateTime? DateAcquired
        {
            get
            {
                return this._dateAcquired;
            }
            set
            {
                this._dateAcquired = value;
            }
        }

        [BelongsTo("AddressKey", NotNull = true)]
        [ValidateNonEmpty("Address is a mandatory field")]
        public virtual Address_DAO Address
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }

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

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityFixedProperty_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityFixedProperty_DAO>.Find(id).As<AssetLiabilityFixedProperty_DAO>();
        }

        public new static AssetLiabilityFixedProperty_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityFixedProperty_DAO>.Find(id).As<AssetLiabilityFixedProperty_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityFixedProperty_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityFixedProperty_DAO>.FindFirst().As<AssetLiabilityFixedProperty_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityFixedProperty_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityFixedProperty_DAO>.FindOne().As<AssetLiabilityFixedProperty_DAO>();
        }

        #endregion Static Overrides
    }
}