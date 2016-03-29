using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("Activity", Schema = "X2")]
    public partial class Activity_DAO : DB_X2<Activity_DAO>
    {
        private string _name;

        private bool _splitWorkFlow;

        private int _priority;

        private string _activityMessage;

        private int _activatedByExternalActivity;

        private string _chainedActivityName;

        private int _iD;

        private IList<InstanceActivitySecurity_DAO> _instanceActivitySecurities;

        private IList<Log_DAO> _logs;

        private IList<StageActivity_DAO> _stageActivities;

        private IList<WorkFlowActivity_DAO> _workFlowActivities;

        private IList<WorkFlowHistory_DAO> _workFlowHistories;

        private ActivityType_DAO _activityType;

        private ExternalActivity_DAO _externalActivity;

        private ExternalActivityTarget_DAO _externalActivityTarget;

        private Form_DAO _form;

        private State_DAO _state;

        private State_DAO _nextState;

        private WorkFlow_DAO _workFlow;

        private IList<SecurityGroup_DAO> _securityGroups;

        [Property("Name", ColumnType = "String", NotNull = true)]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [Property("SplitWorkFlow", ColumnType = "Boolean", NotNull = true)]
        public virtual bool SplitWorkFlow
        {
            get
            {
                return this._splitWorkFlow;
            }
            set
            {
                this._splitWorkFlow = value;
            }
        }

        [Property("Priority", ColumnType = "Int32", NotNull = true)]
        public virtual int Priority
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

        [Property("ActivityMessage", ColumnType = "String")]
        public virtual string ActivityMessage
        {
            get
            {
                return this._activityMessage;
            }
            set
            {
                this._activityMessage = value;
            }
        }

        [Property("ActivatedByExternalActivity", ColumnType = "Int32")]
        public virtual int ActivatedByExternalActivity
        {
            get
            {
                return this._activatedByExternalActivity;
            }
            set
            {
                this._activatedByExternalActivity = value;
            }
        }

        [Property("ChainedActivityName", ColumnType = "String")]
        public virtual string ChainedActivityName
        {
            get
            {
                return this._chainedActivityName;
            }
            set
            {
                this._chainedActivityName = value;
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

        [HasMany(typeof(InstanceActivitySecurity_DAO), ColumnKey = "ActivityID", Table = "InstanceActivitySecurity", Lazy = true)]
        public virtual IList<InstanceActivitySecurity_DAO> InstanceActivitySecurities
        {
            get
            {
                return this._instanceActivitySecurities;
            }
            set
            {
                this._instanceActivitySecurities = value;
            }
        }

        [HasMany(typeof(Log_DAO), ColumnKey = "ActivityID", Table = "Log", Lazy = true)]
        public virtual IList<Log_DAO> Logs
        {
            get
            {
                return this._logs;
            }
            set
            {
                this._logs = value;
            }
        }

        [HasMany(typeof(StageActivity_DAO), ColumnKey = "ActivityID", Table = "StageActivity", Lazy = true)]
        public virtual IList<StageActivity_DAO> StageActivities
        {
            get
            {
                return this._stageActivities;
            }
            set
            {
                this._stageActivities = value;
            }
        }

        [HasMany(typeof(WorkFlowActivity_DAO), ColumnKey = "NextActivityID", Table = "WorkFlowActivity", Lazy = true)]
        public virtual IList<WorkFlowActivity_DAO> WorkFlowActivities
        {
            get
            {
                return this._workFlowActivities;
            }
            set
            {
                this._workFlowActivities = value;
            }
        }

        [HasMany(typeof(WorkFlowHistory_DAO), ColumnKey = "ActivityID", Table = "WorkFlowHistory", Lazy = true)]
        public virtual IList<WorkFlowHistory_DAO> WorkFlowHistories
        {
            get
            {
                return this._workFlowHistories;
            }
            set
            {
                this._workFlowHistories = value;
            }
        }

        [BelongsTo("Type", NotNull = true)]
        public virtual ActivityType_DAO ActivityType
        {
            get
            {
                return this._activityType;
            }
            set
            {
                this._activityType = value;
            }
        }

        [BelongsTo("RaiseExternalActivity", NotNull = false)]
        public virtual ExternalActivity_DAO ExternalActivity
        {
            get
            {
                return this._externalActivity;
            }
            set
            {
                this._externalActivity = value;
            }
        }

        [BelongsTo("ExternalActivityTarget", NotNull = false)]
        public virtual ExternalActivityTarget_DAO ExternalActivityTarget
        {
            get
            {
                return this._externalActivityTarget;
            }
            set
            {
                this._externalActivityTarget = value;
            }
        }

        [BelongsTo("FormID", NotNull = false)]
        public virtual Form_DAO Form
        {
            get
            {
                return this._form;
            }
            set
            {
                this._form = value;
            }
        }

        [BelongsTo("NextStateID", NotNull = true)]
        public virtual State_DAO NextState
        {
            get
            {
                return this._nextState;
            }
            set
            {
                this._nextState = value;
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

        [BelongsTo("WorkFlowID", NotNull = true)]
        public virtual WorkFlow_DAO WorkFlow
        {
            get
            {
                return this._workFlow;
            }
            set
            {
                this._workFlow = value;
            }
        }

        [HasAndBelongsToMany(typeof(SecurityGroup_DAO), ColumnRef = "SecurityGroupID", ColumnKey = "ActivityID", Lazy = true, Schema = "X2", Table = "ActivitySecurity")]
        public virtual IList<SecurityGroup_DAO> SecurityGroups
        {
            get
            {
                return this._securityGroups;
            }
            set
            {
                this._securityGroups = value;
            }
        }
    }
}