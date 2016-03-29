using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO
    /// </summary>
    public partial interface IDifferentialProvisionAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.DifferentialAdjustment
        /// </summary>
        System.Double DifferentialAdjustment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.BalanceType
        /// </summary>
        IBalanceType BalanceType
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }
    }
}