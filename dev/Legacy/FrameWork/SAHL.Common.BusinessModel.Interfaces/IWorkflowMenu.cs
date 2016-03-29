using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// the DAO class that reflect the workflowmenu data structure.
    /// </summary>
    public partial interface IWorkflowMenu : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.WorkflowName
        /// </summary>
        System.String WorkflowName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.StateName
        /// </summary>
        System.String StateName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.ProcessName
        /// </summary>
        System.String ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.CoreBusinessObjectMenu
        /// </summary>
        ICBOMenu CoreBusinessObjectMenu
        {
            get;
            set;
        }
    }
}