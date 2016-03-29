using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Groups workflow role types.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("WorkflowRoleTypeGroup", Schema = "dbo", Lazy = true)]
    public partial class WorkflowRoleTypeGroup_DAO : DB_2AM<WorkflowRoleTypeGroup_DAO>
    {
        private int _Key;

        private string _description;

        private GenericKeyType_DAO _genericKeyType;

        private WorkflowOrganisationStructure_DAO _workflowOrganisationStructure;

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "WorkflowRoleTypeGroupKey", ColumnType = "Int32")]
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
        /// The description of the Workflow Role Type Group
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

        [BelongsTo("GenericKeyTypeKey", NotNull = true, Fetch = FetchEnum.Join)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [BelongsTo("WorkflowOrganisationStructureKey", NotNull = true, Fetch = FetchEnum.Join)]
        [ValidateNonEmpty("Workflow Organisation Structure is a mandatory field")]
        public virtual WorkflowOrganisationStructure_DAO WorkflowOrganisationStructure
        {
            get
            {
                return this._workflowOrganisationStructure;
            }
            set
            {
                this._workflowOrganisationStructure = value;
            }
        }
    }
}