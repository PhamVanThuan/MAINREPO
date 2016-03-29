using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AuditBankAccount",  Schema = "dbo")]
    public partial class AuditBankAccount_DAO : DB_2AM<AuditBankAccount_DAO>
    {

        private string _auditLogin;

        private string _auditHostName;

        private string _auditProgramName;

        private System.DateTime _auditDate;

        private char _auditAddUpdateDelete;

        private int _bankAccountKey;

        private string _aCBBranchCode;

        private string _accountNumber;

        private int _aCBTypeNumber;

        private string _accountName;

        private string _userID;

        private System.DateTime? _changeDate;

        private decimal _key;

        [Property("AuditLogin", ColumnType = "String", NotNull = true)]
        public virtual string AuditLogin
        {
            get
            {
                return this._auditLogin;
            }
            set
            {
                this._auditLogin = value;
            }
        }

        [Property("AuditHostName", ColumnType = "String", NotNull = true)]
        public virtual string AuditHostName
        {
            get
            {
                return this._auditHostName;
            }
            set
            {
                this._auditHostName = value;
            }
        }

        [Property("AuditProgramName", ColumnType = "String", NotNull = true)]
        public virtual string AuditProgramName
        {
            get
            {
                return this._auditProgramName;
            }
            set
            {
                this._auditProgramName = value;
            }
        }

        [Property("AuditDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime AuditDate
        {
            get
            {
                return this._auditDate;
            }
            set
            {
                this._auditDate = value;
            }
        }

        [Property("AuditAddUpdateDelete", ColumnType = "AnsiChar", NotNull = true)]
        public virtual char AuditAddUpdateDelete
        {
            get
            {
                return this._auditAddUpdateDelete;
            }
            set
            {
                this._auditAddUpdateDelete = value;
            }
        }

        [Property("BankAccountKey", ColumnType = "Int32", NotNull = true)]
        public virtual int BankAccountKey
        {
            get
            {
                return this._bankAccountKey;
            }
            set
            {
                this._bankAccountKey = value;
            }
        }

        [Property("ACBBranchCode", ColumnType = "String", NotNull = true)]
        public virtual string ACBBranchCode
        {
            get
            {
                return this._aCBBranchCode;
            }
            set
            {
                this._aCBBranchCode = value;
            }
        }

        [Property("AccountNumber", ColumnType = "String", NotNull = true)]
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

        [Property("ACBTypeNumber", ColumnType = "Int32", NotNull = true)]
        public virtual int ACBTypeNumber
        {
            get
            {
                return this._aCBTypeNumber;
            }
            set
            {
                this._aCBTypeNumber = value;
            }
        }

        [Property("AccountName", ColumnType = "String")]
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

        [PrimaryKey(PrimaryKeyType.Native, "AuditNumber", ColumnType = "Decimal")]
        public virtual decimal Key
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
