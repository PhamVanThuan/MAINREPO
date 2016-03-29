using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FixedRateAdjustment_DAO
    /// </summary>
    public partial interface IFixedRateAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FixedRateAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FixedRateAdjustment_DAO.Rate
        /// </summary>
        System.Double Rate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FixedRateAdjustment_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }
    }
}