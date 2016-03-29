using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Balance_DAO
    /// </summary>
    public partial interface IBalance : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.BalanceType
        /// </summary>
        IBalanceType BalanceType
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.LoanBalance
        /// </summary>
        ILoanBalance LoanBalance
        {
            get;
        }
    }
}