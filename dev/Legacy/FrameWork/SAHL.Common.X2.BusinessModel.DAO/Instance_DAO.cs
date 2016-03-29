using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("Instance", Schema = "X2")]
    public partial class Instance_DAO : DB_X2<Instance_DAO>
    {
        private string _name;

        private string _subject;

        private string _workFlowProvider;

        private string _creatorADUserName;

        private System.DateTime _creationDate;

        private DateTime? _stateChangeDate;

        private DateTime? _deadlineDate;

        private DateTime? _activityDate;

        private string _activityADUserName;

        private Int32? _activityID;

        private Int32? _priority;

        private Int64 _iD;

        //private IList<Instance_DAO> _instances;

        private IList<InstanceActivitySecurity_DAO> _instanceActivitySecurities;

        private IList<WorkList_DAO> _workLists;

        private Instance_DAO _parentInstance;

        private State_DAO _state;

        private WorkFlow_DAO _workFlow;

        private Int64? _SourceInstanceID;
        private Int32? _ReturnActivityID;

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

        [Property("Subject", ColumnType = "String")]
        public virtual string Subject
        {
            get
            {
                return this._subject;
            }
            set
            {
                this._subject = value;
            }
        }

        [Property("WorkFlowProvider", ColumnType = "String", NotNull = true)]
        public virtual string WorkFlowProvider
        {
            get
            {
                return this._workFlowProvider;
            }
            set
            {
                this._workFlowProvider = value;
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

        [Property("StateChangeDate")]
        public virtual DateTime? StateChangeDate
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

        [Property("ActivityDate")]
        public virtual DateTime? ActivityDate
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

        [Property("ActivityADUserName", ColumnType = "String")]
        public virtual string ActivityADUserName
        {
            get
            {
                return this._activityADUserName;
            }
            set
            {
                this._activityADUserName = value;
            }
        }

        [Property("ActivityID")]
        public virtual Int32? ActivityID
        {
            get
            {
                return this._activityID;
            }
            set
            {
                this._activityID = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int64")]
        public virtual Int64 ID
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

        //[HasMany(typeof(Instance), ColumnKey = "ParentInstanceID", Table = "Instance")]
        //public virtual IList<Instance> Instances
        //{
        //    get
        //    {
        //        return this._instances;
        //    }
        //    set
        //    {
        //        this._instances = value;
        //    }
        //}

        [HasMany(typeof(InstanceActivitySecurity_DAO), ColumnKey = "InstanceID", Table = "InstanceActivitySecurity", Lazy = true, OrderBy = "ID")]
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

        [HasMany(typeof(WorkList_DAO), ColumnKey = "InstanceID", Table = "WorkList_DAO", Lazy = true)]
        public virtual IList<WorkList_DAO> WorkLists
        {
            get
            {
                return this._workLists;
            }
            set
            {
                this._workLists = value;
            }
        }

        [BelongsTo("ParentInstanceID", NotNull = false)]
        public virtual Instance_DAO ParentInstance
        {
            get
            {
                return this._parentInstance;
            }
            set
            {
                this._parentInstance = value;
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

        public static IList<Instance_DAO> FindByPrincipal(SAHLPrincipal principal)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(true, true); //principal.GetCachedRolesAsStringForQuery(true, true);
            string HQL = String.Format("SELECT DISTINCT wl.Instance FROM WorkList_DAO wl WHERE wl.ADUserName IN ({0})", groups);
            SimpleQuery<Instance_DAO> q = new SimpleQuery<Instance_DAO>(HQL);
            //q.SetParameter("groups", groups);
            Instance_DAO[] IArr = q.Execute();
            List<Instance_DAO> Ins = new List<Instance_DAO>();
            Ins.AddRange(IArr);
            return Ins;
        }

        [Property("SourceInstanceID", ColumnType = "Int64", NotNull = false)]
        public virtual Int64? SourceInstanceID
        {
            get
            {
                return _SourceInstanceID;
            }
            set
            {
                _SourceInstanceID = value;
            }
        }

        [Property("ReturnActivityID", ColumnType = "Int32", NotNull = false)]
        public virtual Int32? ReturnActivityID
        {
            get
            {
                return _ReturnActivityID;
            }
            set
            {
                _ReturnActivityID = value;
            }
        }
    }
}