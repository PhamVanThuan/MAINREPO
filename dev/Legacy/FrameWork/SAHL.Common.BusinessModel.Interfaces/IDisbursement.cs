using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO
    /// </summary>
    public partial interface IDisbursement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.PreparedDate
        /// </summary>
        DateTime? PreparedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ActionDate
        /// </summary>
        DateTime? ActionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.AccountName
        /// </summary>
        System.String AccountName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.AccountNumber
        /// </summary>
        System.String AccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.Amount
        /// </summary>
        Double? Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.CapitalAmount
        /// </summary>
        Double? CapitalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.GuaranteeAmount
        /// </summary>
        Double? GuaranteeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.InterestRate
        /// </summary>
        Double? InterestRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.InterestStartDate
        /// </summary>
        DateTime? InterestStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.InterestApplied
        /// </summary>
        System.Char InterestApplied
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.PaymentAmount
        /// </summary>
        Double? PaymentAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.AccountDebtSettlements
        /// </summary>
        IEventList<IAccountDebtSettlement> AccountDebtSettlements
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.LoanTransactions
        /// </summary>
        IEventList<IFinancialTransaction> LoanTransactions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ACBBank
        /// </summary>
        IACBBank ACBBank
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ACBBranch
        /// </summary>
        IACBBranch ACBBranch
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ACBType
        /// </summary>
        IACBType ACBType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.DisbursementStatus
        /// </summary>
        IDisbursementStatus DisbursementStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.DisbursementTransactionType
        /// </summary>
        IDisbursementTransactionType DisbursementTransactionType
        {
            get;
            set;
        }
    }
}