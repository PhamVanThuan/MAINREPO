using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("AssetLiability", Schema = "dbo", DiscriminatorColumn = "AssetLiabilityTypeKey", DiscriminatorType = "Int32", DiscriminatorValue = "0", Lazy = true)]
    [HideBaseClass]
    public partial class AssetLiability_DAO : DB_2AM<AssetLiability_DAO>
    {
        private int _Key;

        // private IList<LegalEntityAssetLiability_DAO> _legalEntityAssetLiabilities;

        [PrimaryKey(PrimaryKeyType.Native, "AssetLiabilityKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(LegalEntityAssetLiability_DAO), ColumnKey = "AssetLiabilityKey", Table = "LegalEntityAssetLiability", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        //public virtual IList<LegalEntityAssetLiability_DAO> LegalEntityAssetLiabilities
        //{
        //    get
        //    {
        //        return this._legalEntityAssetLiabilities;
        //    }
        //    set
        //    {
        //        this._legalEntityAssetLiabilities = value;
        //    }
        //}
    }
}