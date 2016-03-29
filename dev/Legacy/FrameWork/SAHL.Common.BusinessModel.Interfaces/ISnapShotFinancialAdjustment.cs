using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO
    /// </summary>
    public partial interface ISnapShotFinancialAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustment
        /// </summary>
        IFinancialAdjustment FinancialAdjustment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustmentSource
        /// </summary>
        IFinancialAdjustmentSource FinancialAdjustmentSource
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustmentType
        /// </summary>
        IFinancialAdjustmentType FinancialAdjustmentType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustmentStatus
        /// </summary>
        IFinancialAdjustmentStatus FinancialAdjustmentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FromDate
        /// </summary>
        DateTime? FromDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.EndDate
        /// </summary>
        DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CancellationDate
        /// </summary>
        DateTime? CancellationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CancellationReasonKey
        /// </summary>
        System.Int32 CancellationReasonKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FRARate
        /// </summary>
        System.Double FRARate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.IRAAdjustment
        /// </summary>
        System.Double IRAAdjustment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.RPAReversalPercentage
        /// </summary>
        System.Double RPAReversalPercentage
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.DPADifferentialAdjustment
        /// </summary>
        System.Double DPADifferentialAdjustment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.DPABalanceType
        /// </summary>
        IBalanceType DPABalanceType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.SnapShotFinancialService
        /// </summary>
        ISnapShotFinancialService SnapShotFinancialService
        {
            get;
            set;
        }
    }
}