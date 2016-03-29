using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("Process", Schema = "X2")]
    public partial class Process_DAO : DB_X2<Process_DAO>
    {
        private string _name;

        private string _version;

        //       private NullableByte[] _designerData;

        private System.DateTime _createDate;

        private int _iD;

        private IList<Process_DAO> _processes;

        private IList<SecurityGroup_DAO> _securityGroups;

        private IList<WorkFlow_DAO> _workFlows;

        private Process_DAO _processAncestor;

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

        [Property("Version", ColumnType = "String", NotNull = true)]
        public virtual string Version
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

        //[Property("DesignerData")]
        //public virtual NullableByte[] DesignerData
        //{
        //    get
        //    {
        //        return this._designerData;
        //    }
        //    set
        //    {
        //        this._designerData = value;
        //    }
        //}

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

        [HasMany(typeof(Process_DAO), ColumnKey = "ProcessAncestorID", Table = "Process", Lazy = true)]
        public virtual IList<Process_DAO> Processes
        {
            get
            {
                return this._processes;
            }
            set
            {
                this._processes = value;
            }
        }

        [HasMany(typeof(SecurityGroup_DAO), ColumnKey = "ProcessID", Table = "SecurityGroup", Lazy = true)]
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

        [HasMany(typeof(WorkFlow_DAO), ColumnKey = "ProcessID", Table = "WorkFlow", Lazy = true)]
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

        [BelongsTo("ProcessAncestorID", NotNull = false)]
        public virtual Process_DAO ProcessAncestor
        {
            get
            {
                return this._processAncestor;
            }
            set
            {
                this._processAncestor = value;
            }
        }
    }
}