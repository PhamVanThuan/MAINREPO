using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;


namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AuditAccountRelationship",  Schema = "dbo")]
    public partial class AuditAccountRelationship_DAO : DB_2AM<AuditAccountRelationship_DAO>
    {

        private string _auditLogin;

        private string _auditHostName;

        private string _auditProgramName;

        private System.DateTime _auditDate;

        private char _auditAddUpdateDelete;

        private int _accountKey;

        private int _relatedAccountKey;

        private int _accountRelationshipKey;

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

        [Property("RelatedAccountKey", ColumnType = "Int32", NotNull = true)]
        public virtual int RelatedAccountKey
        {
            get
            {
                return this._relatedAccountKey;
            }
            set
            {
                this._relatedAccountKey = value;
            }
        }

        [Property("AccountRelationshipKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountRelationshipKey
        {
            get
            {
                return this._accountRelationshipKey;
            }
            set
            {
                this._accountRelationshipKey = value;
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
