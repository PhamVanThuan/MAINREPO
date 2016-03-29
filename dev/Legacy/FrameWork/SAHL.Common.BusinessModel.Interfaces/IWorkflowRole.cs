using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IWorkflowRole : IEntityValidation, IBusinessModelObject
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
        /// The details regarding the Legal Entity playing the Workflow Role is stored in the LegalEntity table. This is
        /// the LegalEntityKey for that Legal Entity.
        /// </summary>
        System.Int32 LegalEntityKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO.WorkflowRoleType
        /// </summary>
        IWorkflowRoleType WorkflowRoleType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the status of the Role was last changed.
        /// </summary>
        System.DateTime StatusChangeDate
        {
            get;
            set;
        }
    }
}