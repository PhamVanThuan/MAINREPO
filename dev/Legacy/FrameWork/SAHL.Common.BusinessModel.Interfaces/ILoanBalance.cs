using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO
    /// </summary>
    public partial interface ILoanBalance : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.Term
        /// </summary>
        System.Int32 Term
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.InitialBalance
        /// </summary>
        System.Double InitialBalance
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.RemainingInstalments
        /// </summary>
        System.Int32 RemainingInstalments
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.InterestRate
        /// </summary>
        System.Double InterestRate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.RateAdjustment
        /// </summary>
        System.Double RateAdjustment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.ActiveMarketRate
        /// </summary>
        System.Double ActiveMarketRate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.MTDInterest
        /// </summary>
        System.Double MTDInterest
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.RateConfiguration
        /// </summary>
        IRateConfiguration RateConfiguration
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.ResetConfiguration
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }
    }
}