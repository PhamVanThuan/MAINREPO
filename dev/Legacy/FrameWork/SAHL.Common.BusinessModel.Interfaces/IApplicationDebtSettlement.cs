using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO
    /// </summary>
    public partial interface IApplicationDebtSettlement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.SettlementAmount
        /// </summary>
        System.Double SettlementAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.SettlementDate
        /// </summary>
        DateTime? SettlementDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.RateApplied
        /// </summary>
        System.Double RateApplied
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.InterestStartDate
        /// </summary>
        DateTime? InterestStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.CapitalAmount
        /// </summary>
        System.Double CapitalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.GuaranteeAmount
        /// </summary>
        System.Double GuaranteeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.BankAccount
        /// </summary>
        IBankAccount BankAccount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.Disbursement
        /// </summary>
        IDisbursement Disbursement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.DisbursementInterestApplied
        /// </summary>
        IDisbursementInterestApplied DisbursementInterestApplied
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.DisbursementType
        /// </summary>
        IDisbursementType DisbursementType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.OfferExpense
        /// </summary>
        IApplicationExpense OfferExpense
        {
            get;
            set;
        }
    }
}