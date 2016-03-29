using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO
    /// </summary>
    public partial interface IBatchLoanTransaction : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO.LoanTransactionNumber
        /// </summary>
        System.Int32 LoanTransactionNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO.BatchTransaction
        /// </summary>
        IBatchTransaction BatchTransaction
        {
            get;
            set;
        }
    }
}