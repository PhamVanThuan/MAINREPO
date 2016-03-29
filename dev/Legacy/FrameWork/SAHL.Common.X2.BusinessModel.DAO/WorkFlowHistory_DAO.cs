using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("WorkFlowHistory", Schema = "X2")]
    public partial class WorkFlowHistory_DAO : DB_X2<WorkFlowHistory_DAO>
    {
        private long _instanceID;

        private string _creatorADUserName;

        private System.DateTime _creationDate;

        private System.DateTime _stateChangeDate;

        private DateTime? _deadlineDate;

        private System.DateTime _activityDate;

        private string _aDUserName;

        private Int32? _priority;

        private int _iD;

        private Activity_DAO _activity;

        private State_DAO _state;

        [Property("InstanceID", ColumnType = "Int64", NotNull = true)]
        public virtual long InstanceID
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

        [Property("CreatorADUserName", ColumnType = "String", NotNull = true)]
        public virtual string CreatorADUserName
        {
            get
            {
                return this._creatorADUserName;
            }
            set
            {
                this._creatorADUserName = value;
            }
        }

        [Property("CreationDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime CreationDate
        {
            get
            {
                return this._creationDate;
            }
            set
            {
                this._creationDate = value;
            }
        }

        [Property("StateChangeDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime StateChangeDate
        {
            get
            {
                return this._stateChangeDate;
            }
            set
            {
                this._stateChangeDate = value;
            }
        }

        [Property("DeadlineDate")]
        public virtual DateTime? DeadlineDate
        {
            get
            {
                return this._deadlineDate;
            }
            set
            {
                this._deadlineDate = value;
            }
        }

        [Property("ActivityDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ActivityDate
        {
            get
            {
                return this._activityDate;
            }
            set
            {
                this._activityDate = value;
            }
        }

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
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

        [Property("Priority")]
        public virtual Int32? Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
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

        [BelongsTo("StateID", NotNull = true)]
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