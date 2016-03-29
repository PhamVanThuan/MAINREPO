using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO
    /// </summary>
    public partial interface IAccountDebtSettlement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.SettlementAmount
        /// </summary>
        Double? SettlementAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.SettlementDate
        /// </summary>
        DateTime? SettlementDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.RateApplied
        /// </summary>
        Double? RateApplied
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.InterestStartDate
        /// </summary>
        DateTime? InterestStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.CapitalAmount
        /// </summary>
        Double? CapitalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.GuaranteeAmount
        /// </summary>
        Double? GuaranteeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.AccountExpense
        /// </summary>
        IAccountExpense AccountExpense
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.BankAccount
        /// </summary>
        IBankAccount BankAccount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.Disbursement
        /// </summary>
        IDisbursement Disbursement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.DisbursementInterestApplied
        /// </summary>
        IDisbursementInterestApplied DisbursementInterestApplied
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDebtSettlement_DAO.DisbursementType
        /// </summary>
        IDisbursementType DisbursementType
        {
            get;
            set;
        }
    }
}