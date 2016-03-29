using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The LegalEntityAssetLiability_DAO class links the entries in the AssetLiability table to the LegalEntity
    /// </summary>
    [ActiveRecord("LegalEntityAssetLiability", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityAssetLiability_DAO : DB_2AM<LegalEntityAssetLiability_DAO>
    {
        private AssetLiability_DAO _assetLiability;

        private GeneralStatus_DAO _generalStatus;

        private int _key;

        private LegalEntity_DAO _legalEntity;

        /// <summary>
        /// The foreign key reference to the AssetLiability table where the information regarding the Asset/Liability is stored. Each
        /// LegalEntityAssetLiabilityKey belongs to a single AssetLiabilityKey.
        /// </summary>
        [BelongsTo("AssetLiabilityKey", NotNull = true, Cascade = CascadeEnum.SaveUpdate)]
        [ValidateNonEmpty("Asset/Liability is a mandatory field")]
        public virtual AssetLiability_DAO AssetLiability
        {
            get
            {
                return this._assetLiability;
            }
            set
            {
                this._assetLiability = value;
            }
        }

        /// <summary>
        /// The status of the record.
        /// </summary>
        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        /// <summary>
        /// Primary Key.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityAssetLiabilityKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the LegalEntity table where the information regarding the Legal Entity is stored. Each
        /// LegalEntityAssetLiabilityKey belongs to a single LegalEntityKey.
        /// </summary>
        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }
    }
}