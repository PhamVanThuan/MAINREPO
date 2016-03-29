using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;


namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AuditBond",  Schema = "dbo")]
    public partial class AuditBond_DAO : DB_2AM<AuditBond_DAO>
    {

        private string _auditLogin;

        private string _auditHostName;

        private string _auditProgramName;

        private System.DateTime _auditDate;

        private char _auditAddUpdateDelete;

        private int _bondKey;

        private int _deedsOfficeKey;

        private int _attorneyKey;

        private string _bondRegistrationNumber;

        private System.DateTime? _bondRegistrationDate;

        private double? _bondRegistrationAmount;

        private double? _bondLoanAgreementAmount;

        private string _userID;

        private System.DateTime _changeDate;

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

        [Property("BondKey", ColumnType = "Int32", NotNull = true)]
        public virtual int BondKey
        {
            get
            {
                return this._bondKey;
            }
            set
            {
                this._bondKey = value;
            }
        }

        [Property("DeedsOfficeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int DeedsOfficeKey
        {
            get
            {
                return this._deedsOfficeKey;
            }
            set
            {
                this._deedsOfficeKey = value;
            }
        }

        [Property("AttorneyKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AttorneyKey
        {
            get
            {
                return this._attorneyKey;
            }
            set
            {
                this._attorneyKey = value;
            }
        }

        [Property("BondRegistrationNumber", ColumnType = "String")]
        public virtual string BondRegistrationNumber
        {
            get
            {
                return this._bondRegistrationNumber;
            }
            set
            {
                this._bondRegistrationNumber = value;
            }
        }

        [Property("BondRegistrationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? BondRegistrationDate
        {
            get
            {
                return this._bondRegistrationDate;
            }
            set
            {
                this._bondRegistrationDate = value;
            }
        }

        [Property("BondRegistrationAmount", ColumnType = "Double")]
        public virtual double? BondRegistrationAmount
        {
            get
            {
                return this._bondRegistrationAmount;
            }
            set
            {
                this._bondRegistrationAmount = value;
            }
        }

        [Property("BondLoanAgreementAmount", ColumnType = "Double")]
        public virtual double? BondLoanAgreementAmount
        {
            get
            {
                return this._bondLoanAgreementAmount;
            }
            set
            {
                this._bondLoanAgreementAmount = value;
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
