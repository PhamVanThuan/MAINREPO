using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "5", Lazy = true)]
    public class AssetLiabilityLifeAssurance_DAO : AssetLiability_DAO
    {
        private string _companyName;

        private double _surrenderValue;

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

        [Property("AssetValue", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Surrender Value is a mandatory field")]
        public virtual double SurrenderValue
        {
            get
            {
                return this._surrenderValue;
            }
            set
            {
                this._surrenderValue = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLifeAssurance_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityLifeAssurance_DAO>.Find(id).As<AssetLiabilityLifeAssurance_DAO>();
        }

        public new static AssetLiabilityLifeAssurance_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityLifeAssurance_DAO>.Find(id).As<AssetLiabilityLifeAssurance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLifeAssurance_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityLifeAssurance_DAO>.FindFirst().As<AssetLiabilityLifeAssurance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLifeAssurance_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityLifeAssurance_DAO>.FindOne().As<AssetLiabilityLifeAssurance_DAO>();
        }

        #endregion Static Overrides
    }
}