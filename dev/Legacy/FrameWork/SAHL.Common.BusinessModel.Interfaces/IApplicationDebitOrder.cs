using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO
    /// </summary>
    public partial interface IApplicationDebitOrder : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.Percentage
        /// </summary>
        System.Double Percentage
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.DebitOrderDay
        /// </summary>
        System.Int32 DebitOrderDay
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.BankAccount
        /// </summary>
        IBankAccount BankAccount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.FinancialServicePaymentType
        /// </summary>
        IFinancialServicePaymentType FinancialServicePaymentType
        {
            get;
            set;
        }
    }
}