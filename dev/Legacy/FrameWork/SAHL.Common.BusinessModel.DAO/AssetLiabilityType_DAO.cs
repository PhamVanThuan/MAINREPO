using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("AssetLiabilityType", Schema = "dbo", Lazy = true)]
    public partial class AssetLiabilityType_DAO : DB_2AM<AssetLiabilityType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<AssetLiability_DAO> _assetLiabilities;

        private IList<AssetLiabilitySubType_DAO> _assetLiabilitySubTypes;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "AssetLiabilityTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(AssetLiability_DAO), ColumnKey = "AssetLiabilityTypeKey", Table = "AssetLiability")]
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

        [HasMany(typeof(AssetLiabilitySubType_DAO), ColumnKey = "AssetLiabilityTypeKey", Table = "AssetLiabilitySubType", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<AssetLiabilitySubType_DAO> AssetLiabilitySubTypes
        {
            get
            {
                return this._assetLiabilitySubTypes;
            }
            set
            {
                this._assetLiabilitySubTypes = value;
            }
        }
    }
}