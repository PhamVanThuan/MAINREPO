using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO
    /// </summary>
    public partial interface IWorkflowOrganisationStructure : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.OrganisationStructure
        /// </summary>
        IOrganisationStructure OrganisationStructure
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.WorkflowName
        /// </summary>
        System.String WorkflowName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.ProcessName
        /// </summary>
        System.String ProcessName
        {
            get;
            set;
        }
    }
}