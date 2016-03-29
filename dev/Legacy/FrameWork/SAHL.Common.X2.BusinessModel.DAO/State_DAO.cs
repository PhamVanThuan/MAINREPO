using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("State", Schema = "X2")]
    public partial class State_DAO : DB_X2<State_DAO>
    {
        private int _iD;

        private string _name;

        private bool _forwardState;

        private IList<Activity_DAO> _activities;

        private IList<Activity_DAO> _nextActivities;

        private IList<Instance_DAO> _instances;

        private IList<Log_DAO> _logs;

        private IList<State_DAO> _states;

        private IList<WorkFlowHistory_DAO> _workFlowHistories;

        private StateType_DAO _stateType;

        private WorkFlow_DAO _workFlow;

        private IList<SecurityGroup_DAO> _securityGroups;

        private IList<Form_DAO> _Forms;

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

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

        [Property("ForwardState", ColumnType = "Boolean", NotNull = true)]
        public virtual bool ForwardState
        {
            get
            {
                return this._forwardState;
            }
            set
            {
                this._forwardState = value;
            }
        }

        [HasMany(typeof(Activity_DAO), ColumnKey = "NextStateID", Table = "Activity", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Activity_DAO> NextActivities
        {
            get
            {
                return this._nextActivities;
            }
            set
            {
                this._nextActivities = value;
            }
        }

        [HasMany(typeof(Activity_DAO), ColumnKey = "StateID", Table = "Activity", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Activity_DAO> Activities
        {
            get
            {
                return this._activities;
            }
            set
            {
                this._activities = value;
            }
        }

        [HasMany(typeof(Instance_DAO), ColumnKey = "StateID", Table = "Instance", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Instance_DAO> Instances
        {
            get
            {
                return this._instances;
            }
            set
            {
                this._instances = value;
            }
        }

        [HasMany(typeof(Log_DAO), ColumnKey = "StateID", Table = "Log", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
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

        [HasMany(typeof(State_DAO), ColumnKey = "ID", Table = "State", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<State_DAO> States
        {
            get
            {
                return this._states;
            }
            set
            {
                this._states = value;
            }
        }

        [HasMany(typeof(WorkFlowHistory_DAO), ColumnKey = "StateID", Table = "WorkFlowHistory", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
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
        public virtual StateType_DAO StateType
        {
            get
            {
                return this._stateType;
            }
            set
            {
                this._stateType = value;
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

        [HasAndBelongsToMany(typeof(SecurityGroup_DAO), ColumnRef = "SecurityGroupID", ColumnKey = "StateID", Schema = "X2", Table = "StateWorkList", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
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

        [HasAndBelongsToMany(typeof(Form_DAO), ColumnRef = "FormID", ColumnKey = "StateID", Lazy = true, OrderBy = "FormOrder", Schema = "X2", Table = "StateForm", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Form_DAO> Forms
        {
            get
            {
                return this._Forms;
            }
            set
            {
                this._Forms = value;
            }
        }
    }
}