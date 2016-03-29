using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The LegalEntityBankAccount_DAO class links the Legal Entity to one or more Bank Accounts.
    /// </summary>

    [ActiveRecord("LegalEntityBankAccount", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityBankAccount_DAO : DB_2AM<LegalEntityBankAccount_DAO>
    {
        private BankAccount_DAO _bankAccount;

        private GeneralStatus_DAO _generalStatus;

        private string _userID;

        private System.DateTime? _changeDate;

        private int _key;

        private LegalEntity_DAO _legalEntity;

        /// <summary>
        /// The foreign key reference to the Bank Account table which has the details regarding the Bank Account.
        /// Each LegalEntityBankAccountKey belongs to a single BankAccountKey
        /// </summary>
        [BelongsTo("BankAccountKey", NotNull = true)]
        [ValidateNonEmpty("Bank Account is a mandatory field")]
        public virtual BankAccount_DAO BankAccount
        {
            get
            {
                return this._bankAccount;
            }
            set
            {
                this._bankAccount = value;
            }
        }

        /// <summary>
        /// The status of the record.
        /// </summary>
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
        /// The UserID of the last person who updated information on the LegalEntityBankAccount.
        /// </summary>
        [Property("UserID", ColumnType = "String")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        /// <summary>
        /// The date the record was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ChangeDate
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityBankAccountKey", ColumnType = "Int32")]
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
        /// The foreign key reference to the LegalEntity table where the details of the Legal Entity are stored. Each LegalEntityBankAccountKey
        /// belongs to a single LegalEntityKey.
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