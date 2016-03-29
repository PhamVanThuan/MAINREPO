using System;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("Log", Schema = "X2")]
    public partial class Log_DAO : DB_X2<Log_DAO>
    {
        private System.DateTime _date;

        private Int32? _processID;

        private Int32? _workFlowID;

        private Int32? _instanceID;

        private string _aDUserName;

        private string _message;

        private string _stackTrace;

        private int _iD;

        private Activity_DAO _activity;

        private State_DAO _state;

        [Property("Date", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }

        [Property("ProcessID")]
        public virtual Int32? ProcessID
        {
            get
            {
                return this._processID;
            }
            set
            {
                this._processID = value;
            }
        }

        [Property("WorkFlowID")]
        public virtual Int32? WorkFlowID
        {
            get
            {
                return this._workFlowID;
            }
            set
            {
                this._workFlowID = value;
            }
        }

        [Property("InstanceID")]
        public virtual Int32? InstanceID
        {
            get
            {
                return this._instanceID;
            }
            set
            {
                this._instanceID = value;
            }
        }

        [Property("ADUserName", ColumnType = "String")]
        public virtual string ADUserName
        {
            get
            {
                return this._aDUserName;
            }
            set
            {
                this._aDUserName = value;
            }
        }

        [Property("Message", ColumnType = "StringClob")]
        public virtual string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

        [Property("StackTrace", ColumnType = "StringClob")]
        public virtual string StackTrace
        {
            get
            {
                return this._stackTrace;
            }
            set
            {
                this._stackTrace = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get
            {
                return this._iD;
            }
            set
            {
                this._iD = value;
            }
        }

        [BelongsTo("ActivityID", NotNull = false)]
        public virtual Activity_DAO Activity
        {
            get
            {
                return this._activity;
            }
            set
            {
                this._activity = value;
            }
        }

        [BelongsTo("StateID", NotNull = false)]
        public virtual State_DAO State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }
    }
}