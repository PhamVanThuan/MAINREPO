using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AuditAccount",  Schema = "dbo")]
    public partial class AuditAccount_DAO : DB_2AM<AuditAccount_DAO>
    {

        private string _auditLogin;

        private string _auditHostName;

        private string _auditProgramName;

        private System.DateTime _auditDate;

        private char _auditAddUpdateDelete;

        private int _accountKey;

        private double? _fixedPayment;

        private int? _accountStatusKey;

        private System.DateTime? _insertedDate;

        private int? _originationSourceProductKey;

        private System.DateTime? _openDate;

        private System.DateTime? _closeDate;

        private int? _rRR_ProductKey;

        private int? _rRR_OriginationSourceKey;

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

        [Property("FixedPayment", ColumnType = "Double", NotNull = true)]
        public virtual double? FixedPayment
        {
            get
            {
                return this._fixedPayment;
            }
            set
            {
                this._fixedPayment = value;
            }
        }

        [Property("AccountStatusKey", ColumnType = "Int32", NotNull = true)]
        public virtual int? AccountStatusKey
        {
            get
            {
                return this._accountStatusKey;
            }
            set
            {
                this._accountStatusKey = value;
            }
        }

        [Property("InsertedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? InsertedDate
        {
            get
            {
                return this._insertedDate;
            }
            set
            {
                this._insertedDate = value;
            }
        }

        [Property("OriginationSourceProductKey", ColumnType = "Int32")]
        public virtual int? OriginationSourceProductKey
        {
            get
            {
                return this._originationSourceProductKey;
            }
            set
            {
                this._originationSourceProductKey = value;
            }
        }

        [Property("OpenDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? OpenDate
        {
            get
            {
                return this._openDate;
            }
            set
            {
                this._openDate = value;
            }
        }

        [Property("CloseDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CloseDate
        {
            get
            {
                return this._closeDate;
            }
            set
            {
                this._closeDate = value;
            }
        }

        [Property("RRR_ProductKey", ColumnType = "Int32")]
        public virtual int? RRR_ProductKey
        {
            get
            {
                return this._rRR_ProductKey;
            }
            set
            {
                this._rRR_ProductKey = value;
            }
        }

        [Property("RRR_OriginationSourceKey", ColumnType = "Int32")]
        public virtual int? RRR_OriginationSourceKey
        {
            get
            {
                return this._rRR_OriginationSourceKey;
            }
            set
            {
                this._rRR_OriginationSourceKey = value;
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