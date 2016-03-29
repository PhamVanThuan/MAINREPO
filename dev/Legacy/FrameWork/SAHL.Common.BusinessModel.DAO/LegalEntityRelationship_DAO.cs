using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The LegalEntityRelationship_DAO class is used in order to store relationships that exist between Legal Entities involved
    /// in SA Home Loans accounts.
    /// </summary>
    [ActiveRecord("LegalEntityRelationship", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityRelationship_DAO : DB_2AM<LegalEntityRelationship_DAO>
    {
        private int _key;

        private LegalEntity_DAO _RealatedlegalEntity;

        private LegalEntity_DAO _legalEntity;

        private LegalEntityRelationshipType_DAO _legalEntityRelationshipType;

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityRelationshipKey", ColumnType = "Int32")]
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
        /// The foreign key reference to the LegalEntity table. Each LegalEntityRelationship belongs to a single LegalEntity.
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

        /// <summary>
        /// The foreign key reference to the LegalEntity table. This is the Legal Entity which that referenced in the Legal Entity
        /// property is related to.
        /// </summary>
        [BelongsTo("RelatedLegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Related Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO RelatedLegalEntity
        {
            get
            {
                return this._RealatedlegalEntity;
            }
            set
            {
                this._RealatedlegalEntity = value;
            }
        }

        /// <summary>
        /// The foreign key reference to to the RelationshipType table. Each LegalEntityRelationship belongs to a single Relationship Type.
        /// </summary>
        [BelongsTo("RelationshipTypeKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity Relationship Type is a mandatory field")]
        public virtual LegalEntityRelationshipType_DAO LegalEntityRelationshipType
        {
            get
            {
                return this._legalEntityRelationshipType;
            }
            set
            {
                this._legalEntityRelationshipType = value;
            }
        }
    }
}