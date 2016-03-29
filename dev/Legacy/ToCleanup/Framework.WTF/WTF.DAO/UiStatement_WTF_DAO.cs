
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("uiStatement", Schema = "dbo")]
    public partial class UiStatement_WTF_DAO : DB_Test_WTF<UiStatement_WTF_DAO>
    {

        private string _applicationName;

        private string _statementName;

        private DateTime? _modifyDate;

        private int _version;

        private string _modifyUser;

        private string _statement;

        private int _type;

        private DateTime? _lastAccessedDate;

        private int _key;

        [Property("ApplicationName", ColumnType = "String")]
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

        [Property("StatementName", ColumnType = "String")]
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

        [Property("ModifyDate", ColumnType = "Timestamp")]
        public virtual DateTime? ModifyDate
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

        [Property("Version", ColumnType = "Int32")]
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

        [Property("ModifyUser", ColumnType = "String")]
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

        [Property("Statement", ColumnType = "String")]
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

        [Property("Type", ColumnType = "Int32")]
        public virtual int Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [Property("LastAccessedDate", ColumnType = "Timestamp")]
        public virtual DateTime? LastAccessedDate
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
    }
}

