using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ExternalRole", Schema = "dbo")]
    public partial class ExternalRole_DAO : DB_2AM<ExternalRole_DAO>
    {
        private int _key;

        private int _genericKey;

        private ExternalRoleType_DAO _externalRoleType;

        private System.DateTime _changeDate;

        private GeneralStatus_DAO _generalStatus;

        private GenericKeyType_DAO _genericKeyType;

        private LegalEntity_DAO _legalEntity;

        private IList<ExternalRoleDeclaration_DAO> _externalRoleDeclarations;

        private IList<ExternalRoleDomicilium_DAO> _externalRoleDomiciliums;

        [PrimaryKey(PrimaryKeyType.Native, "ExternalRoleKey", ColumnType = "Int32")]
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        /// <summary>
        /// The date when the External Role record was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [BelongsTo("ExternalRoleTypeKey", NotNull = true)]
        public virtual ExternalRoleType_DAO ExternalRoleType
        {
            get
            {
                return this._externalRoleType;
            }
            set
            {
                this._externalRoleType = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
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

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
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
        /// A collection of external role declaration that are defined for this ExternalRole.
        /// </summary>
        [HasMany(typeof(ExternalRoleDeclaration_DAO), Table = "ExternalRoleDeclaration", ColumnKey = "ExternalRoleKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ExternalRoleDeclaration_DAO> ExternalRoleDeclarations
        {
            get { return _externalRoleDeclarations; }
            set { _externalRoleDeclarations = value; }
        }

        [Lurker]
        [HasMany(typeof(ExternalRoleDomicilium_DAO), ColumnKey = "ExternalRoleKey", Table = "ExternalRoleDomicilium", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
        public virtual IList<ExternalRoleDomicilium_DAO> ExternalRoleDomiciliums
        {
            get
            {
                return this._externalRoleDomiciliums;
            }
            set
            {
                this._externalRoleDomiciliums = value;
            }
        }
    }
}