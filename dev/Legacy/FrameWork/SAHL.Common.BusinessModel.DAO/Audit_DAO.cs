using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This object models the Audits table in the warehouse database.  It's used to access all audit information that was generated by the domain.
    /// </summary>
    [DoNotTestWithGenericTest()]
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Audits", Schema = "dbo")]
    public partial class Audit_DAO : DB_Warehouse<Audit_DAO>
    {
        private System.DateTime _auditDate;

        private string _applicationName;

        private string _hostName;

        private string _workStationID;

        private string _windowsLogon;

        private string _formName;

        private string _tableName;

        private string _primaryKeyName;

        private string _primaryKeyValue;

        private string _auditData;

        private int _key;

        [Property("AuditDate", ColumnType = "Timestamp", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual System.DateTime AuditDate
        {
            get
            {
                return this._auditDate;
            }
        }

        [Property("ApplicationName", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
        }

        [Property("HostName", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string HostName
        {
            get
            {
                return this._hostName;
            }
        }

        [Property("WorkStationID", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string WorkStationID
        {
            get
            {
                return this._workStationID;
            }
        }

        [Property("WindowsLogon", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string WindowsLogon
        {
            get
            {
                return this._windowsLogon;
            }
        }

        [Property("FormName", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string FormName
        {
            get
            {
                return this._formName;
            }
        }

        [Property("TableName", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string TableName
        {
            get
            {
                return this._tableName;
            }
        }

        [Property("PrimaryKeyName", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string PrimaryKeyName
        {
            get
            {
                return this._primaryKeyName;
            }
        }

        [Property("PrimaryKeyValue", ColumnType = "String", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string PrimaryKeyValue
        {
            get
            {
                return this._primaryKeyValue;
            }
        }

        [Property("AuditData", ColumnType = "StringClob", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual string AuditData
        {
            get
            {
                return this._auditData;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AuditKey", ColumnType = "Int32", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
        }
    }
}