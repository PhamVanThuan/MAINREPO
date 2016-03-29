using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO
    /// </summary>
    public partial interface IFinancialService : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Payment
        /// </summary>
        System.Double Payment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Trade
        /// </summary>
        ITrade Trade
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.NextResetDate
        /// </summary>
        DateTime? NextResetDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceBankAccounts
        /// </summary>
        IEventList<IFinancialServiceBankAccount> FinancialServiceBankAccounts
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceConditions
        /// </summary>
        IEventList<IFinancialServiceCondition> FinancialServiceConditions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.ManualDebitOrders
        /// </summary>
        IEventList<IManualDebitOrder> ManualDebitOrders
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialAdjustments
        /// </summary>
        IEventList<IFinancialAdjustment> FinancialAdjustments
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceAttributes
        /// </summary>
        IEventList<IFinancialServiceAttribute> FinancialServiceAttributes
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.AccountStatus
        /// </summary>
        IAccountStatus AccountStatus
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Category
        /// </summary>
        ICategory Category
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialTransactions
        /// </summary>
        IEventList<IFinancialTransaction> FinancialTransactions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceParent
        /// </summary>
        IFinancialService FinancialServiceParent
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.OpenDate
        /// </summary>
        DateTime? OpenDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.CloseDate
        /// </summary>
        DateTime? CloseDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServices
        /// </summary>
        IEventList<IFinancialService> FinancialServices
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Balance
        /// </summary>
        IBalance Balance
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.ArrearTransactions
        /// </summary>
        IEventList<IArrearTransaction> ArrearTransactions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Fees
        /// </summary>
        IEventList<IFee> Fees
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.LifePolicy
        /// </summary>
        ILifePolicy LifePolicy
        {
            get;
        }
    }
}