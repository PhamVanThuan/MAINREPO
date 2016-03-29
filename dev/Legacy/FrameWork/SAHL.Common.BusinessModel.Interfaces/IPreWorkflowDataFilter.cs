using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO
    /// </summary>
    public partial interface IPreWorkflowDataFilter : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO.WorkflowDataKey
        /// </summary>
        System.Int32 WorkflowDataKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO.OfferTypeKey
        /// </summary>
        System.Int32 OfferTypeKey
        {
            get;
            set;
        }
    }
}