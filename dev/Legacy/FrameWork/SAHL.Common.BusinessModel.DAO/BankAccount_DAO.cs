using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BankAccount", Schema = "dbo", Lazy = true)]
    public class BankAccount_DAO : DB_2AM<BankAccount_DAO>
    {
        private ACBBranch_DAO _aCBBranch;

        private string _accountNumber;

        private ACBType_DAO _aCBType;

        private string _accountName;

        private string _userID;

        private System.DateTime? _changeDate;

        private int _key;

        [BelongsTo(Column = "ACBBranchCode", NotNull = true)]
        [ValidateNonEmpty("ACB Branch is a mandatory field")]
        public virtual ACBBranch_DAO ACBBranch
        {
            get
            {
                return this._aCBBranch;
            }
            set
            {
                this._aCBBranch = value;
            }
        }

        [Property("AccountNumber", ColumnType = "String", NotNull = true, Length = 25)]
        [ValidateNonEmpty("Account Number is a mandatory field")]
        public virtual string AccountNumber
        {
            get
            {
                return this._accountNumber;
            }
            set
            {
                this._accountNumber = value;
            }
        }

        [BelongsTo(Column = "ACBTypeNumber", NotNull = true)]
        [ValidateNonEmpty("ACB Type is a mandatory field")]
        public virtual ACBType_DAO ACBType
        {
            get
            {
                return this._aCBType;
            }
            set
            {
                this._aCBType = value;
            }
        }

        [Property("AccountName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Account Name is a mandatory field")]
        public virtual string AccountName
        {
            get
            {
                return this._accountName;
            }
            set
            {
                this._accountName = value;
            }
        }

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

        [PrimaryKey(PrimaryKeyType.Native, "BankAccountKey", ColumnType = "Int32")]
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
    }
}