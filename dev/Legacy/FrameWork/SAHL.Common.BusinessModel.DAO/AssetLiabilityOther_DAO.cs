using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "4", Lazy = true)]
    public class AssetLiabilityOther_DAO : AssetLiability_DAO
    {
        private double _assetValue;

        private string _description;

        private double _liabilityValue;

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
        public static AssetLiabilityOther_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityOther_DAO>.Find(id).As<AssetLiabilityOther_DAO>();
        }

        public new static AssetLiabilityOther_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityOther_DAO>.Find(id);
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityOther_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityOther_DAO>.FindFirst().As<AssetLiabilityOther_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityOther_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityOther_DAO>.FindOne().As<AssetLiabilityOther_DAO>();
        }

        #endregion Static Overrides
    }
}