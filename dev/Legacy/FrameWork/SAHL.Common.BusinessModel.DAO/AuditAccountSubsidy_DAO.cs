using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{


    [ActiveRecord("AuditAccountSubsidy",  Schema = "dbo")]
    public partial class AuditAccountSubsidy_DAO : DB_2AM<AuditAccountSubsidy_DAO>
    {

        private string _auditLogin;

        private string _auditHostName;

        private string _auditProgramName;

        private System.DateTime _auditDate;

        private char _auditAddUpdateDelete;

        private int _accountSubsidyKey;

        private int _accountKey;

        private int _subsidyKey;

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

        [Property("AccountSubsidyKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountSubsidyKey
        {
            get
            {
                return this._accountSubsidyKey;
            }
            set
            {
                this._accountSubsidyKey = value;
            }
        }

        [Property("AccountKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountKey
        {
            get
            {
                return this._accountKey;
            }
            set
            {
                this._accountKey = value;
            }
        }

        [Property("SubsidyKey", ColumnType = "Int32", NotNull = true)]
        public virtual int SubsidyKey
        {
            get
            {
                return this._subsidyKey;
            }
            set
            {
                this._subsidyKey = value;
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
