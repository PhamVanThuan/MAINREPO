using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LegalEntityMemo", Schema = "dbo")]
    public partial class LegalEntityMemo_DAO : DB_2AM<LegalEntityMemo_DAO>
    {

        private System.DateTime _insertDate;

        private System.DateTime _reminderDate;

        private System.DateTime _expiryDate;

        private string _memo;

        private string _userID;

        private int _accountMemoStatusKey;

        private System.DateTime _changeDate;

        private int _key;

        private LegalEntity_DAO _legalEntity;

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
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

        // TODO: AccountMemoStatus
        [Property("AccountMemoStatusKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountMemoStatusKey
        {
            get
            {
                return this._accountMemoStatusKey;
            }
            set
            {
                this._accountMemoStatusKey = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityMemoKey", ColumnType = "Int32")]
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

        [BelongsTo("LegalEntityKey", NotNull = false)]
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
