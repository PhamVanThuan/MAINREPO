using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("WorkflowOrganisationStructure", Schema = "dbo", Lazy = true)]
    public partial class WorkflowOrganisationStructure_DAO : DB_2AM<WorkflowOrganisationStructure_DAO>
    {
        private int _key;

        private OrganisationStructure_DAO _organisationStructure;

        private string _workflowName;

        private string _processName;

        [PrimaryKey(PrimaryKeyType.Native, "WorkflowOrganisationStructureKey", ColumnType = "Int32")]
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

        [BelongsTo("OrganisationStructureKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Structure is a mandatory field")]
        public virtual OrganisationStructure_DAO OrganisationStructure
        {
            get
            {
                return this._organisationStructure;
            }
            set
            {
                this._organisationStructure = value;
            }
        }

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
    }
}