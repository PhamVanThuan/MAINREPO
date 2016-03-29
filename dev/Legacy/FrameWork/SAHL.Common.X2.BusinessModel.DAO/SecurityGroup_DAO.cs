using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("SecurityGroup", Schema = "X2")]
    public partial class SecurityGroup_DAO : DB_X2<SecurityGroup_DAO>
    {
        private bool _isDynamic;

        private string _name;

        private string _description;

        private int _iD;

        private Process_DAO _process;

        private WorkFlow_DAO _workFlow;

        private IList<Activity_DAO> _activities;

        private IList<State_DAO> _states;

        private IList<WorkFlow_DAO> _workFlows;

        [Property("IsDynamic", ColumnType = "Boolean", NotNull = true)]
        public virtual bool IsDynamic
        {
            get
            {
                return this._isDynamic;
            }
            set
            {
                this._isDynamic = value;
            }
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

        [Property("Description", ColumnType = "String")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
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

        [BelongsTo("ProcessID", NotNull = false)]
        public virtual Process_DAO Process
        {
            get
            {
                return this._process;
            }
            set
            {
                this._process = value;
            }
        }

        [BelongsTo("WorkFlowID", NotNull = false)]
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

        [HasAndBelongsToMany(typeof(Activity_DAO), ColumnRef = "ActivityID", ColumnKey = "SecurityGroupID", Lazy = true, Schema = "X2", Table = "ActivitySecurity")]
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

        [HasAndBelongsToMany(typeof(State_DAO), ColumnRef = "StateID", ColumnKey = "SecurityGroupID", Lazy = true, Schema = "X2", Table = "StateWorkList")]
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

        [HasAndBelongsToMany(typeof(WorkFlow_DAO), ColumnRef = "WorkFlowID", ColumnKey = "SecurityGroupID", Lazy = true, Schema = "X2", Table = "WorkFlowSecurity")]
        public virtual IList<WorkFlow_DAO> WorkFlows
        {
            get
            {
                return this._workFlows;
            }
            set
            {
                this._workFlows = value;
            }
        }
    }
}