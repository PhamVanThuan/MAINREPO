using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;


namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AuditBondMortgageLoan",  Schema = "dbo")]
    public partial class AuditBondMortgageLoan_DAO :  DB_2AM<AuditBondMortgageLoan_DAO>
    {

        private string _auditLogin;

        private string _auditHostName;

        private string _auditProgramName;

        private System.DateTime _auditDate;

        private char _auditAddUpdateDelete;

        private int? _bondMortgageLoanKey;

        private int? _financialServiceKey;

        private int? _bondKey;

        private decimal _auditNumber;

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

        [Property("BondMortgageLoanKey", ColumnType = "Int32", NotNull = true)]
        public virtual int? BondMortgageLoanKey
        {
            get
            {
                return this._bondMortgageLoanKey;
            }
            set
            {
                this._bondMortgageLoanKey = value;
            }
        }

        [Property("FinancialServiceKey", ColumnType = "Int32")]
        public virtual int? FinancialServiceKey
        {
            get
            {
                return this._financialServiceKey;
            }
            set
            {
                this._financialServiceKey = value;
            }
        }

        [Property("BondKey", ColumnType = "Int32")]
        public virtual int? BondKey
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

        [PrimaryKey(PrimaryKeyType.Native, "AuditNumber", ColumnType = "Decimal")]
        public virtual decimal AuditNumber
        {
            get
            {
                return this._auditNumber;
            }
            set
            {
                this._auditNumber = value;
            }
        }
    }
}
