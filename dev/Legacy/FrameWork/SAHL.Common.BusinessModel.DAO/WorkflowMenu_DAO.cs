using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// the DAO class that reflect the workflowmenu data structure.
    /// </summary>
    [ActiveRecord("WorkflowMenu", Schema = "dbo", Lazy = true)]
    public partial class WorkflowMenu_DAO : DB_2AM<WorkflowMenu_DAO>
    {
        private string _workflowName;

        private string _stateName;

        private string _processName;

        private int _key;

        private CBOMenu_DAO _coreBusinessObjectMenu;

        [Property("WorkflowName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Workflow Name is a mandatory field")]
        public virtual string WorkflowName
        {
            get
            {
                return this._workflowName;
            }
            set
            {
                this._workflowName = value;
            }
        }

        [Property("StateName", ColumnType = "String")]
        public virtual string StateName
        {
            get
            {
                return this._stateName;
            }
            set
            {
                this._stateName = value;
            }
        }

        [Property("ProcessName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Process Name is a mandatory field")]
        public virtual string ProcessName
        {
            get
            {
                return this._processName;
            }
            set
            {
                this._processName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "WorkflowMenuKey", ColumnType = "Int32")]
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

        [BelongsTo("CoreBusinessObjectKey", NotNull = true)]
        [ValidateNonEmpty("CoreBusinessObjectMenu is a mandatory field")]
        public virtual CBOMenu_DAO CoreBusinessObjectMenu
        {
            get
            {
                return this._coreBusinessObjectMenu;
            }
            set
            {
                this._coreBusinessObjectMenu = value;
            }
        }
    }
}