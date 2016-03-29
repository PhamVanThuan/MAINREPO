using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Groups workflow role types.
    /// </summary>
    public partial interface IWorkflowRoleTypeGroup : IEntityValidation, IBusinessModelObject
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
        /// The description of the Workflow Role Type Group
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRoleTypeGroup_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRoleTypeGroup_DAO.WorkflowOrganisationStructure
        /// </summary>
        IWorkflowOrganisationStructure WorkflowOrganisationStructure
        {
            get;
            set;
        }
    }
}