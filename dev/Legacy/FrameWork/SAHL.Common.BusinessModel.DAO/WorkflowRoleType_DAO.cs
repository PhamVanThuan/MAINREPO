using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("WorkflowRoleType", Schema = "dbo", Lazy = true)]
    public partial class WorkflowRoleType_DAO : DB_2AM<WorkflowRoleType_DAO>
    {
        private int _Key;

        private string _description;

        private WorkflowRoleTypeGroup_DAO _workflowRoleTypeGroup;

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "WorkflowRoleTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        /// <summary>
        /// The description of the Workflow Role Type
        /// </summary>
        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        /// <summary>
        /// Determines the Workflow Role Type Group to which the Workflow Role Type belongs.
        /// </summary>
        /// <remarks>Fetch type is join as an WorkflowRoleType is almost always used in the context of it's group.</remarks>
        [BelongsTo("WorkflowRoleTypeGroupKey", NotNull = true, Fetch = FetchEnum.Join)]
        [ValidateNonEmpty("Workflow Role Type Group is a mandatory field")]
        public virtual WorkflowRoleTypeGroup_DAO WorkflowRoleTypeGroup
        {
            get
            {
                return this._workflowRoleTypeGroup;
            }
            set
            {
                this._workflowRoleTypeGroup = value;
            }
        }
    }
}