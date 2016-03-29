using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This class interacts with the FinancialTransaction view in 2AM that points to the table of the same name in SAHLDB.
    ///
    /// NB: This object should NOT be queried with HQL, it will fall over spectacularly
    /// The FinancialService is built up from the LoanNumber (NotNull = true), this is only valid for Transactions after Nov 2006.
    /// </summary>
    public partial interface IFinancialTransaction : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.InsertDate
        /// </summary>
        System.DateTime InsertDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.CorrectionDate
        /// </summary>
        System.DateTime CorrectionDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.InterestRate
        /// </summary>
        Single? InterestRate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.ActiveMarketRate
        /// </summary>
        Double? ActiveMarketRate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Balance
        /// </summary>
        System.Double Balance
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Reference
        /// </summary>
        System.String Reference
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.SPV
        /// </summary>
        ISPV SPV
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.IsRolledBack
        /// </summary>
        System.Boolean IsRolledBack
        {
            get;
        }
    }
}