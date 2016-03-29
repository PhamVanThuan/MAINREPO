using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.WorkflowRoleType_DAO
    /// </summary>
    public partial interface IWorkflowRoleType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The description of the Workflow Role Type
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Determines the Workflow Role Type Group to which the Workflow Role Type belongs.
        /// </summary>
        IWorkflowRoleTypeGroup WorkflowRoleTypeGroup
        {
            get;
            set;
        }
    }
}