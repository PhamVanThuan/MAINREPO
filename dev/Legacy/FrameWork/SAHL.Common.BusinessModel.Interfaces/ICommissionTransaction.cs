using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO
    /// </summary>
    public partial interface ICommissionTransaction : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.FinancialServiceKey
        /// </summary>
        System.Int32 FinancialServiceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionCalcAmount
        /// </summary>
        System.Double CommissionCalcAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionAmount
        /// </summary>
        Double? CommissionAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionFactor
        /// </summary>
        Decimal? CommissionFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionType
        /// </summary>
        System.String CommissionType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.KickerCalcAmount
        /// </summary>
        Double? KickerCalcAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.KickerAmount
        /// </summary>
        Double? KickerAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.TransactionDate
        /// </summary>
        System.DateTime TransactionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.BatchRunDate
        /// </summary>
        DateTime? BatchRunDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.FinancialServiceType
        /// </summary>
        IFinancialServiceType FinancialServiceType
        {
            get;
            set;
        }
    }
}