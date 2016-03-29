using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("uiStatement", Schema = "dbo")]
    public partial class UIStatement_DAO : DB_2AM<UIStatement_DAO>
    {
        private string _applicationName;

        private string _statementName;

        private System.DateTime _modifyDate;

        private int _version;

        private string _modifyUser;

        private string _statement;

        private int _key;

        private UIStatementType_DAO _uiStatementType;

        private DateTime _lastAccessedDate;

        [Property("ApplicationName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Application Name is a mandatory field")]
        public virtual string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
            set
            {
                this._applicationName = value;
            }
        }

        [Property("StatementName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Statement Name is a mandatory field")]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [Property("ModifyDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Modify Date is a mandatory field")]
        public virtual System.DateTime ModifyDate
        {
            get
            {
                return this._modifyDate;
            }
            set
            {
                this._modifyDate = value;
            }
        }

        [Property("Version", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Version is a mandatory field")]
        public virtual int Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }

        [Property("ModifyUser", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Modify User is a mandatory field")]
        public virtual string ModifyUser
        {
            get
            {
                return this._modifyUser;
            }
            set
            {
                this._modifyUser = value;
            }
        }

        [Property("Statement", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Statement is a mandatory field")]
        public virtual string Statement
        {
            get
            {
                return this._statement;
            }
            set
            {
                this._statement = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "StatementKey", ColumnType = "Int32")]
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

        [BelongsTo("Type", NotNull = true)]
        public virtual UIStatementType_DAO uiStatementType
        {
            get
            {
                return this._uiStatementType;
            }
            set
            {
                this._uiStatementType = value;
            }
        }

        [Property("LastAccessedDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Last Accessed Date is a mandatory field")]
        public virtual System.DateTime LastAccessedDate
        {
            get
            {
                return this._lastAccessedDate;
            }
            set
            {
                this._lastAccessedDate = value;
            }
        }
    }
}