using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    ///
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("WorkflowRole", Schema = "dbo", Lazy = true)]
    public partial class WorkflowRole_DAO : DB_2AM<WorkflowRole_DAO>
    {
        private int _key;

        private int _legalEntityKey;

        private int _genericKey;

        private WorkflowRoleType_DAO _WorkflowRoleType;

        private GeneralStatus_DAO _generalStatus;

        private System.DateTime _statusChangeDate;

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "WorkflowRoleKey", ColumnType = "Int32")]
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

        /// <summary>
        /// The details regarding the Legal Entity playing the Workflow Role is stored in the LegalEntity table. This is
        /// the LegalEntityKey for that Legal Entity.
        /// </summary>
        /// <remarks>Exposed as a key for performance reasons.</remarks>
        [Property("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual int LegalEntityKey
        {
            get
            {
                return this._legalEntityKey;
            }
            set
            {
                this._legalEntityKey = value;
            }
        }

        [Property("GenericKey", NotNull = true)]
        [ValidateNonEmpty("GenericKey is a mandatory field")]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [BelongsTo("WorkflowRoleTypeKey", NotNull = true)]
        [ValidateNonEmpty("Workflow Role Type is a mandatory field")]
        public virtual WorkflowRoleType_DAO WorkflowRoleType
        {
            get
            {
                return this._WorkflowRoleType;
            }
            set
            {
                this._WorkflowRoleType = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        /// <summary>
        /// The date on which the status of the Role was last changed.
        /// </summary>
        [Property("StatusChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Status Change Date is a mandatory field")]
        public virtual System.DateTime StatusChangeDate
        {
            get
            {
                return this._statusChangeDate;
            }
            set
            {
                this._statusChangeDate = value;
            }
        }
    }
}