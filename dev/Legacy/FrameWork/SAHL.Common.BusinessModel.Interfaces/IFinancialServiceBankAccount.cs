using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO
    /// </summary>
    public partial interface IFinancialServiceBankAccount : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.Percentage
        /// </summary>
        System.Double Percentage
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.BankAccount
        /// </summary>
        IBankAccount BankAccount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.FinancialServicePaymentType
        /// </summary>
        IFinancialServicePaymentType FinancialServicePaymentType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.PaymentSplitType
        /// </summary>
        IPaymentSplitType PaymentSplitType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.IsNaedoCompliant
        /// </summary>
        bool IsNaedoCompliant
        {
            get;
            set;
        }

        int? ProviderKey
        {
            get;
            set;
        }
    }
}