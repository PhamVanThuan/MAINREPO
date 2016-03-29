using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]   
    [ActiveRecord("WorkFlow", Schema = "X2")]
    public partial class WorkFlow_DAO : DB_X2<WorkFlow_DAO>
    {
        private string _name;

        private System.DateTime _createDate;

        private string _storageTable;

        private string _storageKey;

        private string _defaultSubject;

        private int _iD;

        private int _genericKeyTypeKey;

        private IList<Activity_DAO> _activities;

        private IList<Form_DAO> _forms;

        private IList<Instance_DAO> _instances;

        private IList<SecurityGroup_DAO> _securityGroups;

        private IList<State_DAO> _states;

        private IList<WorkFlow_DAO> _workFlows;

        private IList<WorkFlowActivity_DAO> _workFlowActivities;

        private IList<WorkFlowActivity_DAO> _nextWorkFlowActivities;

        private Process_DAO _process;

        private WorkFlow_DAO _workFlow;

        private WorkFlowIcon_DAO _workFlowIcon;

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

        [Property("CreateDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime CreateDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                this._createDate = value;
            }
        }

        [Property("StorageTable", ColumnType = "String", NotNull = true)]
        public virtual string StorageTable
        {
            get
            {
                return this._storageTable;
            }
            set
            {
                this._storageTable = value;
            }
        }

        [Property("StorageKey", ColumnType = "String", NotNull = true)]
        public virtual string StorageKey
        {
            get
            {
                return this._storageKey;
            }
            set
            {
                this._storageKey = value;
            }
        }

        [Property("GenericKeyTypeKey", ColumnType = "Int32", NotNull = false)]
        public virtual int GenericKeyTypeKey
        {
            get
            {
                return this._genericKeyTypeKey;
            }
            set
            {
                this._genericKeyTypeKey = value;
            }
        }

        [Property("DefaultSubject", ColumnType = "String")]
        public virtual string DefaultSubject
        {
            get
            {
                return this._defaultSubject;
            }
            set
            {
                this._defaultSubject = value;
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

        [HasMany(typeof(Activity_DAO), ColumnKey = "WorkFlowID", Table = "Activity", Lazy = true)]
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

        [HasMany(typeof(Form_DAO), ColumnKey = "WorkFlowID", Table = "Form", Lazy = true)]
        public virtual IList<Form_DAO> Forms
        {
            get
            {
                return this._forms;
            }
            set
            {
                this._forms = value;
            }
        }

        [HasMany(typeof(Instance_DAO), ColumnKey = "WorkFlowID", Table = "Instance", Lazy = true)]
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

        [HasMany(typeof(SecurityGroup_DAO), ColumnKey = "WorkFlowID", Table = "SecurityGroup", Lazy = true)]
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

        [HasMany(typeof(State_DAO), ColumnKey = "WorkFlowID", Table = "State", Lazy = true)]
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

        [HasMany(typeof(WorkFlow_DAO), ColumnKey = "WorkFlowAncestorID", Table = "WorkFlow", Lazy = true)]
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

        [HasMany(typeof(WorkFlowActivity_DAO), ColumnKey = "NextWorkFlowID", Table = "WorkFlowActivity", Lazy = true)]
        public virtual IList<WorkFlowActivity_DAO> NextWorkFlowActivities
        {
            get
            {
                return this._nextWorkFlowActivities;
            }
            set
            {
                this._nextWorkFlowActivities = value;
            }
        }

        [HasMany(typeof(WorkFlowActivity_DAO), ColumnKey = "WorkFlowID", Table = "WorkFlowActivity", Lazy = true)]
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

        [BelongsTo("ProcessID", NotNull = true)]
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

        [BelongsTo("WorkFlowAncestorID", NotNull = false)]
        public virtual WorkFlow_DAO WorkFlowAncestor
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

        [BelongsTo("IconID", NotNull = true)]
        public virtual WorkFlowIcon_DAO WorkFlowIcon
        {
            get
            {
                return this._workFlowIcon;
            }
            set
            {
                this._workFlowIcon = value;
            }
        }

        public static IList<WorkFlow_DAO> FindByPrincipal(SAHLPrincipal principal)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(true, true); //principal.GetCachedRolesAsStringForQuery(true, true);
            string HQL = String.Format("SELECT DISTINCT wl.Instance.WorkFlow FROM WorkList_DAO wl WHERE wl.ADUserName IN ({0})", groups);

            SimpleQuery<WorkFlow_DAO> q = new SimpleQuery<WorkFlow_DAO>(HQL);
            //q.SetParameter("groups", groups);
            WorkFlow_DAO[] wfArr = q.Execute();
            List<WorkFlow_DAO> wf = new List<WorkFlow_DAO>();
            wf.AddRange(wfArr);
            return wf;
        }
    }
}