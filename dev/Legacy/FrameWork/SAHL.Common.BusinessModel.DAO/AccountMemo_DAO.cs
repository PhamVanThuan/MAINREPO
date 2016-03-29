using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountMemo",  Schema = "dbo")]
    public partial class AccountMemo_DAO : DB_2AM<AccountMemo_DAO>
    {

        private System.DateTime _insertDate;

        private System.DateTime _reminderDate;

        private System.DateTime _expiryDate;

        private string _memo;

        private string _userID;

        private System.DateTime _changeDate;

        private int _Key;

        private Account_DAO _account;

        private AccountMemoStatus_DAO _accountMemoStatus;

        // todo: Uncomment when AccountMemoStatus implementde
        //private AccountMemoStatus _accountMemoStatus;

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Insert Date is a mandatory field")]
        public virtual System.DateTime InsertDate
        {
            get
            {
                return this._insertDate;
            }
            set
            {
                this._insertDate = value;
            }
        }

        [Property("ReminderDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Reminder Date is a mandatory field")]
        public virtual System.DateTime ReminderDate
        {
            get
            {
                return this._reminderDate;
            }
            set
            {
                this._reminderDate = value;
            }
        }

        [Property("ExpiryDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Expiry Date is a mandatory field")]
        public virtual System.DateTime ExpiryDate
        {
            get
            {
                return this._expiryDate;
            }
            set
            {
                this._expiryDate = value;
            }
        }

        [Property("Memo", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Memo is a mandatory field")]
        public virtual string Memo
        {
            get
            {
                return this._memo;
            }
            set
            {
                this._memo = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
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

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "AccountMemoKey", ColumnType = "Int32")]
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

        [BelongsTo("AccountMemoStatusKey", NotNull = true)]
        [ValidateNonEmpty("Account Memo Status is a mandatory field")]
        public virtual AccountMemoStatus_DAO AccountMemoStatus
        {
            get
            {
                return this._accountMemoStatus;
            }
            set
            {
                this._accountMemoStatus = value;
            }
        }
    }
}
