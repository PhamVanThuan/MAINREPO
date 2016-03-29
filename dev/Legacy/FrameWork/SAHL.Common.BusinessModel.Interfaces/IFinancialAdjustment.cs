using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO
    /// </summary>
    public partial interface IFinancialAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FromDate
        /// </summary>
        DateTime? FromDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.EndDate
        /// </summary>
        DateTime? EndDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.CancellationDate
        /// </summary>
        DateTime? CancellationDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialAdjustmentSource
        /// </summary>
        IFinancialAdjustmentSource FinancialAdjustmentSource
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialAdjustmentStatus
        /// </summary>
        IFinancialAdjustmentStatus FinancialAdjustmentStatus
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialAdjustmentType
        /// </summary>
        IFinancialAdjustmentType FinancialAdjustmentType
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.ReversalProvisionAdjustment
        /// </summary>
        IReversalProvisionAdjustment ReversalProvisionAdjustment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.DifferentialProvisionAdjustment
        /// </summary>
        IDifferentialProvisionAdjustment DifferentialProvisionAdjustment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.PaymentAdjustment
        /// </summary>
        IPaymentAdjustment PaymentAdjustment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.InterestRateAdjustment
        /// </summary>
        IInterestRateAdjustment InterestRateAdjustment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FixedRateAdjustment
        /// </summary>
        IFixedRateAdjustment FixedRateAdjustment
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CancellationReason_DAO.CancellationReason
        /// </summary>
        ICancellationReason CancellationReason
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialServiceAttribute
        /// </summary>
        IFinancialServiceAttribute FinancialServiceAttribute
        {
            get;
        }
    }
}