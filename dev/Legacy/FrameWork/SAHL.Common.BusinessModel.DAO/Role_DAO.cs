using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Role_DAO is used in order to link an SAHL Account to the Legal Entities which play a role in the Account. This relationship
    /// is defined in the AccountRole table.
    /// </summary>
    [ActiveRecord("Role", Schema = "dbo", Lazy = true)]
    public partial class Role_DAO : DB_2AM<Role_DAO>
    {
        private System.DateTime _statusChangeDate;

        private int _accountRoleKey;

        private Account_DAO _account;

        private GeneralStatus_DAO _generalStatus;

        private LegalEntity_DAO _legalEntity;

        private RoleType_DAO _roleType;

        /// <summary>
        /// The date on which the status of the Role was changed.
        /// </summary>
        [Property("StatusChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Status Change Date is a mandatory field")]
        public virtual System.DateTime StatusChangeDate
        {
            get
            {
                return this._statusChangeDate;
            }
            set
            {
                this._statusChangeDate = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "AccountRoleKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._accountRoleKey;
            }
            set
            {
                this._accountRoleKey = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the Account table. Each AccountRole that is defined belongs to an AccountKey.
        /// </summary>
        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Account is a mandatory field")]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the GeneralStatus table. Each AccountRole has a status e.g. active/inactive.
        /// </summary>
        ///
        [Lurker]
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
        ///  The foreign key reference to the LegalEntity table. Each AccountRole that is defined belongs to an LegalEntityKey.
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
        /// The foreign key reference to the RoleType table. Each AccountRole that is defined belongs to an RoleTypeKey.
        /// </summary>
        ///
        [Lurker]
        [BelongsTo("RoleTypeKey", NotNull = true)]
        [ValidateNonEmpty("Role Type is a mandatory field")]
        public virtual RoleType_DAO RoleType
        {
            get
            {
                return this._roleType;
            }
            set
            {
                this._roleType = value;
            }
        }
    }
}