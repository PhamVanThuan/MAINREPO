using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO
    /// </summary>
    public partial interface IReversalProvisionAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO.ReversalPercentage
        /// </summary>
        System.Double ReversalPercentage
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }
    }
}