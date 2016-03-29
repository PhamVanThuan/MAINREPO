using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AssetLiabilitySubType", Schema = "dbo")]
    public partial class AssetLiabilitySubType_DAO : DB_2AM<AssetLiabilitySubType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<AssetLiability_DAO> _assetLiabilities;

        private AssetLiabilityType_DAO _assetLiabilityType;

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

        [PrimaryKey(PrimaryKeyType.Native, "AssetLiabilitySubTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        // commented, this is a lookup.
        //[HasMany(typeof(AssetLiability_DAO), ColumnKey = "AssetLiabilitySubTypeKey", Table = "AssetLiability")]
        //public virtual IList<AssetLiability_DAO> AssetLiabilities
        //{
        //    get
        //    {
        //        return this._assetLiabilities;
        //    }
        //    set
        //    {
        //        this._assetLiabilities = value;
        //    }
        //}

        [BelongsTo("AssetLiabilityTypeKey", NotNull = true)]
        [ValidateNonEmpty("Asset Liability Type is a mandatory field")]
        public virtual AssetLiabilityType_DAO AssetLiabilityType
        {
            get
            {
                return this._assetLiabilityType;
            }
            set
            {
                this._assetLiabilityType = value;
            }
        }
    }
}